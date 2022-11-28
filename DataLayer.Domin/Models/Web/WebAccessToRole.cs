using DataLayer.Domin.Models.BaseModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models.Web
{
    public class WebAccessToRole : IDeleteBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool IsDeleted { get; set; }
    }
}
