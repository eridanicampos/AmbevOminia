using ProjectTest.Application.DTO.Venda;
using ProjectTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Domain.Interfaces.Service;
using AutoMapper;

namespace ProjectTest.Application.Compra.Commands.CreateCompra
{
    public class CreateCompraCommandHandler : ICommandHandler<CreateCompraCommand, VendaDTO>
    {
        private readonly IVendaService _vendaService;
        private readonly IMapper _mapper;

        public CreateCompraCommandHandler(IVendaService vendaService, IMapper mapper)
        {
            _vendaService = vendaService;
            _mapper = mapper;

        }

        public async Task<VendaDTO> Handle(CreateCompraCommand request, CancellationToken cancellationToken)
        {
            var vendaEntity = _mapper.Map<Venda>(request);

            var vendaCriada = await _vendaService.AddAsync(vendaEntity);

            if (vendaCriada == null || vendaCriada.Id == Guid.Empty)
                throw new Exception("Erro ao criar a venda.");

            var result = _mapper.Map<VendaDTO>(vendaCriada);
            return result;
        }
    }
}
