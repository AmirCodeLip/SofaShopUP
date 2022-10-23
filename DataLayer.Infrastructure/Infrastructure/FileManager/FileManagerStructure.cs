using DataLayer.Access.Data;
using DataLayer.Access.Services;
using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models;
using DataLayer.Infrastructure.ViewModel.Form;
using DataLayer.Infrastructure.WebModels;
using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.Infrastructure
{
    public class FileManagerStructure
    {
        private readonly IFolderRepository folderRepository;
        private readonly ApplicationDbContext _context;
        public FileManagerStructure(IFolderRepository folderRepository, ApplicationDbContext context)
        {
            this.folderRepository = folderRepository;
            _context = context;
        }

        public async Task<JsonResponse> EditFolder(FolderInfo folderInfo, ModelStateDictionary modelState)
        {
            var result = new JsonResponse();
            if (!modelState.IsValid)
            {
                foreach (var state in modelState)
                {
                    result.AddError(state.Key, state.Value.Errors[0].ErrorMessage);
                }
                return result;
            }
            if (folderInfo.Id == Guid.Empty)
            {
                WebFolder folder = folderInfo;
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

        public IQueryable<FObjectKind> GetFObjectKindsFromFolder()
        {
            return folderRepository.AsQueryable().Select(x => new FObjectKind
            {
                Id = x.Id,
                FObjectType = FObjectType.Folder,
                Name = x.Name,
            });
        }
    }
}
