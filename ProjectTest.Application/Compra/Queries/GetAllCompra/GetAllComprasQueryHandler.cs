using AutoMapper;
using MediatR;
using ProjectTest.Application.DTO.Venda;
using ProjectTest.Domain.Interfaces.Service;

namespace ProjectTest.Application.Compra.Queries.GetAllCompra
{
    public class GetAllComprasQueryHandler : IRequestHandler<GetAllComprasQuery, List<VendaDTO>>
    {
        private readonly IVendaService _vendaService;
        private readonly IMapper _mapper;

        public GetAllComprasQueryHandler(IVendaService vendaService, IMapper mapper)
        {
            _vendaService = vendaService;
            _mapper = mapper;
        }

        public async Task<List<VendaDTO>> Handle(GetAllComprasQuery request, CancellationToken cancellationToken)
        {
            var vendas = await _vendaService.GetAllAsync();
            return _mapper.Map<List<VendaDTO>>(vendas);
        }
    }
}
