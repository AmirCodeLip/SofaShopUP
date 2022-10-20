using DataLayer.Access.Data;
using DataLayer.Domin.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
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
