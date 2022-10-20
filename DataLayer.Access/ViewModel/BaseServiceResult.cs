using DataLayer.Domin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.ViewModel
{
    public class BaseServiceResult<TInput>
    {
        public ActionStatus Status { get; set; }
        public TInput Result { get; set; }
    }
}
