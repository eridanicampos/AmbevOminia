using MediatR;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Produtos.Commands.DeletarProduto
{
    public class DeletarProdutoCommandHandler : ICommandHandler<DeletarProdutoCommand, Unit>
    {
        private readonly IProdutoService _service;

        public DeletarProdutoCommandHandler(IProdutoService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(DeletarProdutoCommand request, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
