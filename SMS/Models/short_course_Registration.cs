using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("short_course_Registration")]
    public class short_course_Registration
    {
        [Key]
        public int scrId { get; set; }
        public int stdId { get; set; }
        public int classId { get; set; }
        public int scsId { get; set; }
        public int scReceivedFee { get; set; }
        public DateTime scfPaidDate { get; set; }
        public int scPendingFee { get; set; }
        public DateTime scPendingDate { get; set; }
        public int scDiscount { get; set; }
        public string scStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public virtual Class cls { get; set; }
        public virtual Student std { get; set; }
        public virtual short_course_Subject scSub { get; set; }

    }
}