using System.Collections.Generic;
using System.ComponentModel;
using DataLayer.Domin.Models.Interfaces;

namespace DataLayer.Domin.Models
{
    public class ShopProductCategory : IDeleteBase
    {
        public int Id { get; set; }
        [DisplayName("نام دسته بندی")]
        public string FaName { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ShopProduct> Products { get; set; }
        public string GetId =>
         "Cate" + Id;
        public ICollection<ShopCategorySpecificationRelation> CategorySpecificationRelations { get; set; }
    }
}
