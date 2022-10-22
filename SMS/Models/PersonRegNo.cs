using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("PersonRegNo")]
    public class PersonRegNo
    {
        [Key]
        public int prId { get; set; }
        public string prBarcode { get; set; }
        public string prRFID { get; set; }
        public string prFirstFing { get; set; }
        public string prSecFing { get; set; }
        public int roleId { get; set; }
        public int perId { get; set; }

        public virtual Person pr { get; set; }
        public virtual Role role { get; set; }
    }
}