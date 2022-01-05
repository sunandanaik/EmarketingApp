using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmarketingApp.Models;

namespace EmarketingApp.DbOperations
{
    public class FDInterestRepository
    {
        dbemarketingEntities context = new dbemarketingEntities();
        public double? GetCalculatedFDInterest(int Currency,int inputValue,string pay_out,int tenureValue, string dtype)
        {
            
            try
            {
                double? calculatedFDInterest = null;
                var tenure_in_years = tenureValue / 12;

                var getRateData = (from fd in context.FixedDeposits
                                   where fd.Currency == Currency &&
                                   tenure_in_years >= fd.FromSlab &&
                                   tenure_in_years < fd.ToSlab
                                   select fd.Rate).First();

                
                var n = 1; //by default Compounded yearly
                var rate_in_dec = getRateData / 100;

                if (pay_out == "maturity_int_pay")
                {
                    
                    calculatedFDInterest = inputValue*(Math.Pow((1+rate_in_dec),n*tenure_in_years));
                }
                else if (pay_out == "annual_int_pay")
                {
                    calculatedFDInterest = inputValue * rate_in_dec * tenure_in_years;
                }

                return calculatedFDInterest;

            }
            catch(Exception)
            {
                //Log.Error(ex.Message);
                return null;
            }
        }
    }
}