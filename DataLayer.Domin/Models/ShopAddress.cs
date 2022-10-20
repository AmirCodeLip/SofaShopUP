using System;
using System.ComponentModel.DataAnnotations;
using DataLayer.Domin.Models.Identity;
using DataLayer.Domin.Models.Interfaces;
using DataLayer.UnitOfWork;

namespace DataLayer.Domin.Models
{
    public class ShopAddress : IDeleteBase
    {
        public int Id { get; set; }
        [Display(Name = "نام و نام خانوادگی تحویل گیرنده")]
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        public string NameAndLastName { get; set; }

        [Display(Name = "کد پستی")]
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        public string PostalCode { get; set; }
        [Display(Name = "شماره تلفن همراه")]


        [Required(ErrorMessage = SharedRegix.RequiredError)]
        public string PhoneNumber { get; set; }
        [Display(Name = "شماره تلفن منزل")]
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        public string HomePhone { get; set; }



        [Required(ErrorMessage = SharedRegix.RequiredError)]
        public int CityAndStateId { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        public string Email { get; set; }


        [Display(Name = "آدرس پستی")]
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        public string PostAddress { get; set; }

        public Guid UserSpecificationId { get; set; }
        public WebCityAndState CityAndState { get; set; }

        public WebUserSpecification UserSpecification { get; set; }
        public bool IsDeleted { get; set; }
    }
}
