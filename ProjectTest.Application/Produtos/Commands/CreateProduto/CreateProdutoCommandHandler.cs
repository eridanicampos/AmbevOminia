using AutoMapper;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO.Produtos;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Application.Produtos.Commands.CreateProduto
{
    public class CreateProdutoCommandHandler : ICommandHandler<CreateProdutoCommand, ProdutoDTO>
    {
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public CreateProdutoCommandHandler(IProdutoService produtoService, IMapper mapper)
        {
            _produtoService = produtoService;
            _mapper = mapper;
        }

        public async Task<ProdutoDTO> Handle(CreateProdutoCommand request, CancellationToken cancellationToken)
        {
            var produtoEntity = _mapper.Map<Produto>(request);

            var produtoCriado = await _produtoService.AddAsync(produtoEntity);

            if (produtoCriado == null || produtoCriado.Id == Guid.Empty)
                throw new Exception("Erro ao criar o produto.");

            var result = _mapper.Map<ProdutoDTO>(produtoCriado);
            return result;
        }
    }
}
