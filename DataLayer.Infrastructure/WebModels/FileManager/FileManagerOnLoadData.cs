﻿using DataLayer.Infrastructure.ViewModel.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer.Infrastructure.WebModels.FileManager
{
    public class FileManagerOnLoadData
    {
        public FormModel NewFolderForm { get; set; }
    }
}
