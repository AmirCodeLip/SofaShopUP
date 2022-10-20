using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public interface IProductCategoryRepository : IBaseRepository<ShopProductCategory>
    {
        Task<BaseServiceResult<ShopProductCategory>> GetEdit(int? id);
        Task SetEdit(ShopProductCategory productCategory);
        ShopProductCategory RequestCategory(string id);
    }
}
