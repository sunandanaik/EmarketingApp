using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmarketingApp.Models
{
    public class Customer
    {
        public int Contact_id { get; set; }
        public string Contact_Name { get; set; }
        public string Email_Address { get; set; }
        public string Phone_No { get; set; }
        public string Account_holder { get; set; }
        public string Preferred_Branch { get; set; }
        public string Query_Type { get; set; }
        public string Message { get; set; }
    }
}