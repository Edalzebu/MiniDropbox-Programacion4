using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniDropbox.Domain.Entities
{
    public class ApiKeys : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string Token { get; set; }
        public virtual long UserId { get; set; }
        public virtual DateTime ExpirationTime { get; set; }

        public bool IsTokenActive()
        {
            if (DateTime.Now > ExpirationTime)
            {
                return false;
            }
            return true;
            
        }
        
    }
}
