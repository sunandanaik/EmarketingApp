using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using System.Net.Mail;

namespace EmarketingApp.Controllers
{
    public class ContactUsController : Controller
    {
        // GET: ContactUs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

         
        [HttpPost]
        public ActionResult SubmitForm(string Contact_Name, string Email_Address, string Phone_No, string Preferred_Branch, string Query_Type, string Message)
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

                //SMTP Server Address of gmail added in Web.config file.
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.UseDefaultCredentials = true;
                //NetworkCredential nc = new NetworkCredential("sungitmca@gmail.com", "password");

                //smtp.Credentials = nc;
                // Smtp Email ID and Password For authentication
                //smtp.EnableSsl = true;
                smtp.Send(mail);
                ViewBag.Message = "Thank you for contacting us. We are reviewing your request and we'll get in touch as soon as possible.";


            }
            catch (Exception ex)
            {
                ViewBag.Message = "Something went Wrong. Please try again !" + ex.Message;
            }

            return View("Contact");
        }

    }
}