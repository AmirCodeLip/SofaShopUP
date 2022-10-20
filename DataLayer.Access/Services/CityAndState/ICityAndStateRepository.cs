using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public interface ICityAndStateRepository : IBaseRepository<WebCityAndState>
    {
        public IQueryable<WebCityAndState> GetCityAndStateByParentId(int? parentId);

        Task<JsonResponse> AddNewAddress(ShopAddress address, string city, Guid userId);
    }
}
