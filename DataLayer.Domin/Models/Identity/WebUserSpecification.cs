using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using DataLayer.UnitOfWork;
using DataLayer.Domin.Models.Interfaces;

namespace DataLayer.Domin.Models.Identity
{
    public class WebUserSpecification : IDeleteBase
    {
        public WebUserSpecification()
        {

        }
        [StringLength(50, ErrorMessage = SharedRegix.SLError, MinimumLength = 3)]
        public string Name { get; set; }
        [StringLength(50, ErrorMessage = SharedRegix.SLError, MinimumLength = 3)]
        public string LastName { get; set; }
        public Guid UserId { get; set; }
        public WebUser User { get; set; }

        public ICollection<ShopAddress> Addresses { get; set; }
        public bool IsDeleted { get; set; }
    }

}
