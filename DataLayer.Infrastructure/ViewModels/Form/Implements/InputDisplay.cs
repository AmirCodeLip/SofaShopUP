using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.ViewModels.Form
{
    public class InputDisplay : FormDescriptor, IInputDisplay
    {
        public InputDisplay(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

    }
}
