using DataLayer.Access.Data;
using DataLayer.Access.Services;
using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models;
using DataLayer.Domin.Models.Identity;
using DataLayer.Infrastructure.Services;
using DataLayer.Infrastructure.ViewModel;
using DataLayer.Infrastructure.WebModels;
using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DataLayer.Infrastructure.Infrastructure
{
    public class FileManagerStructure
    {
        private readonly IWebHostEnvironment hostingEnv;
        private readonly IFolderRepository folderRepository;
        private readonly IFileRepository fileRepository;
        private readonly IFileVersionRepository fileVersionRepository;
        private readonly IActorOrArtistRepository actorOrArtistRepository;
        private readonly IFileVersionActorOrArtistRepository fileVersionActorOrArtistRepository;
        private readonly ApplicationDbContext context;
        private readonly UserManager<WebUser> userManager;

        public FileManagerStructure(IWebHostEnvironment hostingEnv, IFolderRepository folderRepository,
            IFileRepository fileRepository, IFileVersionRepository fileVersionRepository,
            IActorOrArtistRepository actorOrArtistRepository, IFileVersionActorOrArtistRepository fileVersionActorOrArtistRepository,
            ApplicationDbContext context, UserManager<WebUser> userManager)
        {
            
            this.folderRepository = folderRepository;
            this.fileRepository = fileRepository;
            this.hostingEnv = hostingEnv;
            this.userManager = userManager;
            this.context = context;
            this.fileVersionRepository = fileVersionRepository;
            this.actorOrArtistRepository = actorOrArtistRepository;
            this.fileVersionActorOrArtistRepository = fileVersionActorOrArtistRepository;
        }

        public async Task<JsonResponse> EditFolder(FolderInfo folderInfo, CentralizeData centralizeData)
        {
            var result = new JsonResponse();
            if (!centralizeData.modelState.IsValid)
            {
                foreach (var state in centralizeData.modelState)
                {
                    result.AddError(state.Key, state.Value.Errors[0].ErrorMessage);
                }
                return result;
            }
            var user = await userManager.GetUserAsync(centralizeData.httpContext.User);
            folderInfo.FolderId = folderInfo.FolderId.HasValue ? folderInfo.FolderId.Value : (await RootFolderAsync(user.Id)).Id;
            if (folderInfo.Id == Guid.Empty)
            {
                WebFolder folder = folderInfo;
                folder.CreatorId = user.Id;
                await folderRepository.AddAsync(folder);
            }
            else
            {
                var folder = await this.folderRepository.FindAsync(folderInfo.Id);
                folderInfo.Assign(folder);
                this.folderRepository.Update(folder);
            }
            await context.SaveChangesAsync();
            return result;
        }

        public string FileManagerOnLoadData()
        {
            FileManagerOnLoadData vm = new FileManagerOnLoadData
            {
                EditFolderForm = FormManager.GetFromFrom(typeof(FolderInfo))
            };
            return JsonConvert.SerializeObject(vm);
        }

        public async Task<WebFolder> RootFolderAsync(Guid userId)
        {
            var rootFolder = await folderRepository.SingleOrDefaultAsync(f => f.CreatorId == userId && f.ParentId == null && f.Name == "root");
            if (rootFolder == null)
            {
                await folderRepository.AddAsync((rootFolder = new WebFolder
                {
                    Name = "root",
                    CreatorId = userId,
                    ParentId = null
                }));
            }
            return rootFolder;
        }

        public async Task<IQueryable<FObjectKind>> GetFObjectKindsFromFolder(HttpContext httpContext, Guid? folderID)
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            var fixedFolderId = folderID.HasValue ? folderID.Value : (await RootFolderAsync(user.Id)).Id;
            var folders = folderRepository.Where(x => x.ParentId == fixedFolderId).Select(x => new FObjectKind
            {
                Id = x.Id,
                FObjectType = FObjectType.Folder,
                Name = x.Name,
                FolderId = x.ParentId
            });
            var files = fileRepository.Where(x => x.FolderId == fixedFolderId).Select(x => new FObjectKind
            {
                Id = x.Id,
                FObjectType = FObjectType.File,
                Name = x.Name,
                FolderId = x.FolderId
            });
            return folders.Concat(files);
        }

        public async Task<JsonResponse> Upload(CentralizeData centralizeData, IFormFile file, Guid? folderId)
        {
            try
            {
                context.Database.SetCommandTimeout(3600);
                var result = new JsonResponse();
                var user = await userManager.GetUserAsync(centralizeData.httpContext.User);
                var root = await (folderId.HasValue ? folderRepository.FindAsync(folderId) : RootFolderAsync(user.Id));
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var ex = Path.GetExtension(file.FileName);
                var fileInfo = new WebFile
                {
                    CreatedDate = DateTime.Now,
                    Name = fileName,
                    FolderId = root.Id,
                    Extension = ex
                };
                await fileRepository.AddAsync(fileInfo);
                await context.SaveChangesAsync();
                var fileData = new WebFileVersion { FileId = fileInfo.Id };
                var tempPath = Path.Combine(hostingEnv.ContentRootPath, "tempPath");
                var tempFilePath = Path.Combine(tempPath, Guid.NewGuid().ToString()) + ex;
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);
                using (var stream = new FileStream(tempFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    await file.CopyToAsync(stream);
                }
                var info = ProssesFile(tempFilePath);
                fileData.FileData = await File.ReadAllBytesAsync(tempFilePath);
                fileData.Length = fileData.FileData.Length;
                File.Delete(tempFilePath);
                info.FileDescriptor.Descript(fileData);
                await fileVersionRepository.AddAsync(fileData);
                await context.SaveChangesAsync();
                foreach (var actorOrArtists in info.FileDescriptor.ActorOrArtists)
                {
                    await AddActorOrArtists(fileData, actorOrArtists);
                }
                foreach (var infoData in info.InfoData)
                {
                    infoData.ParentId = fileData.Id;
                    await context.WebFileVersions.AddAsync(infoData);
                }
                await context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AddActorOrArtists(WebFileVersion webFileVersion, string actorOrArtists)
        {
            var artist = await actorOrArtistRepository.GetByName(actorOrArtists, Constants.Cultures.EnUS);
            fileVersionActorOrArtistRepository.Add(new WebFileVersionActorOrArtist
            {
                WebActorOrArtistId = artist.Id,
                WebFileVersionId = webFileVersion.Id
            });
            await context.SaveChangesAsync();
        }

        public (List<WebFileVersion> InfoData, FileDescriptor FileDescriptor) ProssesFile(string filePath)
        {
            var ex = Path.GetExtension(filePath);
            List<WebFileVersion> fileVersions = new List<WebFileVersion>();
            FileDescriptor fileDescriptor = new FileDescriptor();

            switch (ex)
            {
                case ".mp3":
                    using (TagLib.File f = TagLib.File.Create(filePath))
                    {
                        fileDescriptor.ActorOrArtists = f.Tag.AlbumArtists.ToList();
                        fileDescriptor.Title = f.Tag.Title;
                        if (f.Tag.Year != 0)
                            fileDescriptor.BroadCastTime = new DateTime((int)f.Tag.Year, 1, 1);
                        foreach (var picture in f.Tag.Pictures)
                        {
                            var pictureEx = SupportedTypes.Images.Mappings.FirstOrDefault(x => x.Value == picture.MimeType);
                            fileVersions.Add(new WebFileVersion
                            {
                                FileData = picture.Data.Data,
                                Extension = pictureEx.Key
                            });
                        }
                    }
                    break;
            }
            return (fileVersions, fileDescriptor);
        }

        public async Task<FileContentResult> GetFileImage(Guid? id)
        {
            if (id.HasValue)
            {
                var file = await fileRepository.AsQueryable().Include(x => x.WebFileVersions).FirstOrDefaultAsync(x => x.Id == id.Value);
                if (SupportedTypes.Images.Mappings.ContainsKey(file.Extension))
                {
                    var webFileVersions = file.WebFileVersions.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    var a = webFileVersions.AllInfoData;
                    return new FileContentResult(webFileVersions.FileData, SupportedTypes.Images.Mappings[file.Extension]);
                }
                else
                {
                    var webFileVersions = file.WebFileVersions.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    var firstImg = await fileVersionRepository.FirstOrDefaultAsync(x => x.ParentId == webFileVersions.Id);
                    if (firstImg != null)
                    {
                        return new FileContentResult(firstImg.FileData, SupportedTypes.All.Mappings[file.Extension]);
                    }

                }
            }
            return new FileContentResult(await File.ReadAllBytesAsync(Path.Combine(hostingEnv.WebRootPath, "default_images", "unknown.png")), SupportedTypes.Images.Mappings[".png"]);
        }
    }
}
