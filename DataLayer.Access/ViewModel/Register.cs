using DataLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.ViewModel
{ 
    public class Register
    {

        [Display(Name = "نام کاربری")]
        [StringLength(30, ErrorMessage = SharedRegix.SLError, MinimumLength = 2)]
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        public string UserName { get; set; }
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        [StringLength(100, ErrorMessage = SharedRegix.SLError, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "گذرواژه")]
        public string Password { get; set; }
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        [Display(Name = "شماره همراه یا ایمیل")]
        public string PhoneOrEmail { get; set; }


    }
}
