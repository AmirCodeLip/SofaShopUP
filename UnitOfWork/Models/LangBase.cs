using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.UnitOfWork.Models
{
    public class LangBase
    {
        public LangBase(string value)
        {
            Value = value;
            CultureInfo = CultureInfo.GetCultureInfo(value);
        }
        public readonly string Value;
        public readonly CultureInfo CultureInfo;
    }
}
