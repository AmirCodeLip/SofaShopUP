#if shop_project
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Access.EntityMaps
{
    public class ShopCategorySpecificationRelationMap : IEntityTypeConfiguration<ShopCategorySpecificationRelation>
    {
        public void Configure(EntityTypeBuilder<ShopCategorySpecificationRelation> builder)
        {
            builder.HasOne(x => x.ProductCategory).WithMany(x => x.CategorySpecificationRelations).HasForeignKey(k => k.ProductCategoryId);
            builder.HasOne(x => x.CategorySpecification).WithOne(x => x.CategorySpecificationRelation).
                HasForeignKey<ShopCategorySpecificationRelation>(k => k.ProductCategorySpecificationId);
            builder.HasKey(x => new { x.ProductCategoryId, x.ProductCategorySpecificationId });
        }
    }
}
#endif