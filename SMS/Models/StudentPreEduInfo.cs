using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("StudentPreEduInfo")]
    public class StudentPreEduInfo
    {
        [Key]
        public int StdEduInfoId { get; set; }
        public int stdId { get; set; }
        public int DegreeId { get; set; }
        public int StdInsId { get; set; }
        public int TotalMarks { get; set; }
        public int ObtainMarks { get; set; }
        public string RollNo { get; set; }
        public DateTime PassingDate { get; set; }
        public virtual Student std { get; set; }
        public virtual Degree degree { get; set; }
        public virtual StudentInsInfo stdIns { get; set; }
        
    }
}