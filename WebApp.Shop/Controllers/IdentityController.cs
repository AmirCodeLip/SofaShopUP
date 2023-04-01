using Microsoft.AspNetCore.Mvc;
using DataLayer.Infrastructure.Infrastructure.Identity;
using DataLayer.Infrastructure.ViewModels.Form;
using DataLayer.Access.ViewModel;
using Microsoft.AspNetCore.Authorization;
using DataLayer.Infrastructure.WebModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WebApp.Shop.Controllers
{
    public class IdentityController : Controller
    {
        private IdentityStructure _identityManage;
        public IdentityController(IdentityStructure identityManage)
        {
            _identityManage = identityManage;
        }

        [HttpGet]
        public string GetIdentityForm() =>
            _identityManage.GetIdentityForm();

        [HttpPost]
        public async Task<IActionResult> PostLogin([FromBody] LoginModel loginModel) =>
            await _identityManage.Login(loginModel, ModelState); 
        
        [HttpPost]
        public async Task<IActionResult> PostRegister([FromBody] RegisterModel registerModel) =>
            await _identityManage.Register(registerModel, ModelState);

        [HttpPost, Authorize]
        public async Task<JsonResponse<UserPersonalInfo>> GetProfileInfo() => await _identityManage.GetUserProfile(User);   
    }

}
