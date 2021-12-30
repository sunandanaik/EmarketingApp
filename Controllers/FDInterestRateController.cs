using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmarketingApp.Models;
using EmarketingApp.DbOperations;

namespace EmarketingApp.Controllers
{
    public class FDInterestRateController : Controller
    {
        FDInterestRepository fdrepository = null;

        public FDInterestRateController()
        {
            fdrepository = new FDInterestRepository();
        }

        [HttpPost]
        public JsonResult GetFDRate(int Currency, int inputValue, string pay_out, int tenureValue,string dtype)
        {
            //Response.Write("Hello from MVC action method");
            try
            {
                var calculatedFDInterest = fdrepository.GetCalculatedFDInterest(Currency, inputValue, pay_out, tenureValue, dtype);
                var rate = calculatedFDInterest  / inputValue;
                var dataToSend = new
                {
                    Calculated = Math.Round((double)calculatedFDInterest, 2),
                    FDRate = Math.Round((double)rate, 2)
                };
                return Json(dataToSend);
            }
            catch(Exception ex)
            {
                //Log.Error(ex);
                return Json(new { data = "Error: " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
            //return null;
        }


        // GET: FDInterestRate
        public ActionResult Index()
        {
            return View();
        }
    }
}