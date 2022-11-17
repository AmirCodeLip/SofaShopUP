using DataLayer.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.WebModels
{
    [TSModelUsage(CompileOption = CompileOption.ignore)]
    public class CultureInfo
    {
        public string Version { get; set; }
        public string Culture { get; set; }
        public bool Rtl { get; set; }
    }
}
