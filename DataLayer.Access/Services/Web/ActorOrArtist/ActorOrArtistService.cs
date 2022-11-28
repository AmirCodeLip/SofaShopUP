using DataLayer.Access.Data;
using DataLayer.Access.Services.Base;
using DataLayer.Domin.Models.Web;

namespace DataLayer.Access.Services.Web
{
    public class ActorOrArtistService : BaseService<WebActorOrArtist>, IActorOrArtistRepository
    {
        public ActorOrArtistService(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<WebActorOrArtist> GetByName(string name, string culture, Guid? parentId = null)
        {
            var webActorOrArtist = await SingleOrDefaultAsync(x => x.Name == name && x.Culture == culture);
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
                await AddAsync(webActorOrArtist);
                await SaveChangesAsync();
            }
            return webActorOrArtist;
        }
    }
}
