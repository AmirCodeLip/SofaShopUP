using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models
{
    public class CategorySpecificationBase
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public short PropertyType { get; set; }


        public CategorySpecificationBase GetBase() => new CategorySpecificationBase
        {
            Id = this.Id,
            PropertyType = this.PropertyType,
            Name = this.Name
        };
    }

}
