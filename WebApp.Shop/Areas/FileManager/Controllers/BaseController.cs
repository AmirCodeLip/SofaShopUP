using DataLayer.Infrastructure.Infrastructure;
using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Shop.Areas.FileManager.Controllers
{
    [Area("FileManager")]
    public class BaseController : ControllerBase
    {
        private readonly FileManagerStructure fileManagerStructure;
        public BaseController(FileManagerStructure fileManagerStructure)
        {
            this.fileManagerStructure = fileManagerStructure;
        }
        public string FileManagerOnLoadData()
        {
            
            return fileManagerStructure.FileManagerOnLoadData();
        }
    }
}
