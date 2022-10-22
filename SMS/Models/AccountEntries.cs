using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("AccountEntries")]
    public class AccountEntries
    {
        [Key]
        public int aeId { get; set; }
        public int ahId { get; set; }
        public double amount { get; set; }
        public DateTime postedDate { get; set; }
        public string description { get; set; }
        public bool isLocked { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }

        public virtual AccountHeader ah { get; set; }
    }
}