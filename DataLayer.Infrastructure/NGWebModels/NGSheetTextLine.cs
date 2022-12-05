#if NEWSGENERATOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Infrastructure.NGWebModels
{
    public class NGSheetTextLine
    {
        public int Index { get; set; }
        public string TextLine { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string JsonStyle { get; set; }
    }
}
#endif