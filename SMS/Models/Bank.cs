using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models
{
    [Table("Bank")]
    public class Bank
    {
        [Key]
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankLogo { get; set; }
        public bool IsVisible { get; set; }
    }
}