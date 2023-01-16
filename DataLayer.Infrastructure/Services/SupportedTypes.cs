using DataLayer.Infrastructure.WebModels.FileManager;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.Services
{
    public class SupportedTypes
    {
        public SupportedTypes()
        {
            var allTypes = new ContentTypeProvider();
            Images.Add(new ContentTypeInfo
            {
                ContentType = "image/png",
                Ex = ".png",
                TypeKind = Constants.SupportedTypeKinds.Image,
                IdList = new byte[][] { new byte[] { 0x89, 0x50, 0x4E, 0x47 } }
            });
            Images.Add(new ContentTypeInfo
            {
                ContentType = "image/jpg",
                Ex = ".jpg",
                TypeKind = Constants.SupportedTypeKinds.Image
            });
        }
        public readonly ContentTypeProvider Images = new ContentTypeProvider();
        public readonly ContentTypeProvider Audios = new ContentTypeProvider();
        public readonly ContentTypeProvider Videos = new ContentTypeProvider();
        public bool ContainsEX(string ex)
        {
            return Images.ContainsEX(ex) || Audios.ContainsEX(ex) || Videos.ContainsEX(ex);
        }

        public ContentTypeInfo? GetByEx(string ex)
        {
            if (Images.ContainsEX(ex)) return Images.GetByEx(ex);
            if (Audios.ContainsEX(ex)) return Audios.GetByEx(ex);
            if (Videos.ContainsEX(ex)) return Videos.GetByEx(ex);
            return null;
        }
    }
    public struct ContentTypeInfo
    {
        public string Ex { get; set; }
        public string ContentType { get; set; }
        public byte[][] IdList { get; set; }
        public string TypeKind { get; set; }

    }
    public class ContentTypeProvider : List<ContentTypeInfo>
    {
        public bool ContainsEX(string ex)
        {
            var localEX = ex.ToLower();
            if (localEX == ".jpeg")
                localEX = ".jpg";
            return this.Any(x => x.Ex == localEX);
        }

        public ContentTypeInfo? GetByMime(string mime)
        {
            if (this.Any(x => x.ContentType == mime))
                return this.FirstOrDefault(x => x.ContentType == mime);
            return null;
        }

        public ContentTypeInfo? GetByEx(string ex)
        {
            if (this.Any(x => x.Ex == ex))
                return this.FirstOrDefault(x => x.Ex == ex);
            return null;
        }
    }

}
