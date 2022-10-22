using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public class StdViewModel
    {
        public string Month { get; set; }
        public string FeeMonth { get; set; }
        public string Remarks { get; set; }
        public string Year { get; set; }
        public DateTime DueDate { get; set; }
        public int BankId { get; set; }
        public IEnumerable<StdFeeDetail> StdDetail { get; set; }
        public IEnumerable<StdFeeStatus> StdStatus { get; set; }
    }
}