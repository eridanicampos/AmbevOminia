using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class Pedido : EntityGuid
    {
        public string NumeroPedido { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime? DataHoraEntrega { get; set; }
        public EStatusEntrega StatusEntrega { get; set; }


        public virtual Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public override Task<(bool isValid, List<string> messages)> Validate()
        {
            throw new NotImplementedException();
        }
    }
}
