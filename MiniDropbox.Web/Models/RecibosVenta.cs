using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models
{
    public class RecibosVenta
    {
        public string Transaccion { get; set; }
        public string Descripcion { get; set; }
        public string User { get; set; }
        public double Total { get; set; }
        public DateTime Fecha { get; set; }
    }
}