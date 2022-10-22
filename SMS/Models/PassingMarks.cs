using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("PassingMarks")]
    public class PassingMarks
    {
        [Key]
        public int PassingId { get; set; }
        public int classId { get; set; }
        public int Marks { get; set; }
        public virtual Class cl { get; set; }

    }
}