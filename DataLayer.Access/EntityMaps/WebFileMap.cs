using DataLayer.Domin.Models.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Access.EntityMaps
{
    public class WebFileMap : IEntityTypeConfiguration<WebFile>
    {
        public void Configure(EntityTypeBuilder<WebFile> builder)
        {
            builder.Property(x => x.CreatedDate).HasDefaultValueSql("getdate()");
            builder.HasOne(x => x.WebUser).WithMany().HasForeignKey(x => x.CreatorId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
