using DataLayer.Infrastructure.WebModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApp.Shop.Controllers
{
    public class CultureManagerController : Controller
    {
        public static Version CultureVersion = new Version(0, 0, 0);

        public string GetCultureInfo([FromBody] Tuple<string> info)
        {
            return JsonConvert.SerializeObject(new CultureInfo
            {
                Culture = info.Item1,
                Rtl = false,
                Version = CultureVersion.ToString()
            });
        }
    }
}
