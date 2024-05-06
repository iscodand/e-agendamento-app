using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Agendamento.Domain.Entities.Common
{
    public abstract class Entity
    {
        public string Id { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
