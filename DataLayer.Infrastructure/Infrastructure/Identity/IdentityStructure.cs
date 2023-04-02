using Newtonsoft.Json;
using DataLayer.UnitOfWork;
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
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Access.Services.Identity;
using DataLayer.Access.Services;
using DataLayer.Infrastructure.ViewModels;

namespace DataLayer.Infrastructure.Infrastructure.Identity
{
    public class IdentityStructure
    {
        private readonly IUserStore<WebUser> _userStore;
        private readonly IUserRepository _user;
        private readonly IWebUserSpecificationRepository _webUserSpecification;
        private readonly UserManager<WebUser> _userManager;
        private readonly SignInManager<WebUser> _signInManager;
        private readonly ApplicationSettings _applicationSettings;
        private readonly PVInfoStructure _pVInfoStructure;
        private JwtSecurityTokenHandler tokenHandler { get; }

        public IdentityStructure(UserManager<WebUser> userManager, IUserStore<WebUser> userStore, SignInManager<WebUser> signInManager, IUserRepository userRepository, IOptions<ApplicationSettings> applicationSettings, IWebUserSpecificationRepository webUserSpecification, PVInfoStructure pVInfoStructure)
        {
            _userStore = userStore;
            _userManager = userManager;
            _signInManager = signInManager;
            _user = userRepository;
            _applicationSettings = applicationSettings.Value;
            _webUserSpecification = webUserSpecification;
            _pVInfoStructure = pVInfoStructure;
            this.tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GetIdentityForm()
        {
            return JsonConvert.SerializeObject(new
            {
                LoginModel = FormManager.GetFromFrom(typeof(LoginModel)),
                RegisterModel = FormManager.GetFromFrom(typeof(RegisterModel))
            });
        }

        public async Task AddTokenToUser(PVInfoModel pVInfo, string tokenStr)
        {
            if (pVInfo.UserInfoList == null)
                pVInfo.UserInfoList = new List<UserInfo>();
            var userId = tokenHandler.ReadJwtToken(tokenStr).Claims.FirstOrDefault(x => x.Type == "UserId").Value;
            var user = await this.GetUserWithSpecification(Guid.Parse(userId));
            var preData = pVInfo.UserInfoList.FirstOrDefault(x => x.UserName == user.Email || x.UserName == user.PhoneNumber);
            foreach (var infoList in pVInfo.UserInfoList)
            {
                infoList.IsDefault = false;
            }
            if (preData == null)
            {
                pVInfo.UserInfoList.Add(new UserInfo
                {
                    IsDefault = true,
                    Token = tokenStr,
                    UserName = string.IsNullOrWhiteSpace(user.Email) ? user.PhoneNumber : user.Email,
                    Name = user.UserSpecification.Name ?? "No Name"
                });
            }
            else
            {
                preData.IsDefault = true;
                preData.Token = tokenStr;
            }
        }

        public async Task<WebUser> GetUserWithSpecification(Guid userId)
        {
            var user = await _user.FindAsync(userId);
            user.UserSpecification = _webUserSpecification.FirstOrDefault(x => x.UserId == userId);
            if (user.UserSpecification == null)
            {
                user.UserSpecification = new WebUserSpecification { UserId = user.Id };
                await _webUserSpecification.AddAsync(user.UserSpecification);
                await _webUserSpecification.SaveChangesAsync();
            }
            return user;
        }

        public async Task<ContentResult> Register(RegisterModel registerModel, ModelStateDictionary modelState)
        {
            var result = new JsonResponse<LoginOkResult>();
            var pvTokenRaw = _pVInfoStructure.Get(registerModel.Token).TResult001;
            bool isValidEmail = false;
            bool isValidPhone = false;

            if (modelState.IsValid)
            {
                if (!(isValidEmail = SharedRegix.RgEmail.IsMatch(registerModel.PhoneOrEmail)) && !(isValidPhone = SharedRegix.RgPhone.IsMatch(registerModel.PhoneOrEmail)))
                {
                    result.AddError("PhoneOrEmail", "فیلد مشابه شماره همراه و یا ایمل نمیباشد");
                }
                else if (await _user.AnyAsync(x => x.Email == registerModel.PhoneOrEmail))
                {
                    if (isValidEmail)
                        result.AddError("PhoneOrEmail", "ایمیل با این نام موجود هست");
                    else if (isValidPhone)
                        result.AddError("PhoneOrEmail", "ایمیل با این نام موجود هست");
                }
                else
                {
                    try
                    {
                        WebUser user = _user.CreateUser();
                        user.Email = isValidEmail ? registerModel.PhoneOrEmail : null;
                        user.PhoneNumber = isValidPhone ? registerModel.PhoneOrEmail : null;
                        var createResult = await _userManager.CreateAsync(user, registerModel.Password);
                        if (createResult.Succeeded)
                        {
                            var userId = await _userManager.GetUserIdAsync(user);
                            await LoginToken(pvTokenRaw, Guid.Parse(userId), result);
                        }
                        else
                        {
                            foreach (var error in createResult.Errors)
                            {
                                if (error.Code == "DuplicateUserName")
                                {
                                    result.AddError("PhoneOrEmail", SharedRegix.DuplicateUserNameError(user.UserName));
                                }
                                else
                                {
                                    throw new NotImplementedException(error.Code + " " + error.Description);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        //   var result = await _userManager.CreateAsync(user, Input.Password);

                        //   return (false, errors);
                    }
                }
            }
            else
            {
                foreach (var state in modelState)
                {
                    result.AddError(state.Key, state.Value.Errors[0].ErrorMessage);
                }
            }
            return result.GetJson();
        }

        public async Task<ContentResult> Login(LoginModel loginModel, ModelStateDictionary modelState)
        {
            var result = new JsonResponse<LoginOkResult>();
            var pvTokenRaw = _pVInfoStructure.Get(loginModel.Token).TResult001;
            if (!modelState.IsValid)
            {
                foreach (var state in modelState)
                {
                    result.AddError(state.Key, state.Value.Errors[0].ErrorMessage);
                }
                return result.GetJson();
            }
            WebUser user = await _user.GetUserByMatch(loginModel.PhoneOrEmail);
            if (user == null)
            {
                result.AddError("PhoneOrEmail", "فیلد مشابه شماره همراه و یا ایمل نمیباشد");
            }
            if (user == null && result.Status == JsonResponseStatus.Success)
            {
                result.AddError("PhoneOrEmail", "نام کاربری و یا گذرواژه فاقد اعتبار");
            }
            if (result.Status == JsonResponseStatus.Success)
            {
                var loginResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, lockoutOnFailure: false);
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
                    await LoginToken(pvTokenRaw, user.Id, result);
                }
                else
                {
                    result.AddError("PhoneOrEmail", "نام کاربری و یا گذرواژه فاقد اعتبار");
                }
            }
            return result.GetJson();
        }

        public async Task LoginToken(PVInfoModel pvTokenRaw, Guid userId, JsonResponse<LoginOkResult> result)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim("UserId",userId.ToString()),
                            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                        }),
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_applicationSettings.JWT)), SecurityAlgorithms.HmacSha256)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            await AddTokenToUser(pvTokenRaw, token);
            result.TResult001 = new LoginOkResult
            {
                Token = _pVInfoStructure.Set(pvTokenRaw).TResult001
            };
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
