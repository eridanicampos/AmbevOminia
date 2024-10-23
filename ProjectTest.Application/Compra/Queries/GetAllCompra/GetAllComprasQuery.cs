using MediatR;
using ProjectTest.Application.DTO.Venda;

namespace ProjectTest.Application.Compra.Queries.GetAllCompra
{
    public class GetAllComprasQuery : IRequest<List<VendaDTO>>
    {
    }
}
