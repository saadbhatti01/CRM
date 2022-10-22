using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("SMSModuleName")]
    public class SMSModuleName
    {
        [Key]
        public int mnId { get; set; }
        public string mName { get; set; }
        public bool mnStatus { get; set; }
    }
}