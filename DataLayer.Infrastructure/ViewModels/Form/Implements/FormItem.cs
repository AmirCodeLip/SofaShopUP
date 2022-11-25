using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.ViewModels.Form
{
    public class FormItem
    {
        public string Name { get; set; }
        public InputType InputType { get; set; }
        public List<FormDescriptor> FormDescriptors { get; set; }
        = new List<FormDescriptor>();

    }
}
