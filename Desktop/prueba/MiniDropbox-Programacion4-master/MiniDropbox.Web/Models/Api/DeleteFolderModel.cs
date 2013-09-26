using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models.Api
{
    public class DeleteFolderModel
    {
        public string CurrentPath { get; set; }
        public string FolderName { get; set; }
    }
}