using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniDropbox.Domain.Entities
{
    public class FileVersion:IEntity
    {
        public virtual long Id { get; set; }
        public virtual string AmazonVersionId { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual bool IsArchived { get; set; }
    }
}
