using inzCloud.Models.Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inzCloud.Models.Business_layer
{
    public class FileBusinessLogicHelper
    {

        public string SaveFileDetails(FileEntity file)
        {
           if(file.Name!=""&&file.Description!=""&&file.Document!=""&&file.Key!="")
           {
               if (file.File==null)
               {
                   return "NoFile";
               }
               if(file.Document.Contains("pdf"))
               {
                   file.FileType = "PDF";
               }
               else if (file.Document.Contains("jpeg"))
               {
                   file.FileType = "IMAGE";
               }
               else if (file.Document.Contains("jpg"))
               {
                   file.FileType = "IMAGE";
               }
               else if (file.Document.Contains("docx"))
               {
                   file.FileType = "DOCUMENT";
               }
               else if (file.Document.Contains("doc"))
               {
                   file.FileType = "DOCUMENT";
               }
               else
               {
                   file.FileType = "NIL";
               }

               if (FileDatalayer.SaveFileToDatabase(file))

                   return "Sucess";

               else

                   return "Failed";
           }
            else
           {
               return "empty";
           }
            
        }

        public List<FileEntity> GetFileDetails(int USerId)
        {
            FileDatalayer fileDataLayer = new FileDatalayer();
            return fileDataLayer.GetFileDetails(USerId).ToList();
        }


    }
}