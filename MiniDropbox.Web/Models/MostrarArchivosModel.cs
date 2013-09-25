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
    }
}