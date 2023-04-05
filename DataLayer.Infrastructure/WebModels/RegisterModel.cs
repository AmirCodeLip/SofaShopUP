using DataLayer.Infrastructure.Services;
using DataLayer.UnitOfWork;
using DataLayer.UnitOfWork.Lanuages;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Infrastructure.WebModels
{
    [TSModelUsage(CompileOption = CompileOption.ignore)]
    public class RegisterModel
    {
        [Required(ErrorMessage = "RequiredError", ErrorMessageResourceType = typeof(DisplayAndAnnotations))]
        [Display(Name = "PhoneOrEmail", ResourceType = typeof(DisplayAndAnnotations))]
        public string PhoneOrEmail { get; set; }
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        [StringLength(100, ErrorMessage = SharedRegix.SLError, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(DisplayAndAnnotations))]
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
