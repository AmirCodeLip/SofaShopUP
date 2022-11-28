using DataLayer.Access.Services.Base;
using DataLayer.Domin.Models.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services.Web
{
    public interface IActorOrArtistRepository : IBaseRepository<WebActorOrArtist>
    {
        Task<WebActorOrArtist> GetByName(string name, string culture, Guid? parentId = null);
    }
}
