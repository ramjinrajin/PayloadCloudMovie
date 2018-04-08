using inzCloud.Models;
using inzCloud.Models.Business_layer;
using inzCloud.Models.Data_Layer;
using inzCloud.Models.Property;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace inzCloud.Controllers
{
    public class ControlPanelController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Upload(FormCollection FileDetails, HttpPostedFileBase files)
        {

            try
            {
                FileEntity fileObj = new FileEntity();
                if (FileDetails != null && FileDetails.Count != 0)
                {
                    fileObj.Name = FileDetails["Name"].ToString();
                    fileObj.Description = FileDetails["Description"].ToString();
                    fileObj.Key = FileDetails["key"].ToString();
                    fileObj.Document = FileDetails["Name"].ToString();
                    fileObj.File = files;
                    fileObj.UserId = USerConfig.GetUserID();
                }

                if (fileObj.File != null)
                {
                    if (fileObj.File.ContentLength > 0)
                    {


                        var fileName = Path.GetFileName(fileObj.File.FileName);

                        var path = Path.Combine(Server.MapPath("~/img"), fileName);
                        fileObj.File.SaveAs(path);
                        fileObj.Document = fileName;
                    }

                }
               


                FileBusinessLogicHelper helperClass = new FileBusinessLogicHelper();
                string status = helperClass.SaveFileDetails(fileObj);
                if (status == "empty")
                {
                    TempData["AlertMessage"] = "Error";
                }
                else if (status == "NoFile")
                {
                    TempData["AlertMessage"] = "NoFile";
                }
                else
                { 
                    TempData["AlertMessage"] = "Sucess";
                }

            }
            catch
            {
                TempData["AlertMessage"] = "Invalid";
            }


            return RedirectToAction("Index", "Movie");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Settings()
        {
            List<int> integers = new List<int>
             {
                 2,5,6
             };
            return View(integers);
        }


        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        [ActionName("login")]
        public ActionResult Login_()//
        {


            bool isChecked = false;
            if (Request.Form["remember"] != null)
            {
                isChecked = true;
            }

            if (UserControl.AuthenticateUser(Request.Form["USer name"].ToString(), Request.Form["password"].ToString()))//Authentication in database
            //if (FormsAuthentication.Authenticate(Request.Form["USer name"].ToString(), Request.Form["password"].ToString()))//Authentication in Web.config
            {
                FormsAuthentication.SetAuthCookie(Request.Form["USer name"].ToString(), isChecked);
                if (Request.QueryString["returnUrl"] != "" && Request.QueryString["returnUrl"] != null)
                {
                    string sdf = Request.QueryString["returnUrl"];
                    return Redirect(Request.QueryString["returnUrl"]);
                }
                FormsAuthentication.RedirectFromLoginPage(Request.Form["USer name"].ToString(), isChecked);
                ViewBag.UserName = System.Web.HttpContext.Current.User.Identity.Name;
                return RedirectToAction("index", "ControlPanel");
            }

            else
            {
                if (Request.QueryString["returnUrl"] != "" && Request.QueryString["returnUrl"] != null)
                {

                    return Redirect("~/FileDirectory/UnauthorizedUser");
                }
                TempData["AlertMessage"] = "Invalid";
                return RedirectToAction("login", "ControlPanel");
            }

        }


        [HttpPost]
        public ActionResult RegisterUSer(FormCollection FrmUserDetails)
        {

            User _user = new User();
            _user.Username = FrmUserDetails["username"];
            _user.Password = FrmUserDetails["confirm-password2"];
            _user.EmailID = FrmUserDetails["email"];
            if (UserControl.UserRegister(_user))
            {
                TempData["AlertMessage"] = "Sucess";
                return RedirectToAction("login", "ControlPanel");
            }
            else
            {
                TempData["AlertMessage"] = "Exists";
                return RedirectToAction("login", "ControlPanel");
            }


        }


        [Authorize]
        public ActionResult logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("login", "ControlPanel");


        }
        [HttpGet]
        public object GetFileCount()
        {
            List<int> integers = new List<int>();
            integers = FileDatalayer.GetFileCount(USerConfig.GetUserID());
            return Json(new { integers }, JsonRequestBehavior.AllowGet);
        }



    }
}