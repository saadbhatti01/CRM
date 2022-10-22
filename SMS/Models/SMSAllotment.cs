using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("SMSAllotment")]
    public class SMSAllotment
    {
        [Key]
        public int saId { get; set; }
        public string saQty { get; set; }
        public string saRemainingQty { get; set; }
        public string saApprovalDate { get; set; }
        public string saExpiryDate { get; set; }
        public string saStatus { get; set; }
        public string saAmount { get; set; }
        public string saPaidAmountDate { get; set; }
    }
}