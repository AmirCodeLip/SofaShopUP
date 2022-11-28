using DataLayer.Access.Data;
using DataLayer.Access.Services.Base;
using DataLayer.Domin.Models.Web;

namespace DataLayer.Access.Services.Web
{
    public class FolderService : BaseService<WebFolder>, IFolderRepository
    {
        public FolderService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
