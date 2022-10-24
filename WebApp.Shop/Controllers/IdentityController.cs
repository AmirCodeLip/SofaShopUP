﻿using Microsoft.AspNetCore.Mvc;
using DataLayer.Infrastructure.Infrastructure;
using DataLayer.Infrastructure.ViewModel.Form;
using DataLayer.Access.ViewModel;
using Microsoft.AspNetCore.Authorization;
using DataLayer.Infrastructure.WebModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WebApp.Shop.Controllers
{
    public class IdentityController : ControllerBase
    {
        private IdentityStructure _identityManage;

        public IdentityController(IdentityStructure identityManage)
        {
            _identityManage = identityManage;
        }

        [HttpGet]
        public string GetLoginForm() =>
            _identityManage.GetLoginForm();

        [HttpPost]
        public async Task<JsonResponse<LoginOkResult>> PostLogin([FromBody] LoginModel loginModel) =>
            await _identityManage.Login(loginModel, ModelState);

        [HttpPost, Authorize]
        public async Task<JsonResponse<UserPersonalInfo>> GetProfileInfo() => await _identityManage.GetUserProfile(User);
   
    }

}
