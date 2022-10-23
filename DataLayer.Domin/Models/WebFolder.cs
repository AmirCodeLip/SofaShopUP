using DataLayer.Domin.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models
{
    public class WebFolder : IDeleteBase
    {
        public Guid Id { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        [AllowNull]
        public Guid? ParentId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public WebFolder Folder { get; set; }
        public ICollection<WebFolder> Folders { get; set; }
        public ICollection<WebFile> WebFiles { get; set; }
    }
}
