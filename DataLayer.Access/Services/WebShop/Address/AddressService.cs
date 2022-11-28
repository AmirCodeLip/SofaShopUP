#if shop_project
using DataLayer.Access.Data;
using DataLayer.Domin.Models.WebShop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services.WebShop
{
    public class AddressService : BaseService<ShopAddress>, IAddressRepository
    {
        public AddressService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<ShopAddress>> GetFullAddress(Expression<Func<ShopAddress, bool>> predicate)
        {
            var allAddress = await AsQueryable().
                Include(x => x.CityAndState).
                Include(x => x.CityAndState.CityAndStateItem).
                Where(predicate).ToListAsync();
            return allAddress;
        }

    }
}
#endif