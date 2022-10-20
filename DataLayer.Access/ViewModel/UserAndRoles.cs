using DataLayer.Domin.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.ViewModel
{
    public class UserAndRoles
    {
        public UserAndRoles(WebUser user)
        {
            UserName = user.UserName;
            Name = user.UserSpecification.Name ?? "وارد نشده";

        }

        public string UserName { get; set; }
        public string Name { get; set; }
        public IList<string> Roles { get; set; }

    }
}
