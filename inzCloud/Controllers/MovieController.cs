using inzCloud.Models.Data_Layer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inzCloud.Controllers
{

    public class Files
    {
        public string Name { get; set; }
        public byte[] Document { get; set; }
    }
    public class MovieController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


       [HttpPost]
        public ActionResult Index(HttpPostedFileBase _File)
        {

            byte[] File = new byte[_File.ContentLength];

            Files FileModel = new Files();
            FileModel.Document = File;
            FileModel.Name = _File.FileName;
            _File.InputStream.Read(FileModel.Document, 0, _File.ContentLength);
                
      



            using (SqlConnection con = new SqlConnection(ConnectSQL.GetConnectionString()))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Movies ([File],FileId) VALUES (@File,@FileId)", con);
                    cmd.Parameters.AddWithValue("@File", FileModel.Document);
                    cmd.Parameters.AddWithValue("@FileId", GetFileId());
                    cmd.ExecuteNonQuery();
                }
                catch (System.Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }
            }


            return View();


             
        }

        private int GetFileId()
        {
            int FileId = 0;
            using (SqlConnection con = new SqlConnection(ConnectSQL.GetConnectionString()))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 FileID from inz_file Order by FileID DESC", con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if(rdr.HasRows)
                    {
                        while(rdr.Read())
                        {
                            FileId = (int)rdr[0];
                        }
                    }
                }
                catch (System.Exception)
                {

                    throw;
                }
                finally
                {
                    con.Close();
                }
            }
            return FileId;
        }
    }
}