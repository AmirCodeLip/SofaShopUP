using DataLayer.Infrastructure.Services;
using DataLayer.UnitOfWork;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Infrastructure.WebModels
{
    [TSModelUsage(CompileOption = CompileOption.ignore)]
    public class RegisterModel
    {
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        [Display(Name = "شماره همراه یا ایمیل")]
        public string PhoneOrEmail { get; set; }
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        [StringLength(100, ErrorMessage = SharedRegix.SLError, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "گذرواژه")]
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
