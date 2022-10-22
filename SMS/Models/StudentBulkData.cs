using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("StudentBulkData")]
    public class StudentBulkData
    {
        [Key]
        public int BulkId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string CNIC { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public string BloodGroup { get; set; }
        public string ContactOne { get; set; }
        public string ContactTwo { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Cast { get; set; }
        public string Religion { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string Campus { get; set; }
        public string Session { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string RollNumber { get; set; }
        public string StudentStatus { get; set; }
    }
}