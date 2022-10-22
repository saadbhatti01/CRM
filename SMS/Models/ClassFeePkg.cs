using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("ClassFeePkg")]
    public class ClassFeePkg
    {
        [Key]
        public int cfpId { get; set; }
        public int sesId { get; set; }
        public int secId { get; set; }
        public int classId { get; set; }
        public int feeTypeId { get; set; }
        public double cfpAmt { get; set; }
        public int cfpDis { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }

        public virtual Class cls { get; set; }

        public virtual InstSection sec { get; set; }
        public virtual InstSession ses { get; set; }
        public virtual FeeType ft { get; set; }
    }
}