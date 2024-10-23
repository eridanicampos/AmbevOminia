using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using ProjectTest.Application.Compra.Commands.CreateCompra;
using ProjectTest.Application.DTO.ItemVenda;
using ProjectTest.Application.DTO.Venda;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Test.Application.Compra.Commands.CreateCompra
{
    public class CreateCompraCommandHandlerTest
    {
        private readonly IVendaService _vendaService;
        private readonly IMapper _mapper;
        private readonly CreateCompraCommandHandler _handler;

        public CreateCompraCommandHandlerTest()
        {
            _vendaService = Substitute.For<IVendaService>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateCompraCommandHandler(_vendaService, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldCreateVendaSuccessfully()
        {
            // Arrange
            var command = new Faker<CreateCompraCommand>()
                .RuleFor(c => c.ClienteId, f => f.Random.Guid())
                .RuleFor(c => c.NomeCliente, f => f.Person.FullName)
                .RuleFor(c => c.Filial, f => f.Company.CompanyName())
                .RuleFor(c => c.Itens, f => new Faker<CreateItemVendaDTO>()
                    .RuleFor(i => i.ProdutoId, f => f.Random.Guid())
                    .RuleFor(i => i.Quantidade, f => f.Random.Int(1, 100))
                    .RuleFor(i => i.ValorUnitario, f => f.Finance.Amount())
                    .RuleFor(i => i.Desconto, f => f.Finance.Amount(0, 10))
                    .Generate(3))
                .Generate();

            var vendaEntity = new Venda
            {
                Id = Guid.NewGuid(),
                ClienteId = command.ClienteId,
                NomeCliente = command.NomeCliente,
                Filial = command.Filial,
                Itens = new List<ItemVenda>()
            };

            var vendaDTO = new VendaDTO
            {
                Id = vendaEntity.Id,
                ClienteId = vendaEntity.ClienteId,
                NomeCliente = vendaEntity.NomeCliente,
                Filial = vendaEntity.Filial
            };

            _mapper.Map<Venda>(command).Returns(vendaEntity);
            _vendaService.AddAsync(vendaEntity).Returns(vendaEntity);
            _mapper.Map<VendaDTO>(vendaEntity).Returns(vendaDTO);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(vendaDTO.Id);
            await _vendaService.Received(1).AddAsync(Arg.Any<Venda>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenVendaCreationFails()
        {
            // Arrange
            var command = new Faker<CreateCompraCommand>()
                .RuleFor(c => c.ClienteId, f => f.Random.Guid())
                .RuleFor(c => c.NomeCliente, f => f.Person.FullName)
                .RuleFor(c => c.Filial, f => f.Company.CompanyName())
                .RuleFor(c => c.Itens, f => new Faker<CreateItemVendaDTO>()
                    .RuleFor(i => i.ProdutoId, f => f.Random.Guid())
                    .RuleFor(i => i.Quantidade, f => f.Random.Int(1, 100))
                    .RuleFor(i => i.ValorUnitario, f => f.Finance.Amount())
                    .RuleFor(i => i.Desconto, f => f.Finance.Amount(0, 10))
                    .Generate(3))
                .Generate();

            var vendaEntity = new Venda
            {
                Id = Guid.Empty, // Simulando erro de criação
                ClienteId = command.ClienteId,
                NomeCliente = command.NomeCliente,
                Filial = command.Filial,
                Itens = new List<ItemVenda>()
            };

            _mapper.Map<Venda>(command).Returns(vendaEntity);
            _vendaService.AddAsync(vendaEntity).Returns(vendaEntity);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro ao criar a venda.");
        }
    }
}
