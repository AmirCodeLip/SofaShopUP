using DataLayer.UnitOfWork.Models;
using System.Globalization;

namespace DataLayer.UnitOfWork
{
    public partial class ConstTypes
    {
        public class PropertyTypes
        {
            public const string TypeString = "type-string";
            public const string TypeYesOrNo = "type-yesOrNo";
        }

        public class SupportedLanguages
        {
            public const string enUS = "en-US";
            public const string faIR = "fa-IR";
            public static Dictionary<string, LangBase> List
            {
                get
                {
                    var langBase = new Dictionary<string, LangBase>();
                    langBase[enUS] = new LangBase(enUS);
                    langBase[faIR] = new LangBase(faIR);
                    return langBase;
                }
            }
        }
    }
}
