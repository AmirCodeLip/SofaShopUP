using DataLayer.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public interface IActorOrArtistRepository : IBaseRepository<WebActorOrArtist>
    {
        Task<WebActorOrArtist> GetByName(string name, string culture, Guid? parentId = null);
    }
}
