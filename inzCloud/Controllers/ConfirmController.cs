using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inzCloud.Controllers
{
    public class ConfirmController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {

            
            return View();
        }



        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            return RedirectToAction("Index","Payment");
        }
    }
}