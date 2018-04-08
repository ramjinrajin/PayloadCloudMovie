using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;


namespace Device
{
   public class Authenticator
    {
        public static bool AuthenticateUser(string username, string password)
        {
            string db = Environment.CurrentDirectory.ToString();
            using (SqlConnection con = new SqlConnection(string.Format(@"Data Source=(LocalDB)\v11.0;AttachDbFilename={0}\inzCloud.mdf;Integrated Security=True", db)))
            {
                try
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
    }
}
