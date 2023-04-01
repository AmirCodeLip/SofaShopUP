using DataLayer.Access.ViewModel;
using DataLayer.Infrastructure.WebModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataLayer.UnitOfWork;

namespace DataLayer.Infrastructure.Infrastructure.Identity
{
    public class PVInfoStructure
    {
        string privatekey = "PVInfo2294KKhstgcssar922keynoyer";
        byte[] iv = new byte[16];

        public JsonResponse<PVInfoModel> Get(string token)
        {
            var data = new JsonResponse<PVInfoModel>();
            using (SymmetricAlgorithm symmetricAlgorithm = Aes.Create())
            {
                ICryptoTransform crypto = symmetricAlgorithm.CreateDecryptor(Encoding.UTF8.GetBytes(privatekey), iv);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(token)))
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
            return data;
        }

        public JsonResponse<string> Set(PVInfoModel pVInfoModel)
        {
            var data = new JsonResponse<string>();
            if (string.IsNullOrWhiteSpace(pVInfoModel.Language))
            {
                pVInfoModel.Language = UnitOfWork.ConstTypes.SupportedLanguages.List[pVInfoModel.Language].Value;
            }
            else
            {
                pVInfoModel.Language = ConstTypes.SupportedLanguages.List[ConstTypes.SupportedLanguages.enUS].Value;
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
                        data.TResult001 = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            return data;
        }
    }
}
