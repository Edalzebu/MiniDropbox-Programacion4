using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniDropbox.Domain;

namespace MiniDropbox.Domain
{

    public class RecibosVentas : IEntity
    {
        public virtual long Id { get; set; }
        public virtual string Transaccion { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string UserCompro { get; set; }
        public virtual decimal Total { get; set; }
        public virtual DateTime Fecha { get; set; }
    }

}
