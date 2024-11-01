using AutoMapper;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO.Produtos;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Application.Produtos.Commands.UpdateProduto
{
    public class UpdateProdutoCommandHandler : ICommandHandler<UpdateProdutoCommand, ProdutoDTO>
    {
        private readonly IProdutoService _service;
        private readonly IMapper _mapper;

        public UpdateProdutoCommandHandler(IProdutoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<ProdutoDTO> Handle(UpdateProdutoCommand request, CancellationToken cancellationToken)
        {
            var produtoEntity = _mapper.Map<Produto>(request);
            var entityAtualizado = await _service.UpdateAsync(produtoEntity);

            if (entityAtualizado == null)
            {
                throw new Exception("Erro ao atualizar a venda.");
            }
            return _mapper.Map<ProdutoDTO>(entityAtualizado);

        }
    }
}
