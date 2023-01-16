using DataLayer.Domin.Models.Web;
using DataLayer.Infrastructure.Services;
using DataLayer.Infrastructure.WebModels.FileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.WebModels
{
    [TSModelUsage(CompileOption = CompileOption.ignore)]
    public class FileDescriptor
    {
        public List<string> ActorOrArtists { get; set; }
        public string Title { get; set; }
        public DateTime? BroadCastTime { get; set; }
        public void Descript(WebFileVersion fileData)
        {
            fileData.Title = this.Title;
            fileData.BroadCastTime = this.BroadCastTime;
        }
    }
}
