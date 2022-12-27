﻿using DataLayer.Access.Services.Base;
using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DataLayer.Access.Services
{
    public interface IUserRepository : IBaseRepository<WebUser>
    {
        Task<(bool success, Dictionary<string, string> errors)> RegisterAsync(Register Input, ILogger logger);


        Task<WebUser> GetUserAsync(ClaimsPrincipal principal);

        Guid GetUserId(HttpContext httpContext);

        Task SignOutAsync();

        bool IsSignedIn(ClaimsPrincipal user);

        WebUser CreateUser();

        string GetNewId(string userName);


    }
}