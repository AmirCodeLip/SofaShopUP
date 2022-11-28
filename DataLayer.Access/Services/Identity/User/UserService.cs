using DataLayer.Access.Data;
using DataLayer.Access.Services.Base;
using DataLayer.Access.ViewModel;
using DataLayer.Domin.Models.Identity;
using DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DataLayer.Access.Services.Identity
{
    public class UserService : BaseService<WebUser>, IUserRepository
    {
        private readonly IUserStore<WebUser> _userStore;
        private readonly SignInManager<WebUser> _signInManager;
        private readonly UserManager<WebUser> _userManager;
        private Random _rnd = new Random();
        public UserService(ApplicationDbContext context,
            UserManager<WebUser> userManager,
            IUserStore<WebUser> userStore,
            SignInManager<WebUser> signInManager) : base(context)
        {
            _userStore = userStore;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool success, Dictionary<string, string> errors)> RegisterAsync(Register Input, ILogger logger)
        {
            var errors = new Dictionary<string, string>();
            var user = CreateUser();
            if (SharedRegix.RgEmail.IsMatch(Input.PhoneOrEmail))
            {
                if (await AnyAsync(x => x.Email == user.Email))
                {
                    errors["Input.PhoneOrEmail"] = "ایمیل با این نام موجود هست";
                    return (false, errors);
                }
                user.Email = Input.PhoneOrEmail;
            }
            else if (SharedRegix.RgPhone.IsMatch(Input.PhoneOrEmail))
            {
                if (await AnyAsync(x => x.PhoneNumber == user.PhoneNumber))
                {
                    errors["Input.PhoneOrEmail"] = "شماره همراه با این نام موجد هست";
                    return (false, errors);
                }
                user.PhoneNumber = Input.PhoneOrEmail;
            }
            else
            {
                errors["Input.PhoneOrEmail"] = "فیلد مشابه شماره همراه و یا ایمل نمیباشد";
                return (false, errors);
            }
            await _userStore.SetUserNameAsync(user, GetNewId(Input.UserName), CancellationToken.None);
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                logger.LogInformation("User created a new account with password.");
                var userId = await _userManager.GetUserIdAsync(user);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return (true, errors);
            }
            foreach (var error in result.Errors)
            {
                if (error.Code == "DuplicateUserName")
                {
                    errors[""] = SharedRegix.DuplicateUserNameError(user.UserName);
                }
                else
                {
                    errors[error.Code.ToString()] = error.Description;
                }
            }
            return (false, errors);
        }

        public async Task<WebUser> GetUserAsync(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public Guid GetUserId(HttpContext httpContext) => Guid.Parse(_userManager.GetUserId(httpContext.User));

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public bool IsSignedIn(ClaimsPrincipal user)
        {
            return _signInManager.IsSignedIn(user);
        }

        public WebUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<WebUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(WebUser)}'. " +
                    $"Ensure that '{nameof(WebUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        public string GetNewId(string userName) =>
           userName + "#" + _rnd.Next(1000, 9999);


    }
}

