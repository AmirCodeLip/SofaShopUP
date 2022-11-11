using DataLayer.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.EntityMaps
{
    public class WebFileVersionActorOrArtistMap : IEntityTypeConfiguration<WebFileVersionActorOrArtist>
    {
        public void Configure(EntityTypeBuilder<WebFileVersionActorOrArtist> builder)
        {
            builder.HasOne(x => x.WebFileVersion).WithMany(x => x.WebFileVersionActorOrArtists).HasForeignKey(k => k.WebFileVersionId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.WebActorOrArtist).WithMany(x => x.WebFileVersionActorOrArtists).HasForeignKey(k => k.WebActorOrArtistId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasKey(nameof(WebFileVersionActorOrArtist.WebFileVersionId), nameof(WebFileVersionActorOrArtist.WebActorOrArtistId));
        }
    }
}
