using DataLayer.Domin.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models
{
    public class WebFolder: IDeleteBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public WebFolder Folder { get; set; }
        public ICollection<WebFolder> Folders { get; set; }
        public ICollection<WebFile> WebFiles { get; set; }
    }
}
