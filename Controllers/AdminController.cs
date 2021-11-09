using EmarketingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;

namespace EmarketingApp.Controllers
{
    public class AdminController : Controller
    {
        dbemarketingEntities db = new dbemarketingEntities();
        // GET: Admin
        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_admin model)
        {
            
            tbl_admin admin = db.tbl_admin.Where(a => a.ad_username == model.ad_username && a.ad_password == model.ad_password).SingleOrDefault();
            if (admin != null)
            {
                //If successful logged in then create Session
                Session["admin_Sess_id"] = admin.ad_id.ToString(); //to maintain/store session
                return RedirectToAction("Create");
            }
            else
            {
                ViewBag.Error = "Invalid Username/Password";
            }
            //ModelState.Clear();
            return View();
        }

        //After logging in
        public ActionResult Create()
        {
            if (Session["admin_Sess_id"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(tbl_category model,HttpPostedFileBase imgfile)
        {
            string path = uploadingfile(imgfile); //function calling here
            if(path.Equals("-1"))
            {
                ViewBag.Error = "Image Could not be Uploaded";
            }
            else
            {
                tbl_category cat = new tbl_category();
                cat.cat_name = model.cat_name;
                cat.cat_image = path;
                cat.cat_status = "1";//It means available
                cat.cat_fk_ad = Convert.ToInt32(Session["admin_Sess_id"]);
                db.tbl_category.Add(cat);
                db.SaveChanges();
                // redirect back to the Create AM to show the form once again with clear fields.
                return RedirectToAction("ViewCategory");
            }
            return View();
        }

        //Function to upload image
        public string uploadingfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1"; //bcoz function has string returntype.
            int random = r.Next();
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the extension
                string extension = Path.GetExtension(file.FileName);
                if(extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        // extract only the filename
                        string fileName = Path.GetFileName(file.FileName);
                        // store the file inside ~/Content/uploads folder
                        path = Path.Combine(Server.MapPath("~/Content/uploads"), random + fileName);
                        file.SaveAs(path);
                        path= "~/Content/uploads/" + random + fileName;
                    }
                    catch (Exception)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg,jpeg,png formats are allowed !!');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please Select a File !!');</script>");
                path = "-1";
            }
            return path;
        }

        //To show Category list
        public ActionResult ViewCategory(int?page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.Where(x => x.cat_status == "1").OrderByDescending(x=>x.cat_id).ToList();
            IPagedList<tbl_category> catList = list.ToPagedList(pageindex, pagesize);

            return View(catList);
        }
    }
}