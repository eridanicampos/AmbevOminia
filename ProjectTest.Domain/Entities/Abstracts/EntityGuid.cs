using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public abstract class EntityGuid : Entity
    {
        public Guid Id { get; set; }

    }
}
