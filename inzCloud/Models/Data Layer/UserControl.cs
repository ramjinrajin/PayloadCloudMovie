using inzCloud.Models.Property;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace inzCloud.Models.Data_Layer
{
    public class UserControl
    {

        

        public static bool AuthenticateUser(string username, string password)
        {
            // ConfigurationManager class is in System.Configuration namespace
            string CS = ConnectSQL.GetConnectionString();
            // SqlConnection is in System.Data.SqlClient namespace
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spAuthenticateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // FormsAuthentication is in System.Web.Security
                string EncryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
                // SqlParameter is in System.Data namespace
                SqlParameter paramUsername = new SqlParameter("@UserName", username);
                SqlParameter paramPassword = new SqlParameter("@Password", EncryptedPassword);//we are not using authentiacated password ,use EncryptedPassword to use authenticated password 

                cmd.Parameters.Add(paramUsername);
                cmd.Parameters.Add(paramPassword);

                con.Open();
                int ReturnCode = (int)cmd.ExecuteScalar();
                return ReturnCode == 1;
            }
        }


        public static bool UserRegister(User _user)
        {


            string CS = ConnectSQL.GetConnectionString();
            // SqlConnection is in System.Data.SqlClient namespace
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spRegisterUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter username = new SqlParameter("@UserName", _user.Username);
                // FormsAuthentication calss is in System.Web.Security namespace
                string encryptedPassword = FormsAuthentication.
                    HashPasswordForStoringInConfigFile(_user.Password, "SHA1");
                SqlParameter password = new SqlParameter("@Password", encryptedPassword);
                SqlParameter email = new SqlParameter("@Email", _user.EmailID);

                cmd.Parameters.Add(username);
                cmd.Parameters.Add(password);
                cmd.Parameters.Add(email);

                con.Open();
                int ReturnCode = (int)cmd.ExecuteScalar();
                return ReturnCode == 1;
            }

        }

        public static List<User> UserAllUser(int FileId)
        {
            List<User> userlist = new List<User>();          
            try
            {

                SqlCommand cmd = ConnectSQL.ExecuteProcedure("SpFileAccess2");
                cmd.Parameters.AddWithValue("@FileId", FileId);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        User user = new User();
                        user.UserID = (int)rdr["UserID"];
                        user.Username = rdr["Username"].ToString();
                        user.Rowstatus = Convert.ToChar(rdr["Status"]);
                        userlist.Add(user);

                    }
                }
                else
                {
                    rdr.Dispose();
                    cmd.Dispose();
                    cmd = ConnectSQL.ExecuteCommand("select UserID,Username,Rowstatus from inz_USERS");
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            User user = new User();
                            user.UserID = (int)rdr["UserID"];
                            user.Username = rdr["Username"].ToString();
                            user.Rowstatus = Convert.ToChar(rdr["Rowstatus"]);
                            userlist.Add(user);

                        }
                    }
                }
               


                return userlist;
            }
            catch
            {
                return userlist;
            }

            
        }

        public static List<User> GetAllusers()
        {
            List<User> userlist = new List<User>();
            using (SqlConnection con = new SqlConnection(ConnectSQL.GetConnectionString()))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select UserID,Username,Rowstatus from inz_USERS", con);
                   SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            User user = new User();
                            user.UserID = (int)rdr["UserID"];
                            user.Username = rdr["Username"].ToString();
                            user.Rowstatus = Convert.ToChar(rdr["Rowstatus"]);
                            userlist.Add(user);

                        }
                    }

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

            return userlist;

        }

        public static int GetUserID(string USername)
        {
            int Userid=0;
            try
            {
                
                SqlCommand cmd = ConnectSQL.ExecuteCommand("select UserId from inz_USERS Where Username=@Username");
                cmd.Parameters.AddWithValue("@Username", USername);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Userid = (int)rdr["UserId"];

                }
                

                return Userid;
            }
            catch
            {              
               return Userid;
            }
        }


    }



}