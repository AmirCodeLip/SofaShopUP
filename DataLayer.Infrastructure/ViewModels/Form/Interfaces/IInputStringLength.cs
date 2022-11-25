using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.ViewModels.Form
{
    public interface IInputStringLength
    {
        string ErrorMessage { get; set; }
        int MinimumLength { get; set; }
        int MaximumLength { get; set; }
    }
}
