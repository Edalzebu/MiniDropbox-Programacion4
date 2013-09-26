using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniDropbox.Web.Models
{
    public class AccountProfileModel
        {
            [Display(Name = "Nombre")]
            public string Name { get; set; }
            [Display(Name = "Apellido")]
            public string LastName { get; set; }
            [Display(Name = "Espacio total")]
            [HiddenInput(DisplayValue = true)]
            public int SpaceLimit { get; set; }
             [HiddenInput(DisplayValue = true)]
             [Display(Name = "Espacio Usado")]
            public double UsedSpace { get; set; }
            //public string Password{ get; set; }
            //public string EMail { get; set; }
            
            //public DateTime BirthDate;
            //public Byte[] Picture;
        }
}