using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("LoginLogs")]
    public class LoginLogs
    {
        [Key]
        public int logId { get; set; }
        public int id { get; set; }
        public DateTime logDateTime { get; set; }
        public virtual Login login { get; set; }
    }
}