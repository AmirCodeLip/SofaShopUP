﻿using DataLayer.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.EntityMaps
{
    public class WebFolderMap : IEntityTypeConfiguration<WebFolder>
    {
        public void Configure(EntityTypeBuilder<WebFolder> builder)
        {
            builder.HasOne(f => f.Folder).WithMany(f => f.Folders).
                HasForeignKey(k => k.ParentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}