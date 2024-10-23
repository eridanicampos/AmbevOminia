using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class EnderecoEntrega : EntityGuid
    {
        public string CEP { get; set; }
        public string Rua { get; set; }
        public string? Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public override Task<(bool isValid, List<string> messages)> Validate()
        {
            throw new NotImplementedException();
        }
    }
}
