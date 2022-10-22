using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("SMSModule")]
    public class SMSModule
    {
        [Key]
        public int smId { get; set; }
        public int mnId { get; set; }
        public string smText { get; set; }
        public bool smStatus { get; set; }
        public DateTime smStatusChangeDate { get; set; }
        public bool isLocked { get; set; }
        public virtual SMSModuleName moduleName { get; set; }
    }
}