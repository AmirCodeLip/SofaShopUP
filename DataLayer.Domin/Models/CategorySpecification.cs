using DataLayer.Domin.Models.Interfaces;
using DataLayer.UnitOfWork;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Domin.Models
{ 
    public class ShopProductCategorySpecification : CategorySpecificationBase
    {
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        [StringLength(30, ErrorMessage = SharedRegix.SLError, MinimumLength = 3)]
        public override string Name { get => base.Name; set => base.Name = value; }
        public ShopCategorySpecificationRelation CategorySpecificationRelation { get; set; }

    }
 
    public class ShopCategorySpecificationRelation : IDeleteBase
    {
        public int ProductCategorySpecificationId { get; set; }
        public ShopProductCategorySpecification CategorySpecification { get; set; }
        public int ProductCategoryId { get; set; }
        public ShopProductCategory ProductCategory { get; set; }
        public bool IsDeleted { get; set; }

    }    
}
