using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Web.Mvc;

namespace MiniDropbox.Web.Models
{
    public class FolderPublicoModel
    {
        [Display(Name = "Escriba su Email Aqui : ")]
        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        [EmailAddress(ErrorMessage = "Debe tener Formato de Email")]
        public string Email { get; set; }
        
    }
}