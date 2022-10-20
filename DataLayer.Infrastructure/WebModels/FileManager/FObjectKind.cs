using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.WebModels.FileManager
{
    public class FObjectKind
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FObjectType FObjectType { get; set; }
    }
    public enum FObjectType : int
    {
        File,
        Folder
    }
}
