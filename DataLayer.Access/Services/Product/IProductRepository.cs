using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public interface IProductRepository : IBaseRepository<ShopProduct>
    {
        Task<List<ShopProduct>> ListProductAsync();
        SelectList GetProductCategorySelectList();
        Task<BaseServiceResult<ShopProduct>> GetEdit(int? id);
        Task SetEdit(ShopProduct product);
        BaseServiceResult<ProductUserIndex> UserIndex(string id);

    }
}
