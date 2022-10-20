using DataLayer.Access.Data;
using DataLayer.Domin.Models;
using DataLayer.Domin.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public class FileVersionService : BaseService<WebFileVersion>, IFileVersionRepository
    {
        public FileVersionService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
