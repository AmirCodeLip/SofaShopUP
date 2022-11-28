using DataLayer.Domin.Models.BaseModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models.Web
{
    public class WebFileVersion : IDeleteBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Extension { get; set; }
        public byte[] FileData { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? ParentId { get; set; }
        public WebFileVersion Parent { get; set; }
        public ICollection<WebFileVersion> AllInfoData { get; set; }
        public Guid? FileId { get; set; }
        public WebFile File { get; set; }
        public long Length { get; set; }
        public string Title { get; set; }
        public DateTime? BroadCastTime { get; set; }
        public ICollection<WebFileVersionActorOrArtist> WebFileVersionActorOrArtists { get; set; }
    }
}
