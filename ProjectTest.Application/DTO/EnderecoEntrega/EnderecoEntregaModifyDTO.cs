using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO.EnderecoEntrega
{
    public class EnderecoEntregaModifyDTO : EntityPKDTO
    {
        public string CEP { get; set; }
        public string Rua { get; set; }
        public string? Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public Guid UsuarioId { get; set; }
    }
}
