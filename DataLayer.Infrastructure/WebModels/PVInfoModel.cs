using DataLayer.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.WebModels
{
    [TSModelUsage(CompileOption = CompileOption.compile)]
    public class PVInfoModel
    {
        public string Language { get; set; }
        public List<UserInfo> UserInfoList { get; set; }
    }
}
