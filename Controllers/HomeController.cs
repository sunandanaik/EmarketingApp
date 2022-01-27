using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmarketingApp.Models;
using System.Net.Mail; //to send email
using System.Net;
using Rotativa; //To save as PDF
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
            //Using Application Variable
            //ViewBag.OfficeName = HttpContext.Application["office_name"];
            //OR Using Session variable
            //ViewBag.OfficeName = Session["office_name"];
            //ViewBag.Date = Session["Session_Start"];

            //For Testing
            string[] userArr = { "Hrutu", "hrutu@gmail.com", "12353454", "Panaji", "Others", "Test" };
            TempData["userData"] = userArr;
            TempData.Keep("userData");

            return View();
        }

        
        public ActionResult Contact()
        {
            ViewBag.Message = "";
            
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
                //NetworkCredential nc = new NetworkCredential("youremail@gmail.com", "password");

                //smtp.Credentials = nc;
                // Smtp Email ID and Password For authentication
                //smtp.EnableSsl = true;
                smtp.Send(mail);
                ViewBag.Message = "Thank you for contacting us. We are reviewing your request and we'll get in touch as soon as possible.";
                   

            }
            catch(Exception ex)
            {
                ViewBag.Message = "Something went Wrong. Please try again !"+ex.Message;
            }

            return View("Contact");
        }



        //PrintPartialViewToPdf Code
        
        public ActionResult PrintPartialViewToPdf(string Contact_Name, string Email_Address, string Phone_No, string Preferred_Branch, string Query_Type, string Message)
        {
            string[] userArr = { Contact_Name, Email_Address, Phone_No, Preferred_Branch, Query_Type, Message };
            TempData["userData"] = userArr;
            

            TempData.Keep("userData");

            var report = new PartialViewAsPdf("~/Views/Home/DetailCustomer.cshtml");
            return report;
            
           // return Json(report,JsonRequestBehavior.AllowGet);
        }

       

        [HttpPost]
        public ActionResult Contact(string Contact_Name, string Email_Address, string Phone_No, string Preferred_Branch, string Query_Type, string Message)
        {
            string[] userArr = { Contact_Name, Email_Address, Phone_No, Preferred_Branch, Query_Type, Message };
            TempData["userData"] = userArr;


            TempData.Keep("userData");

            return View("CreatePdf");
        }

        //Using iTextSharp Library
        public FileResult CreatePdf(string Contact_Name, string Email_Address, string Phone_No, string Preferred_Branch, string Query_Type, string Message)
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //File name to be created
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 2 columns
            PdfPTable tableLayout = new PdfPTable(1);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table
            //file will be created in this path.
            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF 
            doc.Add(Add_Content_To_PDF(tableLayout,Contact_Name, Email_Address, Phone_No, Preferred_Branch, Query_Type, Message));

            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }

        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, string Contact_Name, string Email_Address, string Phone_No, string Preferred_Branch, string Query_Type, string Message)
        {
            float[] headers = {50};                 //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File width percentage
            tableLayout.HeaderRows = 1;

            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(new Phrase("Customer Details", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });
            ////Add header
            AddCellToHeader(tableLayout, "Contact Name");
            AddCellToHeader(tableLayout, "Email ID");
            AddCellToHeader(tableLayout, "Phone No");
            //AddCellToHeader(tableLayout, "Account Holder?");
            AddCellToHeader(tableLayout, "Preferred Branch");
            AddCellToHeader(tableLayout, "Query Type");
            AddCellToHeader(tableLayout, "Message");

            //Add Body
            AddCellToBody(tableLayout, Contact_Name);
            AddCellToBody(tableLayout, Email_Address);
            AddCellToBody(tableLayout, Phone_No);
            AddCellToBody(tableLayout, Preferred_Branch);
            AddCellToBody(tableLayout, Query_Type);
            AddCellToBody(tableLayout, Message);
            //foreach (var user in (string[])TempData["userData"])
            //{
            //    AddCellToBody(tableLayout, user);
            //    //AddCellToBody(tableLayout, user.Contact_Name);
            //    //AddCellToBody(tableLayout, user.Gender);
            //    //AddCellToBody(tableLayout, user.City);
            //    //AddCellToBody(tableLayout, user.Hire_Date.ToString());
            //}

            return tableLayout;
        }
        // Method to add single cell to the Header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0) });
        }

        // Method to add single cell to the body
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        }



        //---------------------------------------------------------------------------------

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

        public JsonResult GetIntPayOut(string ten)
        {
            List<SelectListItem> ipo = new List<SelectListItem>();
            switch (ten)
            {
                case "0":
                    ipo.Add(new SelectListItem { Text = "Select", Value = "0" });
                    ipo.Add(new SelectListItem { Text = "Maturity Interest Payment", Value = "maturity_int_pay" });
                    ipo.Add(new SelectListItem { Text = "Annual Interest Payment", Value = "annual_int_pay" });
                    break;

                case "3":
                    ipo.Add(new SelectListItem { Text = "Select", Value = "0" });
                    ipo.Add(new SelectListItem { Text = "Annual Interest Payment", Value = "annual_int_pay" });
                    break;
                case "6":
                    ipo.Add(new SelectListItem { Text = "Select", Value = "0" });
                    ipo.Add(new SelectListItem { Text = "Annual Interest Payment", Value = "annual_int_pay" });
                    break;
                case "12":
                    ipo.Add(new SelectListItem { Text = "Select", Value = "0" });
                    ipo.Add(new SelectListItem { Text = "Maturity Interest Payment", Value = "maturity_int_pay" });
                    ipo.Add(new SelectListItem { Text = "Annual Interest Payment", Value = "annual_int_pay" });
                    break;
                case "24":
                    ipo.Add(new SelectListItem { Text = "Select", Value = "0" });
                    ipo.Add(new SelectListItem { Text = "Maturity Interest Payment", Value = "maturity_int_pay" });
                    ipo.Add(new SelectListItem { Text = "Annual Interest Payment", Value = "annual_int_pay" });
                    break;
                case "36":
                    ipo.Add(new SelectListItem { Text = "Select", Value = "0" });
                    ipo.Add(new SelectListItem { Text = "Maturity Interest Payment", Value = "maturity_int_pay" });
                    ipo.Add(new SelectListItem { Text = "Annual Interest Payment", Value = "annual_int_pay" });
                    break;
                case "48":
                    ipo.Add(new SelectListItem { Text = "Select", Value = "0" });
                    ipo.Add(new SelectListItem { Text = "Maturity Interest Payment", Value = "maturity_int_pay" });
                    ipo.Add(new SelectListItem { Text = "Annual Interest Payment", Value = "annual_int_pay" });
                    break;
                case "60":
                    ipo.Add(new SelectListItem { Text = "Select", Value = "0" });
                    ipo.Add(new SelectListItem { Text = "Maturity Interest Payment", Value = "maturity_int_pay" });
                    ipo.Add(new SelectListItem { Text = "Annual Interest Payment", Value = "annual_int_pay" });
                    break;
            }

            return Json(ipo, JsonRequestBehavior.AllowGet);
        }
    }
}