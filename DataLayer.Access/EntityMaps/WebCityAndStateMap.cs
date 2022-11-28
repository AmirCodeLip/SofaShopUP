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
    public class WebCityAndStateMap : IEntityTypeConfiguration<WebCityAndState>
    {
        public void Configure(EntityTypeBuilder<WebCityAndState> builder)
        {
            builder.HasOne(x => x.CityAndStateItem).WithMany(x => x.CityAndStates).
                HasForeignKey(k => k.CityAndStateId).IsRequired(false);
        }
    }
}
