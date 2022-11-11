using DataLayer.Domin.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin.Models
{
    public class WebFileVersionActorOrArtist : IDeleteBase
    {
        public Guid WebActorOrArtistId { get; set; }
        public WebActorOrArtist WebActorOrArtist { get; set; }
        public Guid WebFileVersionId { get; set; }
        public WebFileVersion WebFileVersion { get; set; }
        public bool IsDeleted { get; set; }
    }
}
