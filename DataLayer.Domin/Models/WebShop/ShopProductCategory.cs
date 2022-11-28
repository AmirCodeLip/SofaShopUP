
#if shop_project
using DataLayer.Domin.Models.Interfaces;
using System.ComponentModel;
namespace DataLayer.Domin.Models.WebShop
{
    public class ShopProductCategory : IDeleteBase
    {
        public int Id { get; set; }
        [DisplayName("نام دسته بندی")]
        public string FaName { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ShopProduct> Products { get; set; }
    }
}
#endif