using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniDropbox.Domain.Entities
{
    public class SharedWorking : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string UserReceive { get; set; }
        public virtual string UserShared { get; set; }
        public virtual long File_Id { get; set; }
        public virtual string Url { get; set; }
    }

}

