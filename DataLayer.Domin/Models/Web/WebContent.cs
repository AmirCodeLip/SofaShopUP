using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataLayer.UnitOfWork;

namespace DataLayer.Domin.Models.Web
{
    public class WebContent
    {
        public WebContent()
        {
            CreatedDate = DateTime.Now;
            IsDeleted = false;
        }
        public int Id { get; set; }
        [Required(ErrorMessage = SharedRegix.RequiredError)]
        [MinLength(4, ErrorMessage = SharedRegix.MinLengthError)]
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        [DisplayName(displayName: "توضیحات")]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }


    }
}