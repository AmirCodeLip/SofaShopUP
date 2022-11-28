using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models.Identity;
using DataLayer.Domin.Works;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services.Identity
{
    public interface IManageUserRepository
    {
        Task AddRoleToUser(string id, string uname);

        Task<List<WebUser>> Index();

        //Task<IEnumerable<UserAndRoles>> GetUserList();

        WebAccess GetWebAccess(ClaimsPrincipal claimsPrincipal);

        Task<JsonResponse> ChangeUserName(string userName, HttpContext httpContext);
    }
}
