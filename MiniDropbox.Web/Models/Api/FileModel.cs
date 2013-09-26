using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models.Api
{
    public class FileModel
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string Iurl { get; set; }
        public bool IsDirectory { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}