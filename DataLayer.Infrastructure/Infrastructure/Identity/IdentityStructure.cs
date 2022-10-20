using Newtonsoft.Json;
using DataLayer.UnitOfWork;
using DataLayer.Access.Services;
using DataLayer.Access.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using DataLayer.Domin.Models.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using DataLayer.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using DataLayer.Infrastructure.WebModels;

namespace DataLayer.Infrastructure.Infrastructure
{
    public class IdentityStructure
    {
        private readonly IUserStore<WebUser> _userStore;
        private readonly IUserRepository _user;
        private readonly IWebUserSpecificationRepository _webUserSpecification;
        private readonly UserManager<WebUser> _userManager;
        private readonly SignInManager<WebUser> _signInManager;
        private readonly ApplicationSettings _applicationSettings;
        public IdentityStructure(UserManager<WebUser> userManager, IUserStore<WebUser> userStore, SignInManager<WebUser> signInManager, IUserRepository userRepository, IOptions<ApplicationSettings> applicationSettings, IWebUserSpecificationRepository webUserSpecification)
        {
            _userStore = userStore;
            _userManager = userManager;
            _signInManager = signInManager;
            _user = userRepository;
            _applicationSettings = applicationSettings.Value;
            _webUserSpecification = webUserSpecification;

        }

        public string GetLoginForm()
        {
            return JsonConvert.SerializeObject(FormManager.GetFromFrom(typeof(LoginModel)));
        }

        public async Task<JsonResponse<LoginOkResult>> Login(LoginModel loginModel, ModelStateDictionary modelState)
        {
            var result = new JsonResponse<LoginOkResult>();
            if (!modelState.IsValid)
            {
                foreach (var state in modelState)
                {
                    result.AddError(state.Key, state.Value.Errors[0].ErrorMessage);                    
                }
                return result;
            }
            WebUser user = null;
            if (SharedRegix.RgEmail.IsMatch(loginModel.PhoneOrEmail))
            {
                user = _user.FirstOrDefault(x => x.Email == loginModel.PhoneOrEmail);
            }
            else if (SharedRegix.RgPhone.IsMatch(loginModel.PhoneOrEmail))
            {
                user = _user.FirstOrDefault(x => x.PhoneNumber == loginModel.PhoneOrEmail);
            }
            else
            {
                result.AddError("PhoneOrEmail", "فیلد مشابه شماره همراه و یا ایمل نمیباشد");
            }
            if (user == null && result.Status == JsonResponseStatus.Success)
            {
                result.AddError("PhoneOrEmail", "نام کاربری و یا گذرواژه فاقد اعتبار");
            }
            if (result.Status == JsonResponseStatus.Success)
            {
                var loginResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
                if (loginResult.RequiresTwoFactor)
                {
                    throw new NotImplementedException();
                }
                if (loginResult.IsLockedOut)
                {
                    throw new NotImplementedException();
                }
                else if (loginResult.Succeeded)
                {
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim("UserId",user.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddMonths(1),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_applicationSettings.JWT)), SecurityAlgorithms.HmacSha256)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    result.TResult001 = new LoginOkResult
                    {
                        Token = token,
                    };
                }
                else
                {
                    result.AddError("PhoneOrEmail", "نام کاربری و یا گذرواژه فاقد اعتبار");
                }
            }
            return result;
        }

        public async Task<JsonResponse<UserPersonalInfo>> GetUserProfile(ClaimsPrincipal user)
        {
            var result = new JsonResponse<UserPersonalInfo>();
            Guid userId = Guid.Parse(user.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var userData = await _user.FindAsync(userId);
            var userSpecification = await _webUserSpecification.GetUserProfileById(userId);
            result.TResult001 = new UserPersonalInfo
            {
                Name = userSpecification.Name,
                LastName = userSpecification.LastName,
                UserName = userData.UserName
            };
            return result;
        }

    }
}
