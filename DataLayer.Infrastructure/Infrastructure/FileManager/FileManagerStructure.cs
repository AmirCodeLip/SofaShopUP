using DataLayer.Access.Data;
using DataLayer.Access.Services;
using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models;
using DataLayer.Domin.Models.Identity;
using DataLayer.Infrastructure.ViewModel.Form;
using DataLayer.Infrastructure.WebModels;
using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using DataLayer.Infrastructure.ViewModel;

namespace DataLayer.Infrastructure.Infrastructure
{
    public class FileManagerStructure
    {
        private readonly IFolderRepository folderRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<WebUser> _userManager;
        public FileManagerStructure(IFolderRepository folderRepository, ApplicationDbContext context, UserManager<WebUser> userManager)
        {
            this.folderRepository = folderRepository;
            _userManager = userManager;
            _context = context;
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
            var user = await _userManager.GetUserAsync(centralizeData.httpContext.User);
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
            _context.SaveChanges();
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
            var user = await _userManager.GetUserAsync(httpContext.User);
            var fixedFolderId = folderID.HasValue ? folderID.Value : (await RootFolderAsync(user.Id)).Id;
            var folders = folderRepository.Where(x => x.ParentId == fixedFolderId).Select(x => new FObjectKind
            {
                Id = x.Id,
                FObjectType = FObjectType.Folder,
                Name = x.Name,
            });
            return folders;
        }
    }
}
