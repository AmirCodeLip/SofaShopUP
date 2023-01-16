using DataLayer.Access.Data;
using DataLayer.Access.Services.Web;
using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models.Identity;
using DataLayer.Domin.Models.Web;
using DataLayer.Infrastructure.Services;
using DataLayer.Infrastructure.ViewModels;
using DataLayer.Infrastructure.WebModels;
using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using static DataLayer.Infrastructure.Constants;

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
        private readonly SupportedTypes supportedTypes;
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
            supportedTypes = new SupportedTypes();
        }

        public async Task<JsonResponse> EditFObject(FObjectKind fObjectKindInfo, CentralizeData centralizeData)
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
            fObjectKindInfo.FolderId = fObjectKindInfo.FolderId.HasValue ? fObjectKindInfo.FolderId.Value : (await RootFolderAsync(user.Id)).Id;
            if (fObjectKindInfo.FObjectType == FObjectType.File)
            {
                var file = await this.fileRepository.FindAsync(fObjectKindInfo.Id);
                file.Name = fObjectKindInfo.Name;
                fileRepository.Update(file);
            }
            else
            {
                if (fObjectKindInfo.Id == Guid.Empty)
                {
                    WebFolder folder = fObjectKindInfo;
                    folder.CreatorId = user.Id;
                    await SetFolderPath(folder);
                    await folderRepository.AddAsync(folder);
                }
                else
                {
                    var folder = await this.folderRepository.FindAsync(fObjectKindInfo.Id);
                    if (folder.Name != fObjectKindInfo.Name)
                        folder.Name = fObjectKindInfo.Name;
                    this.folderRepository.Update(folder);
                }
            }
            await context.SaveChangesAsync();
            return result;
        }

        public string FileManagerOnLoadData()
        {
            FileManagerOnLoadData vm = new FileManagerOnLoadData
            {
                EditFolderOrFileForm = FormManager.GetFromFrom(typeof(FObjectKind))
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
                    ParentId = null,
                    Path = "root"
                }));
            }
            return rootFolder;
        }

        public async Task<IQueryable<FObjectKind>> GetFObjectKindsFromFolder(HttpContext httpContext, string folderID)
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            Guid? fixedFolderId = null;
            if (!string.IsNullOrEmpty(folderID))
            {
                if (folderID == "root")
                    fixedFolderId = (await RootFolderAsync(user.Id)).Id;
                else if (!SupportedTypeKinds.ListItem.Values.Contains(folderID))
                    fixedFolderId = Guid.Parse(folderID);
            }

            var qFolder = folderRepository.Where(x => x.CreatorId == user.Id);
            var qFile = fileRepository.Where(x => x.CreatorId == user.Id);
            if (fixedFolderId.HasValue)
            {
                qFolder = qFolder.Where(x => x.ParentId == fixedFolderId.Value);
                qFile = qFile.Where(x => x.FolderId == fixedFolderId.Value);
            }
            var folders = qFolder.Where(x => x.ParentId == fixedFolderId).Select(x => new FObjectKind
            {
                Id = x.Id,
                FObjectType = FObjectType.Folder,
                Name = x.Name,
                FolderId = x.ParentId,
                Path = x.Path,
                TypeKind = "0"
            });
            var files = qFile.Where(x => x.FolderId == fixedFolderId).Select(x => new FObjectKind
            {
                Id = x.Id,
                FObjectType = FObjectType.File,
                Name = x.Name,
                FolderId = x.FolderId,
                Path = x.Path,
                TypeKind = x.TypeKind
            });
            return folders.Concat(files);
        }

        public async Task<JsonResponse> Upload(CentralizeData centralizeData, IFormFile file, Guid? folderId)
        {
            try
            {
                context.Database.SetCommandTimeout(10000);
                var result = new JsonResponse();
                var user = await userManager.GetUserAsync(centralizeData.httpContext.User);
                var root = await (folderId.HasValue ? folderRepository.FindAsync(folderId) : RootFolderAsync(user.Id));
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var ex = Path.GetExtension(file.FileName);
                var fileInfo = new WebFile
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Name = fileName,
                    FolderId = root.Id,
                    Extension = ex
                };
                if (!supportedTypes.ContainsEX(ex))
                {
                    result.AddError(UploadErrorCode.NotSupportedFile.ToString(), "");
                    return result;
                }
                var tempPath = Path.Combine(hostingEnv.ContentRootPath, "tempPath");
                var tempFilePath = Path.Combine(tempPath, Guid.NewGuid().ToString()) + ex;
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);
                using (var stream = new FileStream(tempFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    await file.CopyToAsync(stream);
                }
                var info = ProssesFile(tempFilePath);
                var contentTypeInfo = supportedTypes.GetByEx(ex);
                fileInfo.TypeKind = contentTypeInfo?.TypeKind;
                await fileRepository.AddAsync(fileInfo);
                await context.SaveChangesAsync();
                var fileData = new WebFileVersion { FileId = fileInfo.Id };
                fileData.FileData = await File.ReadAllBytesAsync(tempFilePath);
                fileData.Length = fileData.FileData.Length;
                File.Delete(tempFilePath);
                info.FileDescriptor.Descript(fileData);
                await fileVersionRepository.AddAsync(fileData);
                await context.SaveChangesAsync();
                if (info.FileDescriptor.ActorOrArtists != null)
                {
                    foreach (var actorOrArtists in info.FileDescriptor.ActorOrArtists)
                    {
                        await AddActorOrArtists(fileData, actorOrArtists);
                    }
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
                            var pictureEx = supportedTypes.Images.GetByMime(picture.MimeType);
                            if (pictureEx != null)
                                fileVersions.Add(new WebFileVersion
                                {
                                    FileData = picture.Data.Data,
                                    Extension = pictureEx?.Ex
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
                if (supportedTypes.ContainsEX(file.Extension))
                {
                    var webFileVersions = file.WebFileVersions.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    var t = supportedTypes.GetByEx(file.Extension);
                    return new FileContentResult(webFileVersions.FileData, supportedTypes.GetByEx(file.Extension)?.ContentType);
                }
                else
                {
                    var webFileVersions = file.WebFileVersions.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    var firstImg = await fileVersionRepository.FirstOrDefaultAsync(x => x.ParentId == webFileVersions.Id);
                    if (firstImg != null)
                    {
                        return new FileContentResult(firstImg.FileData, supportedTypes.GetByEx(file.Extension)?.ContentType);
                    }

                }
            }
            return new FileContentResult(await File.ReadAllBytesAsync(Path.Combine(hostingEnv.WebRootPath, "default_images", "unknown.png")), supportedTypes.GetByEx(".png")?.ContentType);
        }

        public async Task SetFolderPath(WebFolder webFolder)
        {
            var paths = new List<string>();
            if (webFolder.ParentId.HasValue)
            {
                var folder = webFolder;
                while (true)
                {
                    paths.Add(folder.Name);
                    if (!folder.ParentId.HasValue)
                        break;
                    folder = await folderRepository.FindAsync(folder.ParentId);
                }
            }
            else
            {
                paths.Add("root");
            }
            paths.Reverse();
            webFolder.Path = string.Join("/", paths);
        }


    }
}
