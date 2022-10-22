using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models
{
    [Table("BankInfo")]
    public class BankInfo
    {
        [Key]
        public int BankInfoId { get; set; }
        public int BankId { get; set; }
        public string AcTitle { get; set; }
        public string AcNumber { get; set; }
        public string BranchAddress { get; set; }
        public string BranchContact { get; set; }
        public bool IsVisible { get; set; }

        public virtual Bank bank { get; set; }
    }
}