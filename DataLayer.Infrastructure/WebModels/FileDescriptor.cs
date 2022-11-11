using DataLayer.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.WebModels
{
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
