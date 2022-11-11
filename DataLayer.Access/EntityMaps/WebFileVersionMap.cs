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
    public class WebFileVersionMap : IEntityTypeConfiguration<WebFileVersion>
    {
        public void Configure(EntityTypeBuilder<WebFileVersion> builder)
        {
            builder.HasOne(x => x.Parent).WithMany(x => x.AllInfoData).HasForeignKey(k => k.ParentId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
