using DataLayer.Domin.Models.BaseModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models.BaseModels.Implements
{
    public class BaseLocal<TId> : IDeleteBase
    {
        public TId Id { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime CreatedDate { get; set; }
    }
}
