using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DataLayer.Access.ViewModel;
using Newtonsoft.Json;
using DataLayer.Infrastructure.Services;

namespace DataLayer.Infrastructure.Services
{
    public class SupportedTypes
    {
        public readonly List<FileStructure> fileStructures;
        public SupportedTypes(string webRootPath)
        {
            var filePath = Path.Combine(webRootPath, "base_definition", "file_structures.json");
            fileStructures = JsonConvert.DeserializeObject<List<FileStructure>>(File.ReadAllText(filePath));
        }
        public FileStructure GetByEx(string ex)
        {
            return this.fileStructures.FirstOrDefault(x => x.FileNameExtensions.Contains(ex.Substring(1, ex.Length - 1)));
        }

        public bool ContainsEX(string ex)
        {
            return this.fileStructures.Any(x => x.FileNameExtensions.Contains(ex.Substring(1, ex.Length - 1).ToLower()));
        }

        public FileStructure GetByMimeType(string mime)
        {
            return this.fileStructures.FirstOrDefault(x => x.FileNameExtensions.Contains(mime));
        }
    }
}


//var allTypes = new ContentTypeProvider();


//Images.Add(new ContentTypeInfo
//{
//    ContentType = "image/png",
//    Ex = ".png",
//    TypeKind = Constants.SupportedTypeKinds.Image,
//    IdList = new byte[][] { new byte[] { 0x89, 0x50, 0x4E, 0x47 } }
//});
//Images.Add(new ContentTypeInfo
//{
//    ContentType = "image/jpg",
//    Ex = ".jpg",
//    TypeKind = Constants.SupportedTypeKinds.Image
//});
//public readonly ContentTypeProvider Images = new ContentTypeProvider();
//public readonly ContentTypeProvider Audios = new ContentTypeProvider();
//public readonly ContentTypeProvider Videos = new ContentTypeProvider();
//public bool ContainsEX(string ex)
//{
//    return Images.ContainsEX(ex) || Audios.ContainsEX(ex) || Videos.ContainsEX(ex);
//}


//}
//public struct ContentTypeInfo
//{
//    public string Ex { get; set; }
//    public string ContentType { get; set; }
//    public byte[][] IdList { get; set; }
//    public string TypeKind { get; set; }

//}
//public class ContentTypeProvider : List<ContentTypeInfo>
//{



//    public ContentTypeInfo? GetByEx(string ex)
//    {
//        if (this.Any(x => x.Ex == ex))
//            return this.FirstOrDefault(x => x.Ex == ex);
//        return null;
//    }
//}

