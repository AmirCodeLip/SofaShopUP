using DataLayer.Access.Data;
using DataLayer.Access.Services.Base;
using DataLayer.Domin.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Access.Services.Identity
{
    public class RoleService : BaseService<WebRole>, IRoleRepository
    {
        private readonly UserManager<WebUser> _userManager;
        private readonly RoleManager<WebRole> _roleManager;
        private readonly IUserRepository _userRepository;
        public RoleService(ApplicationDbContext context,
            RoleManager<WebRole> roleManager,
            UserManager<WebUser> userManager,
            IUserRepository userRepository) : base(context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userRepository = userRepository;
        }


    }
}
