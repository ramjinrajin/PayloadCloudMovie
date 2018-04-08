using inzCloud.Models.Data_Layer;
using inzCloud.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inzCloud.Controllers
{
    public class PaymentController : Controller
    {
       [HttpGet]
        public ActionResult Index()
        {
            
            return View();
        }

       [HttpPost]
       public ActionResult Index(FormCollection frm)
       {
           int FileId = (int)TempData["FileId"];
           int UserId = USerConfig.GetUserID();
           FileDatalayer.SaveFileAccess(UserId, FileId, "A");
           return RedirectToAction("Index", "FileDirectory");
       }
    }
} 