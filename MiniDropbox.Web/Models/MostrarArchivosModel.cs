using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniDropbox.Web.Models
{
    public class MostrarArchivosModel
    {
        public long id { get; set; }
        public FileContentResult file { get; set; }
        public string Bucket { get; set; }
        public string filename { get; set; }
        public string Redirection { get; set; }
    }
}