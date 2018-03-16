using inzCloud.Models.Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inzCloud.Models.Business_Layer
{
    public class FileAccessHelper
    {
        public static bool SaveFileAccess(int FileId,int UserId,string Status)
        {
            if (FileId == null || UserId==null || Status=="")
            {
                return false;
            }
            else
            {
                return FileDatalayer.SaveFileAccess(UserId, FileId, Status);
            }
            
        }

    }
}