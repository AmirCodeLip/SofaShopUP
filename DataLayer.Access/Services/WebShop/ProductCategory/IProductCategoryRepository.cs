#if shop_project
using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models.WebShop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services.WebShop
{
    public interface IProductCategoryRepository : IBaseRepository<ShopProductCategory>
    {
        Task<BaseServiceResult<ShopProductCategory>> GetEdit(int? id);
        Task SetEdit(ShopProductCategory productCategory);
        ShopProductCategory RequestCategory(string id);
    }
}
#endif