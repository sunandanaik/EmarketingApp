using EmarketingApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EmarketingApp.DbOperations
{
    public class ContactRepository
    {
        dbemarketingEntities db = new dbemarketingEntities();

        public List<tbl_setting> GetSMTPSetting(string type)
        {
            List<tbl_setting> setList = new List<tbl_setting>();
            try
            {
                setList= (from set in db.tbl_setting
                          where set.Type == type
                          select set).ToList();
                        //{
                        //    Key = set.Key,
                        //    Value = set.Value,
                        //    Type = set.Type
                        //}
            }
            catch(Exception)
            {
                //Log.Error(ex.Message);
                return null;
            }
            return setList;
        }
    }
}