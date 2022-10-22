using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("SentSMS")]
    public class SentSMS
    {
        [Key]
        public int ssId { get; set; }
        public DateTime ssDate { get; set; }
        public bool ssStatus { get; set; }
        public int perId { get; set; }
        public string ssText { get; set; }
        public virtual Person persons { get; set; }
    }
}