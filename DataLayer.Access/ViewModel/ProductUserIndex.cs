#if shop_project
using DataLayer.Domin.Models.WebShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.ViewModel
{
    public class ProductUserIndex
    {
        public List<ShopProduct> Products { get; set; }
        public ShopProductCategory ProductCategories { get;  set; }
    }
}
#endif