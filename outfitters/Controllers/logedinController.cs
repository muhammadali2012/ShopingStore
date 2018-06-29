using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using outfitters.Models;


namespace outfitters.Controllers
{
    public class logedinController : Controller
    {
        // GET: logedin
        public ActionResult LogedIndex()
        {
            return View();
        }
    }
}