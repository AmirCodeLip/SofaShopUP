using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using DataLayer.UnitOfWork;
using DataLayer.Domin.Models.BaseModels.Interfaces;
#if shop_project
using DataLayer.Domin.Models.WebShop;
#endif

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
#if shop_project
        public ICollection<ShopAddress> Addresses { get; set; }
#endif
        public bool IsDeleted { get; set; }
    }

}
