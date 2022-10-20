using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.ViewModel.Form
{
    public class FormDescriptor : IFormDescriptor
    {
        public FormDescriptor()
        {
            ClassName = this.GetType().Name;
        }
        public string ClassName { get; set; }
    }
}
