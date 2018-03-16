using inzCloud.Models.Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inzCloud.Models.Property
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }
        public string EmailID { get; set; }
        public string mobilenno { get; set; }
        public string description { get; set; }
        public char Rowstatus { get; set; }
    }

    public static class USerConfig 
    {
        public static string GetUserName()
        {
                return System.Web.HttpContext.Current.User.Identity.Name;
        }

        public static int GetUserID()
        {
          return  UserControl.GetUserID(System.Web.HttpContext.Current.User.Identity.Name);
             
        }
    }
}