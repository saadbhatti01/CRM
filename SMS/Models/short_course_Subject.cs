using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("short_course_Subject")]
    public class short_course_Subject
    {
        [Key]
        public int scsId { get; set; }
        public int classId { get; set; }
        public string scsName { get; set; }
        public int scsFee { get; set; }
        public string scsDuration { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }

        public virtual Class cls { get; set; }
    }
}