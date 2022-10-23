using DataLayer.Domin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer.Infrastructure.WebModels.FileManager
{
    public class FolderInfo
    {
        public Guid Id { get; set; }
        [Display(Name = "نام فولدر")]
        public string FolderName { get; set; }

        public static implicit operator WebFolder(FolderInfo folderInfo)
        {
            var webFolder = new WebFolder();
            folderInfo.Assign(webFolder);
            return webFolder;
        }

        public void Assign(WebFolder webFolder)
        {
            webFolder.Id = Id;
            webFolder.Name = FolderName;
        }

    }
}
