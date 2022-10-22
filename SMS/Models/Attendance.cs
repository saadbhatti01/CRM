using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        public int attenId { get; set; }
        public int perId { get; set; }
        public int? sesId { get; set; }
        public int? secId { get; set; }
        public int? classId { get; set; }
        public string attenType { get; set; }
        public string attenMarkType { get; set; }
        public string attenTime { get; set; }
        public DateTime attenDate { get; set; }
        public DateTime attenDateTime { get; set; }
        public int? camId { get; set; }
        public virtual Person pr { get; set; }
        public virtual Class cls { get; set; }
        public virtual InstSection sec { get; set; }
        public virtual InstSession ses { get; set; }
        public virtual Campus cam { get; set; }
    }

    [Table("Campus")]
    public class Campus
    {
        [Key]
        public int camId { get; set; }
        public string campusname { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string CampCode { get; set; }
    }

    [Table("Class")]
    public class Class
    {
        [Key]
        public int classId { get; set; }
        public string classname { get; set; }
        public string classCode { get; set; }
        public DateTime createDate { get; set; }
        public int createdBy { get; set; }
        public DateTime updatedDate { get; set; }
        public int updatedBy { get; set; }
    }
    [Table("City")]
    public class City
    {
        [Key]
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
    [Table("Area")]
    public class Area
    {
        [Key]
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int CityId { get; set; }

        public virtual City city { get; set; }
    }

    [Table("Subject")]
    public class Subject
    {
        [Key]
        public int subId { get; set; }
        public string subName { get; set; }
        public string subCode { get; set; }
        public bool isVisible { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
    }

    [Table("Term")]
    public class Term
    {
        [Key]
        public int termId { get; set; }
        public string termName { get; set; }
    }


    [Table("FeeType")]
    public class FeeType
    {
        [Key]
        public int feeTypeId { get; set; }
        public string feeTypeName { get; set; }
        public string feeTypeStatus { get; set; }
        public int createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public int updatedBy { get; set; }
        public DateTime updatedDate { get; set; }
        public bool isDeleted { get; set; }
        public int deletedBy { get; set; }
        public DateTime deletedDate { get; set; }     
    }


    [Table("ExtraFeeType")]
    public class ExtraFeeType
    {
        [Key]
        public int eftId { get; set; }
        public string eftName { get; set; }
        public bool eftStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
    }


    [Table("StudentExtraFee")]
    public class StudentExtraFee
    {
        [Key]
        public int sefId { get; set; }
        public int stdId { get; set; }
        public int sesId { get; set; }
        public int secId { get; set; }
        public int classId { get; set; }
        public int eftId { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public DateTime eDate { get; set; }
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
        public virtual ExtraFeeType eft { get; set; }

    }

    [Table("FingerReg")]
    public class FingerReg
    {
        [Key]
        public int FRId { get; set; }
        public int studId { get; set; }
        public string FMD1 { get; set; }
        public string FMD2 { get; set; }
    }

    [Table("InstituteInfo")]
    public class InstituteInfo
    {
        [Key]
        public int intId { get; set; }
        public string intName { get; set; }
        public string intAddress { get; set; }
        public string intPhone { get; set; }
        public byte[] intLogo { get; set; }
    }

    [Table("InstSection")]
    public class InstSection
    {
        [Key]
        public int secId { get; set; }
        public string secName { get; set; }
        public string secCode { get; set; }
        public DateTime createDate { get; set; }
        public int createdBy { get; set; }
        public DateTime updatedDate { get; set; }
        public int updatedBy { get; set; }       
    }

    [Table("InstSession")]
    public class InstSession
    {
        [Key]
        public int sesId { get; set; }
        public string sesName { get; set; }
        public string sesCode { get; set; }
        public string sesStatus { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime sesStartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime sesEndDate { get; set; }
        public DateTime createDate { get; set; }
        public int createdBy { get; set; }
        public DateTime updatedDate { get; set; }
        public int updatedBy { get; set; }
    }

    [Table("Login")]
    public class Login
    {
        [Key]
        public int id { get; set; }
        public string usrName { get; set; }
        public string usrLogin { get; set; }
        public string usrPassword { get; set; }
        public int roleId { get; set; }
        public string usrStatus { get; set; }

        public virtual Role role { get; set; }
    }

    [Table("Role")]
    public class Role
    {
        [Key]
        public int roleId { get; set; }
        public string name { get; set; }
    }

    [Table("RptAttendance")]
    public class RptAttendance
    {
        [Key]
        public int rptAttenId { get; set; }
        public string studentName { get; set; }
        public string fatherName { get; set; }
    }

    [Table("Person")]
    public class Person
    {
        [Key]
        public int perId { get; set; }
        public string perName { get; set; }
        public string perGarName { get; set; }
        public string perDOB { get; set; }
        public string perCurrentAdrs { get; set; }
        public string perPermanantAdrs { get; set; }
        public string perContactOne { get; set; }
        public string perContactTwo { get; set; }
        public string perCNIC { get; set; }
        public string perEmail { get; set; }
        public string perBloodGrp { get; set; }
        public string perCast { get; set; }
        public string perReligion { get; set; }

        public string perImage { get; set; }
        public int perCode { get; set; }
        public int roleId { get; set; }
        public int id { get; set; }
        public int CityId { get; set; }
        public int AreaId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public virtual Login login { get; set; }
        public virtual Role role { get; set; }
        public virtual City city { get; set; }
        public virtual Area area { get; set; }

    }



    [Table("Student")]
    public class Student
    {
        [Key]
        public int stdId { get; set; }
        public int perId { get; set; }
        public string stdRollNo { get; set; }
        public int sesId { get; set; }
        public int secId { get; set; }
        public int camId { get; set; }
        public int classId { get; set; }
        public string stdStatus { get; set; }     
       
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public virtual Person pr { get; set; }
        public virtual Class cls { get; set; }
        public virtual InstSection sec { get; set; }
        public virtual InstSession ses { get; set; }
        public virtual Campus cam { get; set; }

    }

    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int empId { get; set; }
        public int perId { get; set; }
        public string empStatus { get; set; }
        public string empStartSal { get; set; }
        public string empCrntSal { get; set; }
        public DateTime empJoinDate { get; set; }
        public string Remarks { get; set; }
        
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }

    }

    [Table("Relation")]
    public class Relation
    {
        [Key]
        public int RelationId { get; set; }
        public int id { get; set; }
        public int perId { get; set; }
    }

    [Table("StudentFee")]
    public class StudentFee
    {
        [Key]
        public int feeId { get; set; }
        public int stdId { get; set; }
        public int classId { get; set; }
        public int feeTypeId { get; set; }
        public int sesId { get; set; }
        public int secId { get; set; }
        public double feeAmount { get; set; }
        public double PandingAmount { get; set; }
        
        public string feeStatus { get; set; }
        public string StdRemarks { get; set; }
        public DateTime paidDate { get; set; }

        public bool EntryLocked { get; set; }
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
        public virtual FeeType ft { get; set; }

    }

    [Table("TempAttenRpt")]
    public class TempAttenRpt
    {
        [Key]
        public int rptId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Class { get; set; }
        public string RollNo { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string Contact { get; set; }
        public string AttendanceDate { get; set; }
        public string AttendanceTime { get; set; }
        public string AttendanceStatus { get; set; }
        public int userId { get; set; }
    }
}