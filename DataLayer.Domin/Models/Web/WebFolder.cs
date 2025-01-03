﻿using DataLayer.Domin.Models.BaseModels.Interfaces;
using DataLayer.Domin.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models.Web
{
    public class WebFolder : IDeleteBase
    {
        public Guid Id { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        [MaxLength(1000)]
        public string Path { get; set; }
        public long Size { get; set; }
        [AllowNull]
        public Guid? ParentId { get; set; }
        public WebFolder Folder { get; set; }
        public Guid CreatorId { get; set; }
        public WebUser WebUser { get; set; }
        public ICollection<WebFolder> Folders { get; set; }
        public ICollection<WebFile> WebFiles { get; set; }
    }
}
