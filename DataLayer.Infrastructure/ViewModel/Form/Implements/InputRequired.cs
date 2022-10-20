using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.ViewModel.Form
{
    public class InputRequired : FormDescriptor, IInputRequired
    {
        public InputRequired(bool required, string errorMessage)
        {
            this.Required = required;
            ErrorMessage = errorMessage;
        }
        public bool Required { get; set; }
        public string ErrorMessage { get; set; }
    }
}
