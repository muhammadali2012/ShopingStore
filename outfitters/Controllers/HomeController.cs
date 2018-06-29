using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using outfitters.Models;
using System.IO;
using System.Data.Entity.Validation;
using System.Data.Entity;


//using outfitters.Controllers;

namespace outfitters.Controllers
{
    public class HomeController : Controller
    {
        //private singup s = new singup();
        private outfiterEntities1 db = new outfiterEntities1();
        ///  private outfitersEntities1 db1 = new outfitersEntities1();

        public ActionResult Index()
        {
            List<int> ls = new List<int>();
            // ls.Add(992);
            Session["cart"] = ls;
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Payment()
        {
            return View();
        }
        public ActionResult Store()
        {
            return View();
        }
        public ActionResult Faq()
        {
            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Admin(product p)
        {

            HttpPostedFileBase file1 = Request.Files["FileUploadf"];
            HttpPostedFileBase file2 = Request.Files["FileUploadb"];


            if (ModelState.IsValid)
            {


                //--------------------todays time saving------------------------- 
                DateTime today = DateTime.Today;
                p.date = today;

                //-------------------------catagory--------------------

                string s1 = p.catagory;
                //----------------------image saving ---------------------

                if (file1 != null && file1.ContentLength > 0)
                {
                    var filename = Path.GetFileName(file1.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    file1.SaveAs(path);
                    p.imgf = "../images/" + filename;
                }


                if (file2 != null && file2.ContentLength > 0)
                {
                    var filename = Path.GetFileName(file2.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    file2.SaveAs(path);
                    p.imgb = "../images/" + filename;
                }









































                db.products.Add(p);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }







                // db.SaveChanges();
                return RedirectToAction("Index");
            }

            //string s = p.catagory;
            // DateTime today = DateTime.Today;
            // p.date = today;
            // string s= p.catagory;
            return View(p);
        }



        [HttpPost]
        public ActionResult Save(HttpPostedFileBase FileUploadf, HttpPostedFileBase FileUploadb)
        {


            HttpPostedFileBase file1 = Request.Files["FileUploadf"];
            HttpPostedFileBase file2 = Request.Files["FileUploadb"];


            if (FileUploadf != null && FileUploadf.ContentLength > 0)
            {
                var filename = Path.GetFileName(FileUploadf.FileName);
                var path = Path.Combine(Server.MapPath("~/images"), filename);
                FileUploadf.SaveAs(path);
            }


            if (FileUploadb != null && FileUploadb.ContentLength > 0)
            {
                var filename = Path.GetFileName(FileUploadb.FileName);
                var path = Path.Combine(Server.MapPath("~/images"), filename);
                FileUploadb.SaveAs(path);
            }



            product p = new product();
            Admin(p);

            return RedirectToAction("Index");

        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Buy(int id)
        {

            return View();
        }
        public ActionResult Carto()
        {

            List<int> lsid = (List<int>)Session["cart"];
            List<productholder> lspd = new List<productholder>();

            foreach (int i in lsid)
            {


                productholder p = (from s in db.products
                                   where s.Id == i
                                   select new productholder
                                   {
                                       id = (int)s.Id,
                                       price = (int)s.price,
                                       name = s.name,
                                       imgf = s.imgf,
                                       imgb = s.imgb,
                                   }).Single();
                lspd.Add(p);
            }

            return View(lspd);
        }








        public ActionResult Mens(String cat, String specification)
        {

            //for inilization and backup
            var items = (from s in db.products
                         where s.catagory.StartsWith(cat)
                         select new productholder
                         {
                             id = (int)s.Id,
                             price = (int)s.price,
                             name = s.name,
                             imgf = s.imgf,
                             imgb = s.imgb,
                         }).ToList();
            //----------------------------------------------------



            if (specification == "newarival")
            {
                DateTime today = DateTime.Today;
                items = (from s in db.products
                         where s.catagory.StartsWith(cat)
                         && s.date == today
                         select new productholder
                         {
                             id = (int)s.Id,
                             price = (int)s.price,
                             name = s.name,
                             imgf = s.imgf,
                             imgb = s.imgb,
                         }).ToList();

            }
            else if (specification == "discounted")
            {

                items = (from s in db.products
                         where s.catagory.StartsWith(cat)
                         && s.discount > 10
                         select new productholder
                         {
                             id = (int)s.Id,
                             price = (int)s.price,
                             name = s.name,
                             imgf = s.imgf,
                             imgb = s.imgb,
                         }).ToList();

            }
            else if (specification == "sale")
            {

                items = (from s in db.products
                         where s.catagory.StartsWith(cat)
                         && s.discount > 50
                         select new productholder
                         {
                             id = (int)s.Id,
                             price = (int)s.price,
                             name = s.name,
                             imgf = s.imgf,
                             imgb = s.imgb,
                         }).ToList();

            }
            else if (specification == "latest")
            {
                DateTime today = DateTime.Today;
                items = (from s in db.products
                         where s.catagory.StartsWith(cat)
                         && s.date == today
                         select new productholder
                         {
                             id = (int)s.Id,
                             price = (int)s.price,
                             name = s.name,
                             imgf = s.imgf,
                             imgb = s.imgb,
                         }).ToList();

            }

            else if (specification == "$10")
            {
                DateTime today = DateTime.Today;
                items = (from s in db.products
                         where s.catagory.StartsWith(cat)
                         && s.price == 10
                         select new productholder
                         {
                             id = (int)s.Id,
                             price = (int)s.price,
                             name = s.name,
                             imgf = s.imgf,
                             imgb = s.imgb,
                         }).ToList();

            }
            else if (specification == "$50")
            {
                DateTime today = DateTime.Today;
                items = (from s in db.products
                         where s.catagory.StartsWith(cat)
                         && s.price < 50
                         select new productholder
                         {
                             id = (int)s.Id,
                             price = (int)s.price,
                             name = s.name,
                             imgf = s.imgf,
                             imgb = s.imgb,
                         }).ToList();

            }









            return View(items);
        }
        public ActionResult Singup()
        {
            singup s = new singup();
            String n = Request["Name"];
            String e = Request["Email"];
            String p = Request["Password"];
            String pn = Request["Phone Number"];
            // String a = Request["Adress"];
            s.Id = 1;
            s.name = n;
            s.email = e;
            s.password = p;
            s.phonenumber = pn;
            db.singups.Add(s);
            db.SaveChanges();
            // ViewBag.Username = n;
            return RedirectToAction("logedindex", "logedin");

        }
        public ActionResult Login()
        {
            string n = Request["Userame"];
            string p = Request["Passwordl"];
            if (n == "admin" && p == "admin")
            {
                return RedirectToAction("Admin", "Home");
            }
            List<singup> ls = db.singups.ToList();
            foreach (var m in ls)
            {
                /* int nl= n.Length; //6
                   int pl = p.Length;
                   int dnl= m.name_.Length;
                   int dpl = m.password.Length;
                   string s = m.password;
                   int sl = s.Length;
                   int c = string.CompareOrdinal(n, m.name_);
                   */



                if (m.name.StartsWith(n) && m.password.StartsWith(p))
                    Session["username"] = n;
                return RedirectToAction("mens", "Home");


            }

            return RedirectToAction("Index", "Home");

        }

        public JsonResult CheckEmail()
        {

            string email = Request["Email"];
            if (email == "")
            {
                email = "bla bla ";
            }

            //check from database
            List<singup> ls = db.singups.ToList();
            foreach (var v in ls)
            {
                if (v.email.StartsWith(email))
                    return this.Json(true, JsonRequestBehavior.AllowGet);
            }

            return this.Json(false, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CheckPhone()
        {

            string phone = Request["Phone Number"];
            if (phone == "")
            {
                phone = "bla bla ";
            }
            List<singup> ls = db.singups.ToList();
            foreach (var v in ls)
            {
                if (v.phonenumber.StartsWith(phone))
                {
                    return this.Json(true, JsonRequestBehavior.AllowGet);
                }
            }

            return this.Json(false, JsonRequestBehavior.AllowGet);

        }


        public JsonResult Idholder()
        {
            string id = Request["ID"];
            int x = Int32.Parse(id);
            List<int> t = (List<int>)Session["cart"];
            t.Add(x);
            Session["cart"] = t;
            foreach (var l in t)
            {
                var o = l;
            }
            return null;

        }




        public JsonResult premover()
        {
            string id = Request["ID"];
            int x = Int32.Parse(id);
            List<int> t = (List<int>)Session["cart"];
            t.Remove(x);
            Session["cart"] = t;
            foreach (var l in t)
            {
                var o = l;
            }
            return null;


        }
    }
}