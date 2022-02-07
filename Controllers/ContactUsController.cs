using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using EmarketingApp.Models;
using EmarketingApp.DbOperations;

namespace EmarketingApp.Controllers
{
    public class ContactUsController : Controller
    {
        private ContactRepository objContactRepository;

        public ContactUsController()
        {
            objContactRepository = new ContactRepository();
        }
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
            //Response.Write("Testing");
            try
            {
                List<tbl_setting> setList = new List<tbl_setting>();
                setList = objContactRepository.GetSMTPSetting("SMTP"); //function call from ContactRepository
                string FromMail = "", Port = "", Host = "", Password = "",ToMail="";

                if (setList != null && setList.Count > 0)
                {
                     FromMail = setList.Where(x => x.Key == "FromMail").FirstOrDefault().Value;
                     Port = setList.Where(x => x.Key == "Port").FirstOrDefault().Value;
                     Host = setList.Where(x => x.Key == "Host").FirstOrDefault().Value;
                     Password = setList.Where(x => x.Key == "Password").FirstOrDefault().Value;
                     ToMail = setList.Where(x => x.Key == "ToMail").FirstOrDefault().Value;
                }

                    MailMessage mail = new MailMessage(FromMail, ToMail);
                    //mail.To.Add(new MailAddress(ToMail)); //Where mail will be sent
                    //mail.From = new MailAddress(FromMail); //Email which you are getting from contact us page 
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

                    //The following SMTP Server Address details of gmail added in Web.config file.
                    //You can also mention below here.
                    smtp.Host = Host;
                    smtp.Port = Convert.ToInt32(Port);
                    smtp.UseDefaultCredentials = true;
                    NetworkCredential nc = new NetworkCredential(FromMail, Password);

                    smtp.Credentials = nc;
                    // Smtp Email ID and Password For authentication
                    smtp.EnableSsl = true; //if SSL certificate
                    smtp.Send(mail);
                    ViewBag.Message = "Thank you for contacting us. We are reviewing your request and we'll get in touch as soon as possible.";
                    //OR
                    TempData["SuccessMessage"]= "Thank you for contacting us. We are reviewing your request and we'll get in touch as soon as possible.";

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Something went Wrong. Please try again !" + ex.Message+" and "+ex.StackTrace;
            }

            return View("ContactUs");
        }

    }
}