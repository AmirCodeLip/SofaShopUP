using DataLayer.Access.ViewModel;
using DataLayer.Infrastructure;
using DataLayer.Infrastructure.WebModels;
using DataLayer.UnitOfWork;
using DataLayer.UnitOfWork.Lanuages;
using DataLayer.UnitOfWork.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Shop.ViewModels;

namespace WebApp.Shop.Controllers
{
    public class CultureManagerController : Controller
    {
        public static Version CultureVersion = new Version(0, 0, 0);

        [HttpPost]
        public string GetCultureInfo([FromBody] Tuple<string> info)
        {
            PublicWord001.Culture = ConstTypes.SupportedLanguages.List[ConstTypes.SupportedLanguages.faIR].CultureInfo;
            var culture = global::System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag(info.Item1);
            return JsonConvert.SerializeObject(new CultureInfo
            {
                Culture = info.Item1,
                Rtl = culture.TextInfo.IsRightToLeft,
                Version = CultureVersion.ToString()
            });
        }

        [HttpPost]
        public IActionResult GetList([FromBody] GetListInfo data)
        {
            var wordModels = WordModel.Parse(data.Words);
            var response = new JsonResponse<List<KeyValuePair<string, string>>>();
            response.TResult001 = new List<KeyValuePair<string, string>>();
            var cellers = wordModels.Select(x => x.Celler).Distinct();
            var cultureInfo = global::System.Globalization.CultureInfo.GetCultureInfo(data.CultureInfo.Culture);
            foreach (var celler in cellers)
            {
                var rm = Constants.CellerWordList.List[celler];
                foreach (var wordModel in wordModels.Where(x => x.Celler == celler))
                {
                    response.TResult001.Add(new KeyValuePair<string, string>($"{celler}.{wordModel.Name}", rm.GetString(wordModel.Name, cultureInfo) ?? ""));
                }
            }
            return response.GetJson();
        }
    }
}
