using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("StudentFeePkg")]
    public class StdFeePkg
    {
        [Key]
        public int sfpId { get; set; }
        public int sesId { get; set; }
        public int secId { get; set; }
        public int classId { get; set; }
        public int sfpstdId { get; set; }
        public int feeTypeId { get; set; }
        public double sfpAmt { get; set; }
        public int sfpDis { get; set; }
        public string sfpRemarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public virtual FeeType ft { get; set; }
        public virtual InstSection sec { get; set; }
        public virtual InstSession ses { get; set; }
        public virtual Class cls { get; set; }
        //public virtual Student std { get; set; }


    }
}