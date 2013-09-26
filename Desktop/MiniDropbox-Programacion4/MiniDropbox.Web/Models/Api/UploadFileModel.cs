using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models.Api
{
    public class UploadFileModel
    {
        public HttpPostedFileBase file { get; set; }
        public string CurrentPath { get; set; }
    }
}