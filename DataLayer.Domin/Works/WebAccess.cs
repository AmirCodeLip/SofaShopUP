using DataLayer.Domin.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Works
{
    public class WebAccess
    {
        public UserSectionAccess userSection;
        public WebAccess(UserManager<WebUser> userManager)
        {
            Type UserSectionAccessType = typeof(UserSectionAccess);
            userSection = new UserSectionAccess();
        }





        //public bool HaveAccess(AllRoles role)
        //{
        //    return _userSectionAccesses.Any(u => u.Role == role && u.Access);
        //}
    }

}
