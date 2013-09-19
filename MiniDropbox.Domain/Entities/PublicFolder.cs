using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniDropbox.Domain.Entities
{
    public class PublicFolder : IEntity
    {
        private readonly IList<File> _files = new List<File>();

        public PublicFolder()
        {
            _files = new List<File>();
        }

        public virtual IEnumerable<File> Files
        {
            get { return _files; }
        }

        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string Token { get; set; }
        public virtual long File_Id { get; set; }
        public virtual long Account_Id { get; set; }
    }
}
