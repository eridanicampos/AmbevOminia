using AutoMapper;
using MediatR;
using ProjectTest.Application.DTO.Produtos;
using ProjectTest.Application.DTO.Venda;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Interfaces.Service;

namespace ProjectTest.Application.Produtos.Queries.GetId
{
    public class GetIdProdutoQueryHandler : IRequestHandler<GetIdProdutoQuery, ProdutoDTO>
    {
        private readonly IProdutoService _service;
        private readonly IMapper _mapper;

        public GetIdProdutoQueryHandler(IProdutoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<ProdutoDTO> Handle(GetIdProdutoQuery request, CancellationToken cancellationToken)
        {
            var result = await _service.GetById(request.Id);
            return _mapper.Map<ProdutoDTO>(result);
        }
    }
}
