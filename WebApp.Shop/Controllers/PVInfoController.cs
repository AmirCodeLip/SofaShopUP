using DataLayer.Access.ViewModel;
using DataLayer.Infrastructure.WebModels;
using Newtonsoft.Json;
using DataLayer.UnitOfWork;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData.Edm;
using DataLayer.Infrastructure.Infrastructure.Identity;

namespace WebApp.Shop.Controllers
{
    public class PVInfoController : Controller
    {
        private readonly PVInfoStructure pVInfo;
        public PVInfoController(PVInfoStructure pVInfo)
        {
            this.pVInfo = pVInfo;
        }


        [HttpPost]
        public IActionResult Get([FromBody] Tuple<string> info)
        {
            return pVInfo.Get(info.Item1).GetJson();
        }

        [HttpPost]
        public IActionResult Set([FromBody] PVInfoModel pVInfoModel)
        {
            return pVInfo.Set(pVInfoModel).GetJson();
        }
        //https://www.section.io/engineering-education/encrypt-decrypt-using-rijndael-key-in-c-sharp/
    }
}
