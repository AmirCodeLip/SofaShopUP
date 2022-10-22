using DataLayer.Infrastructure.ViewModel.Form;
using DataLayer.Infrastructure.WebModels;
using DataLayer.Infrastructure.WebModels.FileManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.Infrastructure
{
    public class FileManagerStructure
    {
        public string FileManagerOnLoadData()
        {
            FileManagerOnLoadData vm = new FileManagerOnLoadData
            {
                NewFolderForm = FormManager.GetFromFrom(typeof(FolderInfo))
            };
            return JsonConvert.SerializeObject(vm);
        }
    }
}
