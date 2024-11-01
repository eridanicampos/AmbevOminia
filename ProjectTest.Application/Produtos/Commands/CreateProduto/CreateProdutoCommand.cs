using MediatR;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO.Produtos;

namespace ProjectTest.Application.Produtos.Commands.CreateProduto
{
    public class CreateProdutoCommand : ICommand<ProdutoDTO>
    {
        public string NomeProduto { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal? Desconto { get; set; }
    }
}
