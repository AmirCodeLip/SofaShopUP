using DataLayer.UnitOfWork.Lanuages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure
{
    public class Constants
    {
        public class Cultures
        {
            public const string EnUS = "en-US";
            public const string FaIR = "fa-IR";

        }

        public class CellerWordList
        {
            public const string PublicWord001Key = nameof(PublicWord001);


            public static Dictionary<string, global::System.Resources.ResourceManager> List =
                  new Dictionary<string, System.Resources.ResourceManager>(new List<KeyValuePair<string, global::System.Resources.ResourceManager>>
                    {
                        new KeyValuePair<string, System.Resources.ResourceManager>(PublicWord001Key , PublicWord001.ResourceManager)
                    });






        }
    }
}

