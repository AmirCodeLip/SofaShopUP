using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models.Identity;
using DataLayer.Domin.Works;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using DataLayer.Domin;
using DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Http;
using DataLayer.Access.Services.Identity;

namespace DataLayer.Access.Services.Identity
{
    public class ManageUserService : IManageUserRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly RoleManager<WebRole> _roleManager;
        private readonly UserManager<WebUser> _userManager;

        public ManageUserService(IUserRepository userRepository,
            IRoleRepository roleRepository,
            RoleManager<WebRole> roleManager,
            UserManager<WebUser> userManager)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task AddRoleToUser(string id, string uname)
        {
            var user = _userRepository.FirstOrDefault(x => x.UserName == uname);
            var role = _roleRepository.FirstOrDefault(x => x.Id == Guid.Parse(id));
            var rn = await _userManager.AddToRoleAsync(user, role.Name);
        }

        //public async Task<IEnumerable<UserAndRoles>> GetUserList()
        //{
        //    var userAndRoles = await AwaitAll(_userRepository.GetList()
        //        .Select(async user =>
        //        new UserAndRoles(await _userRepository.GetUserInfo(user))
        //        {
        //            Roles = await _userManager.GetRolesAsync(user)
        //        }));
        //    return userAndRoles;
        //}

        public async Task<IEnumerable<T>> AwaitAll<T>(IEnumerable<Task<T>> ts)
        {
            List<T> tList = new();
            foreach (var item in ts)
            {
                tList.Add(await item);
            }
            return tList;
        }

        public WebAccess GetWebAccess(ClaimsPrincipal claimsPrincipal)
        {
            return new WebAccess(_userManager);
        }

        public async Task<JsonResponse> ChangeUserName(string userName, HttpContext httpContext)
        {
            var vm = new JsonResponse();

            if (string.IsNullOrWhiteSpace(userName))
            {
                vm.InfoData["userNameError"] = string.Format(SharedRegix.RequiredError, "نام کاربری");
                vm.Status = JsonResponseStatus.HaveError;
            }
            else if (userName.Length < 2 || userName.Length > 30)
            {
                vm.InfoData["userNameError"] = string.Format(SharedRegix.SLError, "نام کاربری", 30, 2);
                vm.Status = JsonResponseStatus.HaveError;
            }
            else
            {
                var user = await _userManager.GetUserAsync(httpContext.User);
                user.UserName = _userRepository.GetNewId(userName);
                _userRepository.Update(user);
                //await _userRepository.SaveChangesAsync();
                var personalInfo = new PersonalInfo
                {
                    UserName = user.UserName
                };
                vm.InfoData[nameof(personalInfo.UserJustName)] = personalInfo.UserJustName;
                vm.InfoData[nameof(personalInfo.UserJustTag)] = personalInfo.UserJustTag;
            }
            return vm;
        }

        public async Task<List<WebUser>> Index()
        {
            return await _userRepository.GetListAsync();
        }
    }
}
