using MediatR;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Compra.Commands.CancelarCompra
{
    public class CancelarCompraCommandHandler : ICommandHandler<CancelarCompraCommand, Unit>
    {
        private readonly IVendaService _vendaService;

        public CancelarCompraCommandHandler(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        public async Task<Unit> Handle(CancelarCompraCommand request, CancellationToken cancellationToken)
        {
            await _vendaService.CancelarVendaAsync(request.VendaId);
            return Unit.Value;
        }
    }
}
