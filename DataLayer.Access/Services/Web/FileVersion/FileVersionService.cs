using DataLayer.Access.Data;
using DataLayer.Access.Services.Base;
using DataLayer.Domin.Models.Web;

namespace DataLayer.Access.Services.Web
{
    public class FileVersionService : BaseService<WebFileVersion>, IFileVersionRepository
    {
        public FileVersionService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
