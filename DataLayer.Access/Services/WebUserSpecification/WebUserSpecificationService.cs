using DataLayer.Access.Data;
using DataLayer.Domin.Models;
using DataLayer.Domin.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public class WebUserSpecificationService : BaseService<WebUserSpecification>, IWebUserSpecificationRepository
    {
        public WebUserSpecificationService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<WebUserSpecification> GetUserProfileById(Guid userId)
        {
            var us = FirstOrDefault(x => x.UserId == userId);
            if (us == null)
            {
                us = new WebUserSpecification();
                us.UserId = userId;
                Add(us);
                //await SaveChangesAsync();
            }
            return us;
        }
    }
}
