using DataLayer.Infrastructure.Services;
using DataLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DataLayer.UnitOfWork.Lanuages;

namespace DataLayer.Infrastructure.WebModels
{
    [TSModelUsage(CompileOption = CompileOption.compile)]
    public class LoginModel
    {
        [Required(ErrorMessage = "RequiredError", ErrorMessageResourceType = typeof(DisplayAndAnnotations))]
        [Display(Name = "PhoneOrEmail", ResourceType = typeof(DisplayAndAnnotations))]
        public string PhoneOrEmail { get; set; }
        [Required(ErrorMessage = "RequiredError", ErrorMessageResourceType = typeof(DisplayAndAnnotations))]
        //[Required(ErrorMessage = SharedRegix.RequiredError)]
        [StringLength(100, ErrorMessage = "SLError", ErrorMessageResourceType = typeof(DisplayAndAnnotations), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(DisplayAndAnnotations))]
        public string Password { get; set; }
        public string Token { get; set; }
    }

}
