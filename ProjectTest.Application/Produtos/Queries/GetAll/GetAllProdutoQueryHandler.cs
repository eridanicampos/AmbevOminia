using AutoMapper;
using MediatR;
using ProjectTest.Application.DTO.Produtos;
using ProjectTest.Application.Interfaces;

namespace ProjectTest.Application.Produtos.Queries.GetAll
{
    public class GetAllProdutoQueryHandler : IRequestHandler<GetAllProdutoQuery, List<ProdutoDTO>>
    {
        private readonly IProdutoService _service;
        private readonly IMapper _mapper;

        public GetAllProdutoQueryHandler(IProdutoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<List<ProdutoDTO>> Handle(GetAllProdutoQuery request, CancellationToken cancellationToken)
        {
            var vendas = await _service.GetAllAsync();
            return _mapper.Map<List<ProdutoDTO>>(vendas);
        }
    }
}
