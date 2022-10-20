using DataLayer.Domin.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models
{
    public class WebFileVersion : IDeleteBase
    {
        public Guid Id { get; set; }
        public Guid? FileId { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte[] FileData { get; set; }
        public bool IsDeleted { get; set; }
        public WebFile File { get; set; }
    }
}
