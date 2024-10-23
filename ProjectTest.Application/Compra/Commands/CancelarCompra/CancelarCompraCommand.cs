using MediatR;
using ProjectTest.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Compra.Commands.CancelarCompra
{
    public class CancelarCompraCommand : ICommand<Unit>
    {
        public Guid VendaId { get; set; }
    }
}
