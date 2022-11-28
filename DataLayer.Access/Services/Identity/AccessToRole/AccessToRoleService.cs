using DataLayer.Access.Data;
using DataLayer.Access.Services.Base;
using DataLayer.Domin.Models.Web;

namespace DataLayer.Access.Services.Identity
{
    public class AccessToRoleService : BaseService<WebAccessToRole>, IAccessToRoleRepository
    {
        public AccessToRoleService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
