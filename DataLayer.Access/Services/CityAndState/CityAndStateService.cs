using DataLayer.Access.Data;
using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models;
using DataLayer.Domin.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public class CityAndStateService : BaseService<WebCityAndState>, ICityAndStateRepository
    {
        public CityAndStateService(ApplicationDbContext context, UserManager<WebUser> userManager) : base(context)
        {

        }

        public IQueryable<WebCityAndState> GetCityAndStateByParentId(int? parentId)
        {
            return Where(x => parentId.HasValue ? x.CityAndStateId == parentId.Value : x.CityAndStateId == null);
        }

        public async Task<JsonResponse> AddNewAddress(ShopAddress address, string city, Guid userId)
        {
            var vm = new JsonResponse();
            WebCityAndState cs = null;
            if ((cs = _context.WebCityAndStates.FirstOrDefault(x => x.CityAndStateId.HasValue && x.Name == city)) == null)
            {
                vm.InfoData["City"] = "لطفا شهر را انتخاب کنید";
                vm.Status = JsonResponseStatus.HaveError;

            }
            else if (address.Id != 0)
            {
                var sp = _context.WebUserSpecifications.FirstOrDefault(x => x.UserId == userId);
                if (_context.ShopAddreses.FirstOrDefault(x => x.Id == address.Id).UserSpecificationId != sp.UserId)
                {
                    vm.InfoData["NameAndLastName"] = "اطلاعات تعلق به فرد دیگری دارد";
                    vm.Status = JsonResponseStatus.HaveError;
                }
                else
                {
                    _context.ChangeTracker.Clear();
                    address.CityAndStateId = cs.Id;
                    address.UserSpecificationId = userId;
                    _context.ShopAddreses.Update(address);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                var sp = _context.WebUserSpecifications.FirstOrDefault(x => x.UserId == userId);
                if (sp == null)
                {
                    sp = new WebUserSpecification();
                    await _context.AddAsync(sp);
                    await _context.SaveChangesAsync();
                }
                address.CityAndStateId = cs.Id;
                address.UserSpecificationId = userId;
                await _context.AddAsync(address);
                await _context.SaveChangesAsync();

            }

            return vm;
        }
    }
}
