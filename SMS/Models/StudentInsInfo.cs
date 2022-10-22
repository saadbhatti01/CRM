using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models
{
    [Table("StudentInsInfo")]
    public class StudentInsInfo
    {
        [Key]
        public int StdInsId { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
    }
}