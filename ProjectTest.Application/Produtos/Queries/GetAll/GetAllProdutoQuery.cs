using MediatR;
using ProjectTest.Application.DTO.Produtos;
using ProjectTest.Application.DTO.Venda;

namespace ProjectTest.Application.Produtos.Queries.GetAll
{
    public class GetAllProdutoQuery : IRequest<List<ProdutoDTO>>
    {
    }
}
