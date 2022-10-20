using DataLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.ViewModel
{
    public class PersonalInfo
    {
        private string userName;
        public string UserName
        {
            get => this.userName; 
            set
            {
                this.userName = value;
                var jn = value.Length - 5;
                var jt = value.Length - jn;
                UserJustName = value.Substring(0, jn);
                UserJustTag = value.Substring(jn, jt);
            }
        }
        
        public string UserJustName { get; private set; }
        public string UserJustTag { get; private set; }

        [StringLength(50, ErrorMessage = SharedRegix.SLError, MinimumLength = 3)]
        [Display(Name = "نام")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = SharedRegix.SLError, MinimumLength = 3)]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "شماره همراه")]
        public string PhoneNumber { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        public string Mail { get; set; }
    }

}
