using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.ViewModels.Form
{
    public class InputStringLength : FormDescriptor, IInputStringLength
    {
        public InputStringLength(string errorMessage, int minimumLength, int maximumLength)
        {
            ErrorMessage = errorMessage;
            MinimumLength = minimumLength;
            MaximumLength = maximumLength;
        }

        public string ErrorMessage { get; set; }
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }
    }
}
