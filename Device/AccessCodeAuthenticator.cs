using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device
{
  public static class AccessCodeAuthenticator
    {
      public static bool GetAccess(int FileId, string AccessCode)
      {
          bool IsValidUser = false;

          string db = Environment.CurrentDirectory.ToString();
          using (SqlConnection con = new SqlConnection(string.Format(@"Data Source=(LocalDB)\v11.0;AttachDbFilename={0}\inzCloud.mdf;Integrated Security=True", db)))
          {
              try
              {
                  con.Open();
                  SqlCommand cmd = new SqlCommand("select * from inz_file where FileID=@FileId And [Key]=@AccessCode", con);
                  cmd.Parameters.AddWithValue("@FileId",FileId);
                  cmd.Parameters.AddWithValue("@AccessCode", AccessCode);
                  SqlDataReader rdr=cmd.ExecuteReader();
                  return rdr.HasRows;
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


          return IsValidUser;
      }
    }
}
