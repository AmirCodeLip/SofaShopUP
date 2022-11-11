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
        static SupportedTypes()
        {
            var allTypes = new FileExtensionContentTypeProvider();
            All = new FileExtensionContentTypeProvider(new Dictionary<string, string>());
            Images = new FileExtensionContentTypeProvider(new Dictionary<string, string>());
            Audios = new FileExtensionContentTypeProvider(new Dictionary<string, string>());
            Images.Mappings.Clear();
            foreach (var img in new[] { ".png", ".jpg" })
            {
                Images.Mappings.Add(img, allTypes.Mappings[img]);
                All.Mappings.Add(img, allTypes.Mappings[img]);
            }
            foreach (var audio in new[] { ".mp3" })
            {
                Audios.Mappings.Add(audio, allTypes.Mappings[audio]);
                All.Mappings.Add(audio, allTypes.Mappings[audio]);
            }


        }

        public readonly static FileExtensionContentTypeProvider All;
        public readonly static FileExtensionContentTypeProvider Images;
        public readonly static FileExtensionContentTypeProvider Audios;
    }
}
