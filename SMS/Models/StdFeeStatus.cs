using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public class StdFeeStatus
    {
        public int feeTypeId { get; set; }
        public double PaidAmt { get; set; }
        public string FeePaidName { get; set; }
        public string FeePaidStatus { get; set; }
        public double Panding { get; set; }
    }
}