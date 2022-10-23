using DataLayer.Access.ViewModel;
using DataLayer.Infrastructure.Infrastructure;
using DataLayer.Infrastructure.WebModels;
using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.Mvc;

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

        public string FileManagerOnLoadData() => fileManagerStructure.FileManagerOnLoadData();

        [HttpPost]
        public async Task<IActionResult> EditFolder([FromBody] FolderInfo folderInfo) =>
           Json(await fileManagerStructure.EditFolder(folderInfo, ModelState));
    }
}
