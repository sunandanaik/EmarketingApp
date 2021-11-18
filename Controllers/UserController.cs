using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmarketingApp.Models;
using PagedList;

namespace EmarketingApp.Controllers
{
    public class UserController : Controller
    {
        dbemarketingEntities db = new dbemarketingEntities();
        // GET: User
        public ActionResult Index(int?page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.Where(x => x.cat_status == "1").OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_category> catList = list.ToPagedList(pageindex, pagesize);

            return View(catList);
        }

        [HttpGet]
        public ActionResult SignUp()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(tbl_user model,HttpPostedFileBase imgfile)
        {
            string path = Uploadingfile(imgfile); //function calling here
            if (path.Equals("-1"))
            {
                ViewBag.Error = "Image Could not be Uploaded";
            }
            else
            {
                tbl_user user = new tbl_user();
                user.u_name = model.u_name;
                user.u_email = model.u_email;
                user.u_password = model.u_password;
                user.u_image = path;
                user.u_contact = model.u_contact;
                db.tbl_user.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }

        //Function to upload Profile image
        public string Uploadingfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1"; //bcoz function has string returntype.
            int random = r.Next();
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the extension
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        // extract only the filename
                        string fileName = Path.GetFileName(file.FileName);
                        // store the file inside ~/Content/uploads folder
                        path = Path.Combine(Server.MapPath("~/Content/uploads"), random + fileName);
                        file.SaveAs(path);
                        path = "~/Content/uploads/" + random + fileName;
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

        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_user model)
        {

            tbl_user user = db.tbl_user.Where(a => a.u_email == model.u_email && a.u_password == model.u_password).SingleOrDefault();
            if (user != null)
            {
                //If successful logged in then create Session
                Session["user_Sess_id"] = user.u_id.ToString(); //to maintain/store session
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Invalid Username/Password";
            }
            //ModelState.Clear();
            return View();
        }

        //After User logs in Successfully
        [HttpGet]
        public ActionResult CreateAd()
        {
            if (Session["user_Sess_id"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                List<tbl_category> catList = db.tbl_category.ToList();
                ViewBag.categoryList = new SelectList(catList, "cat_id", "cat_name");
                
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult CreateAd(tbl_product model,HttpPostedFileBase imgfile)
        {
            string path = Uploadingfile(imgfile); //function calling here
            if (path.Equals("-1"))
            {
                ViewBag.Error = "Image Could not be Uploaded";
            }
            else
            {
                tbl_product prod = new tbl_product();
                prod.pro_name = model.pro_name;
                prod.pro_image = path;
                prod.pro_desc = model.pro_desc;
                prod.pro_price = model.pro_price;
                prod.pro_fk_cat = model.pro_fk_cat;
                prod.pro_fk_user = Convert.ToInt32(Session["user_Sess_id"].ToString());
                db.tbl_product.Add(prod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
                return View();
        }

        public ActionResult ViewAds(int?id, int?page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_product.Where(x=>x.pro_fk_cat==id).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<tbl_product> catList = list.ToPagedList(pageindex, pagesize);

            return View(catList);
        }

        [HttpPost]
        public ActionResult ViewAds(int?id,int? page,string search)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_product.Where(x => x.pro_name.Contains(search)).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<tbl_product> prodList = list.ToPagedList(pageindex, pagesize);

            return View(prodList);
        }

        public ActionResult ViewAd(int? id)
        {
            AdViewModel avm = new AdViewModel();
            tbl_product prod = db.tbl_product.Where(x => x.pro_id == id).SingleOrDefault();
            avm.pro_id = prod.pro_id;
            avm.pro_name = prod.pro_name;
            avm.pro_desc = prod.pro_desc;
            avm.pro_image = prod.pro_image;
            avm.pro_price = prod.pro_price;

            tbl_category cat = db.tbl_category.Where(x => x.cat_id == prod.pro_fk_cat).SingleOrDefault();
            avm.cat_name = cat.cat_name;

            tbl_user usr = db.tbl_user.Where(x => x.u_id == prod.pro_fk_user).SingleOrDefault();
            avm.u_name = usr.u_name;
            avm.u_image = usr.u_image;
            avm.u_contact = usr.u_contact;
            avm.pro_fk_user=usr.u_id;

            return View(avm);
        }

        public ActionResult DeleteAd(int?id)
        {
            tbl_product prod = db.tbl_product.Where(x => x.pro_id == id).SingleOrDefault();
            db.tbl_product.Remove(prod);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult LogOut()
        {
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}