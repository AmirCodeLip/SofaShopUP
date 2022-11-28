using DataLayer.Access.Data;
using DataLayer.Access.Services.Base;
using DataLayer.Access.Services.Web;
using DataLayer.Domin.Models.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services.Web
{
    public class FileVersionActorOrArtistService : BaseService<WebFileVersionActorOrArtist>, IFileVersionActorOrArtistRepository
    {
        public FileVersionActorOrArtistService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
