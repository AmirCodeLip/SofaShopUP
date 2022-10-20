using DataLayer.Domin.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Works
{
    public class UserSectionAccess
    {
        public bool AddRoleToUser { get; set; } = true;
        public bool ViewProducts { get; set; } = true;
        public bool ViewProductCategories { get; set; } = true;
        public bool ViewUsers { get; set; } = true;
    }

 

}
