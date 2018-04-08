using inzCloud.Models.Business_Layer;
using inzCloud.Models.Property;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace inzCloud.Models.Data_Layer
{
    public class FileDatalayer
    {
        public static bool SaveFileToDatabase(FileEntity file)
        {
             
                using(SqlConnection con = new SqlConnection(ConnectSQL.GetConnectionString()))
                {
                    try
                    {
                        con.Open();
                        int FileId = 0;
                        SqlCommand cmd = new SqlCommand("spInsertFileDetails", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FileName", file.Name);
                        cmd.Parameters.AddWithValue("@description", file.Description);
                        cmd.Parameters.AddWithValue("@Key", file.Key);
                        cmd.Parameters.AddWithValue("@UserId", file.UserId);
                        cmd.Parameters.AddWithValue("@IsEncrypted", 1);
                        cmd.Parameters.AddWithValue("@FileType", file.FileType);
                        cmd.Parameters.AddWithValue("@Document", file.Document);
                        //FileId = (int)cmd.ExecuteScalar();
                        return InsertInitalApproveForCurrentUser(FileId);
                    }
                    catch (Exception)
                    {
                        
                        throw;
                    }
                    finally
                    {
                        con.Close();
                    }

                }
               

            

        }

        public static bool InsertInitalApproveForCurrentUser(int FileId)
        {
            try
            {
                string Status = "D";
                int CurrentUserId = USerConfig.GetUserID();
                List<User> listUser = new List<User>();
                listUser = UserControl.GetAllusers();

                foreach (User User in listUser)
                {
                    if (CurrentUserId == User.UserID)
                    {
                        Status = "A";
                    }

                    FileAccessHelper.SaveFileAccess(FileId, User.UserID, Status);


                }

                return true;
            }
            catch
            {

                throw;
            }


        }


        public static bool SaveFileAccess(int UserId, int FileId, string Status)
        {
            try
            {
                SqlCommand cmd = ConnectSQL.ExecuteProcedure("spUpdateFileAccess");
                cmd.Parameters.AddWithValue("@FileId", FileId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {

                return false;
            }

        }


        public IEnumerable<FileEntity> GetFileDetails(int UserId)
        {




            List<FileEntity> FilesCollection = new List<FileEntity>();
            SqlCommand cmd;
            using (SqlConnection con = new SqlConnection(ConnectSQL.GetConnectionString()))
            {
                con.Open();
                if (UserId == 0)//is admin
                {
                    cmd = new SqlCommand("select * from inz_file  order by 1 desc", con);
                }
                else
                {
                    cmd = new SqlCommand("select * from inz_file  order by 1 desc", con);
                    cmd.Parameters.AddWithValue("@UserID", UserId);
                }

                cmd.CommandType = CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    FileEntity _File = new FileEntity();
                    _File.FileId = (int)rdr["FileID"];
                    _File.Name = rdr["FileName"].ToString();
                    _File.Description = rdr["Description"].ToString();
                    _File.FileType = rdr["FileType"].ToString();
                    _File.Document = rdr["DocumentName"].ToString();
                    FilesCollection.Add(_File);
                }

            }

            return FilesCollection;

        }

        public static List<int> GetFileCount(int UserId)
        {
            List<int> Countlist = new List<int>();

            try
            {
                SqlCommand cmd = ConnectSQL.ExecuteCommand("select [Count] from inz_file_status Where UserId=@UserId");
                cmd.Parameters.AddWithValue("@UserID", UserId);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Countlist.Add((int)(rdr["Count"]));

                }
                if (Countlist.Count == 0)
                {
                    Countlist.Add(0);
                }

                return Countlist;
            }
            catch
            {
                Countlist.Add(0);
                return Countlist;
            }


        }

        public static string GetFileName(int FileId)
        {
            string FileName = "NIL";
            try
            {

                SqlCommand cmd = ConnectSQL.ExecuteCommand("select [DocumentName] from inz_file Where FileId=@FileId");
                cmd.Parameters.AddWithValue("@FileId", FileId);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        FileName = rdr["DocumentName"].ToString();

                    }

                }



                return FileName;
            }
            catch
            {

                return FileName;
            }
        }

        public IEnumerable<FileEntity> GetAllFiles()
        {

            List<FileEntity> FilesCollection = new List<FileEntity>();

            using (SqlConnection con = new SqlConnection(ConnectSQL.GetConnectionString()))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from inz_file", con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    FileEntity _File = new FileEntity();
                    _File.FileId = (int)rdr["FileID"];
                    _File.Name = rdr["FileName"].ToString();
                    _File.Description = rdr["Description"].ToString();
                    _File.FileType = rdr["FileType"].ToString();
                    _File.Document = rdr["DocumentName"].ToString();
                    FilesCollection.Add(_File);
                }

            }

            return FilesCollection;

        }

        public static bool UpdateFileAccessCount(int fileId, int UserID)
        {
            try
            {
                SqlCommand cmd = ConnectSQL.ExecuteProcedure("spUpdateFileStatus");
                cmd.Parameters.AddWithValue("@FileID", fileId);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {

                return false;
            }
        }

        public static bool DeleteFile(int fileId)
        {
            try
            {
                SqlCommand cmd = ConnectSQL.ExecuteCommand("Delete from inz_file where FileID=@FileId Delete from inz_file_status where FileID=@FileId");
                cmd.Parameters.AddWithValue("@FileID", fileId);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {

                return false;
            }
        }

        public static bool UserAccessPermission(int fileId, int UserID)
        {
            string Access = "D";
            try
            {

                SqlCommand cmd = ConnectSQL.ExecuteCommand("select [Status] from inzFileAccess where USERID=@UserId and FileId=@FileId");
                cmd.Parameters.AddWithValue("@FileId", fileId);
                cmd.Parameters.AddWithValue("@UserId", UserID);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Access = rdr["Status"].ToString();

                    }

                }



                return Access == "A";
            }
            catch
            {

                return false;
            }
        }
    }
}