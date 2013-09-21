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
            public string Name { get; set; }
            public string LastName { get; set; }
            [Editable(false)]
            [HiddenInput(DisplayValue = true)]
            public int SpaceLimit { get; set; }
             [HiddenInput(DisplayValue = true)]
            public int UsedSpace { get; set; }
            //public string Password{ get; set; }
            //public string EMail { get; set; }
            
            //public DateTime BirthDate;
            //public Byte[] Picture;
        }
}