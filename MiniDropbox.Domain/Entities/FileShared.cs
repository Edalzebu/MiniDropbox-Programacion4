using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniDropbox.Domain
{
    public class FileShared : IEntity
    {
         private readonly IList<File> _files = new List<File>();

        public FileShared()
        {
            _files = new List<File>();
        }

        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual bool IsShared { get; set; }
        public virtual string UserReceive { get; set; }
        public virtual string UserShared { get; set; }
        
        public virtual IEnumerable<File> Files
        {
            get { return _files; }
        }

        public virtual void AddFile(File file)
        {
            if (!_files.Contains(file))
            {
                _files.Add(file);
            }
        }
    }
}
