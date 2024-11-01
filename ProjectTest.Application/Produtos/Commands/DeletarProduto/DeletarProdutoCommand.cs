using MediatR;
using ProjectTest.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Produtos.Commands.DeletarProduto
{
    public class DeletarProdutoCommand : ICommand<Unit>
    {
        public Guid Id { get; set; }
    }
}
