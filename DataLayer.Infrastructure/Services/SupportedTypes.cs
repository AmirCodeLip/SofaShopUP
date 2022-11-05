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
            Images = new FileExtensionContentTypeProvider(new Dictionary<string, string>());
            Images.Mappings.Clear();
            foreach (var img in new[] { ".png" })
            {
                Images.Mappings.Add(img, allTypes.Mappings[img]);
            }
        }

        public readonly static FileExtensionContentTypeProvider Images;

    }
}
