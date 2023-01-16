using DataLayer.Domin.Models.BaseModels.Interfaces;
using DataLayer.Domin.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models.Web
{
    public class WebFile : IDeleteBase
    {
        public Guid Id { get; set; }
        [StringLength(256, MinimumLength = 3)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Extension { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(1000)]
        public string Path { get; set; }
        public string TypeKind { get; set; }
        public long Size { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatorId { get; set; }
        public WebUser WebUser { get; set; }
        public Guid? FolderId { get; set; }
        public WebFolder Folder { get; set; }
        public ICollection<WebFileVersion> WebFileVersions { get; set; }
    }
}
