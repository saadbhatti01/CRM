using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("TeacherClass")]
    public class TeacherClass
    {
        [Key]
        public int TeachClassId { get;set; }
        public int perId { get;set; }
        public int camId { get;set; }
        public int secId { get;set; }
        public int sesId { get;set; }
        public int classId { get;set; }
        public int subId { get;set; }

        public virtual Person person { get; set; }
        public virtual Campus campus { get; set; }
        public virtual InstSession ses { get; set; }
        public virtual Class cls { get; set; }
        public virtual InstSection sec { get; set; }
        public virtual Subject subject { get; set; }
    }
}