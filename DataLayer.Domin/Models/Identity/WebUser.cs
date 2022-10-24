using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections;
using System.Collections.Generic;
using DataLayer.Domin.Models.Interfaces;

namespace DataLayer.Domin.Models.Identity
{
    public class WebUser : IdentityUser<Guid>, IDeleteBase
    {
        public WebUser()
        {

        }
        public WebUserSpecification UserSpecification { get; set; }

        [Display(Name = "تلفن همراه")]
        public override string PhoneNumber { get => base.PhoneNumber; set { base.PhoneNumber = value; } }
        public bool IsDeleted { get; set; }
        public ICollection<WebFolder> Folders { get; set; }
        public virtual ICollection<ShopAddress> Addresses { get; set; }
    }
}
