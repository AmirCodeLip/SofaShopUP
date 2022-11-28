#if shop_project
using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models;
using DataLayer.Domin.Models.WebShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services.WebShop
{
    public interface ICityAndStateRepository : IBaseRepository<WebCityAndState>
    {
        public IQueryable<WebCityAndState> GetCityAndStateByParentId(int? parentId);

        Task<JsonResponse> AddNewAddress(ShopAddress address, string city, Guid userId);
    }
}
#endif