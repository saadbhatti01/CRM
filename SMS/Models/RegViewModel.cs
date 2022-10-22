using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public class RegViewModel
    {
        [Key]
        public int perId { get; set; }
        public int stdId { get; set; }
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
        public int CityId { get; set; }
        public string CityName { get; set; }
       
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string stdRollNo { get; set; }
        public int sesId { get; set; }
        public string sesName { get; set; }
        public int secId { get; set; }
        public string secName { get; set; }
        public int camId { get; set; }

        public string campusname { get; set; }
        public int feeTypeId { get; set; }
        public int classId { get; set; }
        public string ClassName { get; set; }
        public string stdStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }

        //Previouse Educational Info
        public int DegreeId { get; set; }
        public int StdInsId { get; set; }
        public int TotalMarks { get; set; }
        public int ObtainMarks { get; set; }
        public string RollNo { get; set; }
        public DateTime PassingDate { get; set; }

    }
}