using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("ExamType")]
    public class ExamType
    {
        [Key]
        public int etId { get; set; }
        public string etname { get; set; }
        public bool etStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
    }

    [Table("SubMarkType")]
    public class SubMarkType
    {
        [Key]
        public int smtId { get; set; }
        public string smtName { get; set; }
        public bool smtStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
    }

    [Table("StdObtainMarks")]
    public class StdObtainMarks
    {
        [Key]
        public int somID { get; set; }
        public int sesId { get; set; }
        public int secId { get; set; }
        public int classId { get; set; }
        public int stdId { get; set; }
        public int etId { get; set; }
        public int subId { get; set; }
        public double smt1 { get; set; }
        public double smt2 { get; set; }
        public double smt3 { get; set; }
        public double totalObtainMarks { get; set; }
        public double subTotalMarks { get; set; }
        public bool somStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public virtual Student std { get; set; }
        public virtual Class cls { get; set; }
        public virtual InstSection sec { get; set; }
        public virtual InstSession ses { get; set; }
        public virtual ExamType et { get; set; }
        public virtual Subject sb { get; set; }
    }

    public class StdObtDetail
    {
        [Key]
        public int stdId { get; set; }
        public int sesId { get; set; }
        public int secId { get; set; }
        public int classId { get; set; }
        public int etId { get; set; }
        public int subId { get; set; }
        public string stdName { get; set; }
        public string stdRollNo { get; set; }
        public double smt1 { get; set; }
        public double smt2 { get; set; }
        public double smt3 { get; set; }
        public string sm1 { get; set; }
        public string sm2 { get; set; }
        public string sm3 { get; set; }
        public double totalObtainMarks { get; set; }
        public double subTotalMarks { get; set; }
        public double OutOf { get; set; }
        public List<StdObtDetail> StdSubList { get; set; }
        
    }
}