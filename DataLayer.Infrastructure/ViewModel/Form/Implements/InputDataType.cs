using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.ViewModel.Form
{
    public class InputDataType : FormDescriptor, IInputDataType
    {
        public InputDataType(DataType dataType)
        {
            DataType = (int)dataType;
        }
        public int DataType { get; set; }
    }

}
