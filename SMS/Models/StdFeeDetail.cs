using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public class StdFeeDetail
    {
        public int feeId { get; set; }
        public int sfpId { get; set; }
        public string Stdname { get; set; }
        public string Title { get; set; }
        public string FatherName { get; set; }
        public int FatherCode { get; set; }
        public string RollNo { get; set; }
        public string Contact { get; set; }
        public int stdId { get; set; }
        public int sesId { get; set; }
        public int secId { get; set; }
        public int classId { get; set; }
        public int feeTypeId { get; set; }
         public string Ses { get; set; }
        public string Sec { get; set; }
        public string Class { get; set; }
        public string Fee { get; set; }
        public double FeeAmt { get; set; }

        public double TotalFee { get; set; }
        public string FeeName { get; set; }
        public string FeeStatus { get; set; }
        public string Date { get; set; }
        public double PandingAmt { get; set; }
        public int Dis { get; set; }
        public DateTime FeeDate { get; set; }
        public DateTime PaidDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UserId { get; set; }
        public int CreatedById { get; set; }
        public int UpdatedById { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string CreateRoleName { get; set; }
        public string UpdateRoleName { get; set; }
        public string sfpRemarks { get; set; }
        public double PaidAmt { get; set; }
        public string FeePaidName { get; set; }
        public string FeePaidStatus { get; set; }
        public string StdStatus { get; set; }
        public double Panding { get; set; }
        public string LogIn { get; set; }
        public string Password { get; set; }
       
        public string Phone { get; set; }
        public string Subject { get; set; }
        public int DocId { get; set; }
        public string Note { get; set; }
        public string PersonalNote { get; set; }
        public DateTime Expiry { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
        public string CNIC {get; set; }
    }
}