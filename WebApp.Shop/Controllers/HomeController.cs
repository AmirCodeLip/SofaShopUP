using DataLayer.Infrastructure.Infrastructure;
using DataLayer.Infrastructure.Services;
using DataLayer.UnitOfWork.Lanuages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using WebApp.Shop.Neptons;
using DataLayer.UnitOfWork;
using TagLib.Ogg.Codecs;

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

        public async Task<IActionResult> Index()
        {
            //TSModelGeneratore.AddNewsGeneratorWebModel();
            //await defaultCreatorStructure.FastCreate();

            //PublicWord001.Culture = ConstTypes.SupportedLanguages.List[ConstTypes.SupportedLanguages.faIR].CultureInfo;
            return View();
        }

    }
}
