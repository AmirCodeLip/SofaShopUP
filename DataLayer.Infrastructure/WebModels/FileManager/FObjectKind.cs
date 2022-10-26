using DataLayer.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.WebModels.FileManager
{
    [TSModelUsage(CompileOption = CompileOption.compile)]
    public class FObjectKind
    {
        public Guid Id { get; set; }
        public Guid? FolderId { get; set; }
        public string Name { get; set; }
        public FObjectType FObjectType { get; set; }
    }
    public enum FObjectType : int
    {
        File,
        Folder
    }
}
