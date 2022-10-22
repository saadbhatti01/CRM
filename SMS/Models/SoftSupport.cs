using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public class SoftSupport
    {
        public static string SoftContactNo = System.Configuration.ConfigurationManager.AppSettings["SoftSupport"].ToString();
    }
}