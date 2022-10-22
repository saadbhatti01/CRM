using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("AttenParameter")]
    public class AttenParameter
    {
        [Key]
        public int apId { get; set; }
        public int roleId { get; set; }
        public string InMinTime { get; set; }
        public string InMaxTime { get; set; }
        public string OutMinTime { get; set; }
        public string OutMaxTime { get; set; }
        public virtual Role role { get; set; }
    }
}