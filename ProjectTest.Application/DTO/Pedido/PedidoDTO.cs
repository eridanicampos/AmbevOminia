using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO.Pedido
{
    public class PedidoDTO : EntityPKDTO
    {
        public string NumeroPedido { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime? DataHoraEntrega { get; set; }
        public EStatusEntrega StatusEntrega { get; set; }

        public Guid UsuarioId { get; set; }
    }
}
