using DataLayer.Infrastructure.Services;
using DataLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.WebModels
{
    [TSModelUsage(CompileOption = CompileOption.compile)]
    public class LoginModel
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
