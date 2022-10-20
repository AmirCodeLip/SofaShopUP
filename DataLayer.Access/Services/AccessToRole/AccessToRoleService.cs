using DataLayer.Access.Data;
using DataLayer.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public class AccessToRoleService : BaseService<WebAccessToRole>, IAccessToRoleRepository
    {
        public AccessToRoleService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
