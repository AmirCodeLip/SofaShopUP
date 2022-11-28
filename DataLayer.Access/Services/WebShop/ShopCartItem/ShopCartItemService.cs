#if shop_project
using DataLayer.Access.Data;
using DataLayer.Domin.Models.WebShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services.WebShop
{
    public class ShopCartItemService : BaseService<ShopCartItem>, IShopCartItemRepository
    {
        public ShopCartItemService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
#endif