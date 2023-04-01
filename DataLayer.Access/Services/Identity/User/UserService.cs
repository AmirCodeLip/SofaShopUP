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
            WebUser webUser = Activator.CreateInstance<WebUser>();
            webUser.UserName = GetHexString(25);
            return webUser;
        }

        public string GetNewId(string userName) =>
           userName + "#" + _rnd.Next(1000, 9999);

        static string[] StrHex = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "A", "B", "C", "D", "E", "F" };

        public string GetHexString(int count)
        {
            string hex = "";
            for (int i = 0; i < count; i++)
                hex += StrHex[_rnd.Next(0, 16)];
            return hex;
        }

        public async Task<WebUser> GetUserByMatch(string phoneOrEmail)
        {
            if (SharedRegix.RgEmail.IsMatch(phoneOrEmail))
            {
                return await FirstOrDefaultAsync(x => x.Email == phoneOrEmail);
            }
            else if (SharedRegix.RgPhone.IsMatch(phoneOrEmail))
            {
                return await FirstOrDefaultAsync(x => x.PhoneNumber == phoneOrEmail);
            }
            else
                return null;
        }


       

    }
}

