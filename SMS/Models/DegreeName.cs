using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models
{
    [Table("Degree")]
    public class Degree
    {
        [Key]
        public int DegreeId { get; set; }
        public string DegreeName { get; set; }
        public bool IsVisible { get; set; }
    }
}