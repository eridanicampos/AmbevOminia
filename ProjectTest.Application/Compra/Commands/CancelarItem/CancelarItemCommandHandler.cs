using MediatR;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Compra.Commands.CancelarItem
{
    public class CancelarItemCommandHandler : ICommandHandler<CancelarItemCommand, Unit>
    {
        private readonly IVendaService _vendaService;

        public CancelarItemCommandHandler(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        public async Task<Unit> Handle(CancelarItemCommand request, CancellationToken cancellationToken)
        {
            await _vendaService.CancelarItemAsync(request.VendaId, request.ItemId);
            return Unit.Value; 
        }
    }
}
