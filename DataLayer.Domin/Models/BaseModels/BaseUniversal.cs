using DataLayer.Domin.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models.BaseModels
{
    public class BaseUniversal<TId> : BaseLocal<TId>
    {
        public string Culture { get; set; }
    }
}
