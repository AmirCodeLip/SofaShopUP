using DataLayer.Domin.Models.BaseModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models.Web
{
    public class WebFile : IDeleteBase
    {
        public Guid Id { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        public string Extension { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? FolderId { get; set; }
        public WebFolder Folder { get; set; }
        public ICollection<WebFileVersion> WebFileVersions { get; set; }
    }
}
