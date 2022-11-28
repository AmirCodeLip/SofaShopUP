using DataLayer.Access.Services.Base;
using DataLayer.Domin.Models.Identity;


namespace DataLayer.Access.Services.Identity
{
    public interface IWebUserSpecificationRepository : IBaseRepository<WebUserSpecification>
    {
        Task<WebUserSpecification> GetUserProfileById(Guid userId);
    }
}
