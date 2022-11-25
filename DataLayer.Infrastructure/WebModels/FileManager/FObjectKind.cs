using DataLayer.Domin.Models;
using DataLayer.Infrastructure.Services;
using DataLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer.Infrastructure.WebModels.FileManager
{
    [TSModelUsage(CompileOption = CompileOption.compile)]
    public class FObjectKind
    {
        public Guid Id { get; set; }
        public Guid? FolderId { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        [StringLength(256, ErrorMessage = SharedRegix.SLError, MinimumLength = 3)]
        public string Name { get; set; }
        public FObjectType FObjectType { get; set; }

        public static implicit operator WebFolder(FObjectKind f)
        {
            return new WebFolder
            {
                Name = f.Name,
                Id = f.Id,
                ParentId = f.FolderId
            };
        }
    }
    public enum FObjectType : int
    {
        File,
        Folder
    }
}
