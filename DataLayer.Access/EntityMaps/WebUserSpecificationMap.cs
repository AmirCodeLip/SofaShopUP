using DataLayer.Domin.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.EntityMaps
{
    public class WebUserSpecificationMap : IEntityTypeConfiguration<WebUserSpecification>
    {
        public void Configure(EntityTypeBuilder<WebUserSpecification> builder)
        {

            builder.HasKey(k => k.UserId);
            builder.HasOne(x => x.User).WithOne(x => x.UserSpecification).HasForeignKey<WebUserSpecification>(u => u.UserId);
        }
    }
}
