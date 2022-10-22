using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("Documents")]
    public class Documents
    {
        [Key]
        public int DocId { get; set; }
        public int camId { get; set; }
        public int secId { get; set; }
        public int sesId { get; set; }
        public int classId { get; set; }
        public int subId { get; set; }
        public string DocTitle { get; set; }
        public string Note { get; set; }
        public string PersonalNote { get; set; }
        public int CreatedBy { get; set; }
        public string FilePath { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isVisible { get; set; }

      
        public virtual Campus campus { get; set; }
        public virtual InstSession ses { get; set; }
        public virtual Class cls { get; set; }
        public virtual InstSection sec { get; set; }
        public virtual Subject subject { get; set; }
    }
}