using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    [Table("AccountHeader")]
    public class AccountHeader
    {
        [Key]
        public int ahId { get; set; }
        public string headerName { get; set; }
        public int subHeader { get; set; }
        public int subSubHeader { get; set; }
        public int Code { get; set; }
        public bool isVisible { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}