using DataLayer.Access.ViewModel;
using DataLayer.Infrastructure.WebModels;
using Newtonsoft.Json;
using DataLayer.UnitOfWork;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData.Edm;

namespace WebApp.Shop.Controllers
{
    public class PVInfoController : Controller
    {
        string privatekey = "PVInfo2294KKhstgcssar922keynoyer";

        byte[] iv = new byte[16];

        [HttpPost]
        public IActionResult Get([FromBody] Tuple<string> info)
        {
            var data = new JsonResponse<PVInfoModel>();
            using (SymmetricAlgorithm symmetricAlgorithm = Aes.Create())
            {
                ICryptoTransform crypto = symmetricAlgorithm.CreateDecryptor(Encoding.UTF8.GetBytes(privatekey), iv);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(info.Item1)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, crypto, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(csDecrypt))
                        {
                            data.TResult001 = JsonConvert.DeserializeObject<PVInfoModel>(sr.ReadToEnd());
                        }
                    }
                }
            }
            return data.GetJson();
        }

        [HttpPost]
        public IActionResult Set([FromBody] PVInfoModel pVInfoModel)
        {
            var data = new JsonResponse<string>();
            if (string.IsNullOrWhiteSpace(pVInfoModel.Language))
            {
                var language = ConstTypes.SupportedLanguages.List[pVInfoModel.Language];
            }
            string clearItem = JsonConvert.SerializeObject(pVInfoModel);
            using (SymmetricAlgorithm symmetricAlgorithm = Aes.Create())
            {
                ICryptoTransform crypto = symmetricAlgorithm.CreateEncryptor(Encoding.UTF8.GetBytes(privatekey), iv);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, crypto, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(csEncrypt))
                        {
                            sw.WriteLine(clearItem);
                        }
                        //data.TResult001 = Encoding.UTF8.GetString(msEncrypt.ToArray());
                        data.TResult001 = Convert.ToBase64String(msEncrypt.ToArray());
                        //var t = Get();
                    }
                }
            }
           ;
            return data.GetJson();
        }
        //https://www.section.io/engineering-education/encrypt-decrypt-using-rijndael-key-in-c-sharp/
    }
}
