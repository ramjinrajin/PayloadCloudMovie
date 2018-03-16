using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inzCloud.Models
{
    public class FileEntity
    {
        public int FileId { get; set; }
        public string Name { get; set; }

        public string FileType { get; set; }

        public string Description { get; set; }
        public string Key { get; set; }

        public HttpPostedFileBase File { get; set; }

        public string Document { get; set; }

        public int UserId { get; set; }

        

    }
}