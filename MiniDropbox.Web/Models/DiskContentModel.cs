using System;
using System.ComponentModel.DataAnnotations;

namespace MiniDropbox.Web.Models
{
    public class DiskContentModel
    {
        public long Id { get; set; }
        
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Seleccionado")]
        [Editable(true)]
        public bool Selected { get; set; }
        [Display(Name = "Tipo")]
        public string Type { get; set; }
        [Display(Name = "Fecha Modificacion")]
        public DateTime ModifiedDate { get; set; }
        [Display(AutoGenerateField = false)]
        public bool IsShared { get; set; }
    }
}