﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using DataLayer.Domin.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DataLayer.Access.EntityMaps;
using DataLayer.Domin.Models.Web;
#if shop_project
using DataLayer.Domin.Models.WebShop;
#endif

namespace DataLayer.Access.Data
{
    public class ApplicationDbContext : IdentityDbContext<WebUser, WebRole, Guid, IdentityUserClaim<Guid>,
       IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<WebAccessToRole> WebAccessToRoles { get; set; }
        public DbSet<WebUserSpecification> WebUserSpecifications { get; set; }
        public DbSet<WebContent> WebContents { get; set; }
        public DbSet<WebLog> WebLogs { get; set; }
        public DbSet<RelationBetweenUser> RelationBetweenUsers { get; set; }
        public DbSet<WebCityAndState> WebCityAndStates { get; set; }
#region shop dbsets
#if shop_project
        public DbSet<ShopProductCategory> ShopProductCategories { get; set; }
        public DbSet<ShopProduct> ShopProducts { get; set; }
        public DbSet<ShopCartItem> ShopCartItems { get; set; }
        public DbSet<ShopAddress> ShopAddreses { get; set; }
        public DbSet<ShopProductCategorySpecification> ShopProductCategorySpecifications { get; set; }
        public DbSet<ShopCategorySpecificationRelation> ShopCategorySpecificationRelations { get; set; }
#endif
#endregion
#region File And Folders
        public DbSet<WebFile> WebFiles { get; set; }
        public DbSet<WebFolder> WebFolders { get; set; }
        public DbSet<WebFileVersion> WebFileVersions { get; set; }
        public DbSet<WebActorOrArtist> WebActorOrArtists { get; set; }
        public DbSet<WebFileVersionActorOrArtist> WebFileVersionActorOrArtists { get; set; }
#endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
#if shop_project
            builder.Entity<ShopAddress>().HasOne(x => x.UserSpecification).WithMany(x => x.Addresses).HasForeignKey(k => k.UserSpecificationId);
#endif
            builder.ApplyConfigurationsFromAssembly(typeof(AccessResolveDependencies).Assembly);
            base.OnModelCreating(builder);
        }
    }
}