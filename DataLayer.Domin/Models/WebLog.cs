using System;

namespace DataLayer.Domin.Models
{
    public class WebLog
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string LogText { get; set; }
    }
}
