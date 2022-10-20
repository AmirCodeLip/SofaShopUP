using DataLayer.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public interface IAddressRepository
    {
        Task<List<ShopAddress>> GetFullAddress(Expression<Func<ShopAddress, bool>> predicate);
    }
}
