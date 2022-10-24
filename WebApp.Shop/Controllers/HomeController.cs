using DataLayer.Infrastructure.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApp.Shop.Neptons;

namespace WebApp.Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly DefaultCreatorStructure defaultCreatorStructure;
        private readonly FileManagerStructure fileManagerStructure;
        public HomeController(IWebHostEnvironment hostingEnv, DefaultCreatorStructure defaultCreatorStructure, FileManagerStructure fileManagerStructure)
        {
            this._hostingEnv = hostingEnv;
            this.fileManagerStructure = fileManagerStructure;
            this.defaultCreatorStructure = defaultCreatorStructure;
        }

        public async Task<string> Index()
        {
            StaticExtensions.AddWebModels(_hostingEnv);
            await defaultCreatorStructure.FastCreate();



            return "home";
        }
       
    }
}
