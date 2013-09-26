using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models.Api
{
    public class CreateFolderModel
    {
        public string folderName { get; set; }
        public string currentPath { get; set; }
    
    }
}