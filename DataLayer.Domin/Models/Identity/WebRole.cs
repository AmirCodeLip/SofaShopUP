using DataLayer.Domin.Models.BaseModels.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace DataLayer.Domin.Models.Identity
{
    public class WebRole : IdentityRole<Guid>, IDeleteBase
    {
        public bool IsDeleted { get; set; }
        //public string PersianName { get; set; }
        //public string Color { get; set; }
        //public int RoleIndex { get; set; }
    }
}
