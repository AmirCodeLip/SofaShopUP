﻿using DataLayer.Access.ViewModel;
using DataLayer.Infrastructure.Infrastructure;
using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using WebApp.Shop.Neptons;

namespace WebApp.Shop.Areas.FileManager.Controllers
{
    [Area("FileManager")]
    public class BaseController : Controller
    {
        private readonly FileManagerStructure fileManagerStructure;
        public BaseController(FileManagerStructure fileManagerStructure)
        {
            this.fileManagerStructure = fileManagerStructure;
        }

        [Authorize]
        public string FileManagerOnLoadData() => fileManagerStructure.FileManagerOnLoadData();

        [HttpPost, Authorize]
        public async Task<IActionResult> EditFolder([FromBody] FolderInfo folderInfo) =>
           (await fileManagerStructure.EditFolder(folderInfo, this.CentralizeDataFiller())).GetJson();

        [HttpPost, Authorize]
        public async Task<IActionResult> Upload(IFormFile file, [FromBody] Tuple<Guid?> folderId)
        {
            var response = await fileManagerStructure.Upload(this.CentralizeDataFiller(), file, folderId.Item1);
            return response.GetJson();
        }

        public async Task<IActionResult> GetFileImage(Guid? id) => await fileManagerStructure.GetFileImage(id);
    }
}
