using inzCloud.Models;
using inzCloud.Models.Business_layer;
using inzCloud.Models.Business_Layer;
using inzCloud.Models.Data_Layer;
using inzCloud.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inzCloud.Controllers
{
    public class AccountSettingsController : Controller
    {
        // GET: AccountSettings
        [Authorize]
        public ActionResult Admin()
        {
            FileDatalayer datalayer = new FileDatalayer();
            List<FileEntity> fileDetails = datalayer.GetAllFiles().ToList();
            return View(fileDetails);
        }

        
        [HttpGet]
        [Authorize]
        public ActionResult FilePermission(int FileID)
        {
            List<User> listUser = UserControl.UserAllUser(FileID);
            return View(listUser);
        }




        [HttpPost]
        [ActionName("FilePermission")]
        [Authorize]
        public ActionResult FileAccess(int FileID, FormCollection frm)
        {
            List<User> listUser = new List<User>();
            try
            {
                string USerNameStatus = "";
                List<string> name = new List<string>();
                listUser = UserControl.UserAllUser(FileID);
                foreach (string Name in frm)
                {
                    foreach (User User in listUser)
                    {
                        if (User.Username == Name)
                        {
                            USerNameStatus = frm[Name].ToString();
                            FileAccessHelper.SaveFileAccess(FileID, User.UserID, USerNameStatus);
                            User.Rowstatus = Convert.ToChar(USerNameStatus);
                        }
                    }
                }
                TempData["AlertMessage"] = "Sucess";
                return View(listUser);
            }
            catch
            {
                TempData["AlertMessage"] = "Error";
                return View(listUser);
            }






        }

        public ActionResult FileSubmit()
        {
            List<User> listUser = UserControl.UserAllUser(9);
            return View(listUser);
        }
    }
}