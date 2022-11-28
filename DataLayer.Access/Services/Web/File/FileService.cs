using DataLayer.Access.Data;
using DataLayer.Access.Services.Base;
using DataLayer.Domin.Models.Web;

namespace DataLayer.Access.Services.Web
{
    public class FileService : BaseService<WebFile>, IFileRepository
    {
        public FileService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
