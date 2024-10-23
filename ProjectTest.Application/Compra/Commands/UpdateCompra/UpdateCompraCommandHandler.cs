using AutoMapper;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO.Venda;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Compra.Commands.UpdateCompra
{
    public class UpdateCompraCommandHandler : ICommandHandler<UpdateCompraCommand, VendaDTO>
    {
        private readonly IVendaService _vendaService;
        private readonly IMapper _mapper;

        public UpdateCompraCommandHandler(IVendaService vendaService, IMapper mapper)
        {
            _vendaService = vendaService;
            _mapper = mapper;
        }

        public async Task<VendaDTO> Handle(UpdateCompraCommand request, CancellationToken cancellationToken)
        {
            var vendaEntity = _mapper.Map<Venda>(request);
            var vendaAtualizada = await _vendaService.UpdateAsync(vendaEntity);

            if (vendaAtualizada == null)
            {
                throw new Exception("Erro ao atualizar a venda.");
            }
            return _mapper.Map<VendaDTO>(vendaAtualizada);

        }
    }
}
