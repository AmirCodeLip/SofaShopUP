using DataLayer.Access.Data;
using DataLayer.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public class ActorOrArtistService : BaseService<WebActorOrArtist>, IActorOrArtistRepository
    {
        public ActorOrArtistService(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<WebActorOrArtist> GetByName(string name, string culture, Guid? parentId = null)
        {
            var webActorOrArtist = await this.SingleOrDefaultAsync(x => x.Name == name && x.Culture == culture);
            if (webActorOrArtist == null)
            {
                webActorOrArtist = new WebActorOrArtist
                {
                    Name = name,
                    Culture = culture,
                };
                if (parentId.HasValue)
                {
                    webActorOrArtist.ParentId = parentId.Value;
                }
                await this.AddAsync(webActorOrArtist);
                await this.SaveChangesAsync();
            }
            return webActorOrArtist;
        }
    }
}
