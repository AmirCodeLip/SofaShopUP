using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.Infrastructure
{
    public class ReactFilesStructure
    {
        public readonly string ReactRoot;
        public ReactFilesStructure()
        {
            ReactRoot = Path.Combine(Directory.GetCurrentDirectory(), "App");
        }
        public string GetCss()
        {
            var d = ReactRoot;
            return "";
        }


    }
}
