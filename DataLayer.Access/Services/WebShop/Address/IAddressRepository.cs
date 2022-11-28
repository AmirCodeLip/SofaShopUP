#if shop_project
using DataLayer.Domin.Models.WebShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services.WebShop
{
    public interface IAddressRepository
    {
        Task<List<ShopAddress>> GetFullAddress(Expression<Func<ShopAddress, bool>> predicate);
    }
}
#endif