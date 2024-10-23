using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class AcessoUsuario : EntityGuid
    {
        public virtual Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public override Task<(bool isValid, List<string> messages)> Validate()
        {
            throw new NotImplementedException();
        }
    }
}
