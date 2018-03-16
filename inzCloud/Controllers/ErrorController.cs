using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inzCloud.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ViewResult Index()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return Redirect("~/Error/Index");
        }
    }
}