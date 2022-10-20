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
    public class WebFolderService : BaseService<WebFolder>, IWebFolderRepository
    {
        public WebFolderService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
