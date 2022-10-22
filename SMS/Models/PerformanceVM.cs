using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public class PerformanceVM
    {
        public int etId { get; set; }
        public int et2 { get; set; }
        public int sub1 { get; set; }
        
        public int sub2 { get; set; }
        public string subName1 { get; set; }
        public string subName2 { get; set; }
        public string exam { get; set; }
        public string exam2 { get; set; }
        public double Obt1 { get; set; }
        public double Obt2 { get; set; }
        public double Total1 { get; set; }
        public double Total2 { get; set; }
        public double per1 { get; set; }
        public double per2 { get; set; }
        public double performance { get; set; }
        public double Finalperformance { get; set; }
        public string ExamsName { get; set; }
    }
}