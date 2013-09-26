using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Web.Mvc;
using Microsoft.Web.Mvc.Controls;

namespace MiniDropbox.Web.Models
{
    public class FolderSharedModelRezdWrite
    {
        [Display(Name = "Emails : ")]
        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        [StringLength(2000)]
        [UIHint("LimitedTextArea")]
        [DataType(DataType.MultilineText)]
        public string Email { get; set; }
        /*[Display(Name = "Read permissions : ")]
        public bool IsReader { get; set; }
        [Display(Name = "Write permissions : ")]
        public bool IsWrite { get; set; }*/
        




    }
}