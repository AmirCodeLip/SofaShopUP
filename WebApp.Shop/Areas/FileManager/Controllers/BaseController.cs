﻿using DataLayer.Access.ViewModel;
using DataLayer.Infrastructure.Infrastructure;
using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
           Json(await fileManagerStructure.EditFolder(folderInfo, this.CentralizeDataFiller()));

        [HttpPost, Authorize]
        public async Task<IActionResult> Upload(IFormFile file, Guid? folderId)
        {
            await fileManagerStructure.Upload(this.CentralizeDataFiller(), file, folderId);
            return Json(new JsonResponse());
        }

        public async Task<IActionResult> GetFileImage() => await fileManagerStructure.GetFileImage();
    }
}
