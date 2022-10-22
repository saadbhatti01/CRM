using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public class TeacherAttendance
    {
        public int perId { get; set; }
        public int? sesId { get; set; }
        public int? classId { get; set; }
        public int? secId { get; set; }

        public string Name { get; set; }
        public string RollNo { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public string IN { get; set; }
        public string OUT { get; set; }
        public DateTime Date { get; set; }
    }
}