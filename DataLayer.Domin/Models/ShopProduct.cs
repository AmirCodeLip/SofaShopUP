using DataLayer.Domin.Models.Interfaces;
using DataLayer.UnitOfWork;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Domin.Models
{
    public class ShopProduct : IDeleteBase
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        [StringLength(30, ErrorMessage = SharedRegix.SLError, MinimumLength = 3)]
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "فایل عکس")]
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        public string FileAddress { get; set; }
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        public int Price { get; set; }
        public ShopProductCategory Category { get; set; }

    }
}
