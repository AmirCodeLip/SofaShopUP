using DataLayer.Domin.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models
{
    public class WebActorOrArtist : BaseUniversal<Guid>
    {
        public Guid? ParentId { get; set; }
        public WebActorOrArtist Parent { get; set; }
        /// <summary>
        /// use for store another language names
        /// </summary>
        public ICollection<WebActorOrArtist> Children { get; set; }
        public ICollection<WebFileVersionActorOrArtist> WebFileVersionActorOrArtists { get; set; }
    }
}
