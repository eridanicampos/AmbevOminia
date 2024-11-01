using MediatR;
using ProjectTest.Application.DTO.Produtos;
using ProjectTest.Application.DTO.Venda;

namespace ProjectTest.Application.Produtos.Queries.GetId
{
    public class GetIdProdutoQuery : IRequest<ProdutoDTO>
    {
        public Guid Id { get; set; }

    }
}
