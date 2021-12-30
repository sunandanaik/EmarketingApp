using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmarketingApp.Models
{
    public class FixedDepositModel
    {
        public int FixedDepositId { get; set; }
        public int Currency { get; set; }
        public double Rate { get; set; }
        public double FromSlab { get; set; }
        public double ToSlab { get; set; }
    }
}