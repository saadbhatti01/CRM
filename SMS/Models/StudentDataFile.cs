using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("StudentDataFile")]
    public class StudentDataFile
    {
        [Key]
        public int fileId { get; set; }
        public string DataFile { get; set; }
        public string Status { get; set; }
        public DateTime UploadedDate { get; set; }
    }
}