using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmarketingApp.Models;
using System.Net.Mail; //to send email
using System.Net;


namespace EmarketingApp.Controllers
{
    public class HomeController : Controller
    {
        dbemarketingEntities db = new dbemarketingEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(string Contact_Name, string Email_Address, string Phone_No, string Preferred_Branch, string Query_Type, string Message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add("sungitmca@gmail.com");//Where mail will be sent
                mail.From = new MailAddress(Email_Address);//Email which you are getting from contact us page 
                mail.Subject = Preferred_Branch + "/" + Query_Type + "/" + Contact_Name;
                
                string userMessage = "";
                userMessage = "<br/>" + Contact_Name;
                userMessage = userMessage + "<br/>Email ID:" + Email_Address;
                userMessage = userMessage + "<br/>Phone No: " + Phone_No;
                userMessage = userMessage + "<br/>Message: " + Message;

                string Body = "Hi, <br/><br/> A new enquiry by user. Detail is as follows:<br/><br/> " + userMessage + "<br/><br/>Thanks";
                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                //SMTP Server Address of gmail
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = true;
                NetworkCredential nc = new NetworkCredential("sungitmca@gmail.com", "password");
                
                smtp.Credentials=nc;
                // Smtp Email ID and Password For authentication
                smtp.EnableSsl = true;
                smtp.Send(mail);
                ViewBag.Message = "Thank you for contacting us. We are reviewing your request and we'll get in touch as soon as possible.";
            }
            catch(Exception ex)
            {
                ViewBag.Message = "Something went Wrong. Please try again !"+ex.Message;
            }
            return View();
        }

        public ActionResult FDCalculator()
        {
            ViewBag.Message = "Fixed Deposit Interest Rate Calculator";

            //Generic List created to populate into UI dropdownlist.
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Fixed Rate Bond", Value = "0" });
            li.Add(new SelectListItem { Text = "Fixed Rate ISA", Value = "1" });
            ViewData["deposittypeslist"] = li; //to pass to the View
            
            return View();
        }

        public JsonResult GetCurrencies(string id)
        {
            List<SelectListItem> currencies = new List<SelectListItem>();
            switch (id)
            {
                case "0":   currencies.Add(new SelectListItem { Text = "Select", Value = "0" });
                            currencies.Add(new SelectListItem { Text = "GBP", Value = "1" });
                            currencies.Add(new SelectListItem { Text = "USD", Value = "2" });
                            currencies.Add(new SelectListItem { Text = "EUR", Value = "3" });
                    break;
                case "1":  currencies.Add(new SelectListItem { Text = "Select", Value = "0" });
                           currencies.Add(new SelectListItem { Text = "GBP", Value = "1" });
                    break;

            }

            return Json(currencies,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTenures(string tdid,string cid)
        {
            List<SelectListItem> tenures = new List<SelectListItem>();
            switch (tdid)
            {
                case "0":
                    switch (cid)
                    {
                        case "1":
                            tenures.Add(new SelectListItem { Text = "Select", Value = "0" });
                            tenures.Add(new SelectListItem { Text = "3 months", Value = "3" });
                            tenures.Add(new SelectListItem { Text = "6 months", Value = "6" });
                            tenures.Add(new SelectListItem { Text = "1 year", Value = "12" });
                            tenures.Add(new SelectListItem { Text = "2 years", Value = "24" });
                            tenures.Add(new SelectListItem { Text = "3 years", Value = "36" });
                            tenures.Add(new SelectListItem { Text = "4 years", Value = "48" });
                            tenures.Add(new SelectListItem { Text = "5 years", Value = "60" });
                            break;

                        case "2":
                            tenures.Add(new SelectListItem { Text = "Select", Value = "0" });
                            tenures.Add(new SelectListItem { Text = "3 months", Value = "3" });
                            tenures.Add(new SelectListItem { Text = "6 months", Value = "6" });
                            tenures.Add(new SelectListItem { Text = "1 year", Value = "12" });
                            tenures.Add(new SelectListItem { Text = "2 years", Value = "24" });
                            tenures.Add(new SelectListItem { Text = "3 years", Value = "36" });
                            tenures.Add(new SelectListItem { Text = "4 years", Value = "48" });
                            tenures.Add(new SelectListItem { Text = "5 years", Value = "60" });
                            break;

                        case "3":
                            tenures.Add(new SelectListItem { Text = "Select", Value = "0" });
                            tenures.Add(new SelectListItem { Text = "3 months", Value = "3" });
                            tenures.Add(new SelectListItem { Text = "6 months", Value = "6" });
                            tenures.Add(new SelectListItem { Text = "1 year", Value = "12" });
                            tenures.Add(new SelectListItem { Text = "2 years", Value = "24" });
                            tenures.Add(new SelectListItem { Text = "3 years", Value = "36" });
                            tenures.Add(new SelectListItem { Text = "4 years", Value = "48" });
                            tenures.Add(new SelectListItem { Text = "5 years", Value = "60" });
                            break;
                    }
                    break;

                case "1":
                    switch (cid)
                    {
                        case "1":
                            tenures.Add(new SelectListItem { Text = "Select", Value = "0" });
                            tenures.Add(new SelectListItem { Text = "1 year", Value = "12" });
                            tenures.Add(new SelectListItem { Text = "2 years", Value = "24" });
                            tenures.Add(new SelectListItem { Text = "3 years", Value = "36" });
                            tenures.Add(new SelectListItem { Text = "4 years", Value = "48" });
                            tenures.Add(new SelectListItem { Text = "5 years", Value = "60" });
                            break;

                    }
                    break;

            }
            
            return Json(tenures,JsonRequestBehavior.AllowGet);
        }
    }
}