using DataLayer.Domin.Models.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.EntityMaps
{
    public class WebActorOrArtistMap : IEntityTypeConfiguration<WebActorOrArtist>
    {
        public void Configure(EntityTypeBuilder<WebActorOrArtist> builder)
        {
            builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(k => k.ParentId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(nameof(WebActorOrArtist.Name), nameof(WebActorOrArtist.Culture)).IsUnique();
        }
    }
}
