using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities.Enum
{
    public enum EStatusEntrega
    {
        PedidoRecebido,
        PagamentoAprovado,
        EmProcessamento,
        Enviado,
        EmTransito,
        Entregue,
        Cancelado,
        Devolvido
    }
}
