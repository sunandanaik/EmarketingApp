using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace EmarketingApp.Models
{
    public class ReCaptcha
    {
        public string Key = "6Ld3hAkfAAAAAE-EQlZ7mECazmFWykvmBBVT86tj";

        public string Secret = "6Ld3hAkfAAAAAAqk2GMgAnC24XRTU6BCCIFYP0EJ";
        public string Response { get; set; }
    }
}