using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public class ExpenseVM
    {
        public int ahId { get; set; }
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public string SubSUbHeader { get; set; }
        public int Code { get; set; }
        public bool Visible { get; set; }
    }
}