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
    public class FileDirectoryController : Controller
    {
        // GET: FileDirectory
        [Authorize]
        public ActionResult Index()
        {
            FileBusinessLogicHelper FileViewmodel = new FileBusinessLogicHelper();
            List<FileEntity> FileDetails = new List<FileEntity>();
            ViewBag.UserName = System.Web.HttpContext.Current.User.Identity.Name;
            if(ViewBag.UserName=="admin")
            {
               FileDetails = FileViewmodel.GetFileDetails(0);
            }
            else
            {
                FileDetails = FileViewmodel.GetFileDetails(USerConfig.GetUserID());
            }

            ViewBag.host = Request.Url.Host;
            return View(FileDetails);
        }

        [Authorize]
        public ActionResult Download()
        {
            string filename = "docIcon.jpg";
            //string filepath = AppDomain.CurrentDomain.BaseDirectory + "img" + filename;
            var filepath = Path.Combine(Server.MapPath("~/img"), filename);
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);
            string EncryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(filename, "SHA1");
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        int FileId = 0;
        [Authorize]
        public ActionResult GetFile(int FileId)
        {
            try
            {
                string filename = "";
                int UserId=USerConfig.GetUserID();
                FileDatalayer.UpdateFileAccessCount(FileId,UserId );

                if (FileId != 0 && FileId != null)
                {
                    //  FileId = FileId;
                    filename = FileDatalayer.GetFileName(FileId);
                }
                if (filename == "NIL")
                {
                    return View();
                }

                if (FileDatalayer.UserAccessPermission(FileId, UserId))
                {
                    //string filepath = AppDomain.CurrentDomain.BaseDirectory + "img" + filename;
                    var filepath = Path.Combine(Server.MapPath("~/img"), filename);
                    byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                    string contentType = MimeMapping.GetMimeMapping(filepath);

                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = filename,
                        Inline = true,
                    };

                    Response.AppendHeader("Content-Disposition", cd.ToString());

                    return File(filedata, contentType);
                }
                else
                {
                    return Redirect("~/FileDirectory/UnauthorizedFile");
                }
                 
               
            }
            catch
            {
                return View();
            }
      
        }

        [HttpGet]
        public ActionResult Delete(int FileId)
        {
            try
            {
                string fileName = FileDatalayer.GetFileName(FileId);
                if (FileDatalayer.DeleteFile(FileId))
                {
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);


                    if ((System.IO.File.Exists(path)))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                return Redirect("~/AccountSettings/Admin");
            }
          
            catch
            {
                return Redirect("~/AccountSettings/Admin");
            }

           
        }


        public ActionResult DownloadXml(int FileId)
        {

            string filename = "";
                int UserId=USerConfig.GetUserID();
                FileDatalayer.UpdateFileAccessCount(FileId,UserId );

                if (FileId != 0 && FileId != null)
                {
                    //  FileId = FileId;
                    filename = FileDatalayer.GetFileName(FileId);
                }
                if (filename == "NIL")
                {
                    return View();
                }

                if (FileDatalayer.UserAccessPermission(FileId, USerConfig.GetUserID()))
                {
                    string filename2 = "Movie.xfs";
                    if (filename == "NIL")
                    {
                        return View();
                    }
                    var filepath2 = Path.Combine(Server.MapPath("~/img"),"Movie.xfs");
                    TextWriter tw = System.IO.File.CreateText(filepath2);

                    tw.WriteLine(FileId);

                    tw.Close();



                    //string filepath = AppDomain.CurrentDomain.BaseDirectory + "img" + filename;
                    var filepath = Path.Combine(Server.MapPath("~/img"), "Movie.xfs");
                    byte[] filedata = System.IO.File.ReadAllBytes(filepath2);
                    string contentType = MimeMapping.GetMimeMapping(filepath2);

                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = filename2,
                        Inline = true,
                    };

                    Response.AppendHeader("Content-Disposition", cd.ToString());

                    return File(filedata, contentType);
                }

                else
                {
                    return Redirect("~/FileDirectory/UnauthorizedFile");
                }

            
        }

        public ActionResult UnauthorizedUser()
        {
            return View();
        }



    }
}