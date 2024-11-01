using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using ProjectTest.Application.Compra.Commands.CreateCompra;
using ProjectTest.Application.DTO.ItemVenda;
using ProjectTest.Application.DTO.Venda;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Service;

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
            var command = CreateFakeCreateCompraCommand();
            var vendaEntity = CreateFakeVendaFromCommand(command);
            var vendaDTO = CreateFakeVendaDTO(vendaEntity);

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
            var command = CreateFakeCreateCompraCommand();
            var vendaEntity = CreateFakeVendaFromCommand(command);
            vendaEntity.Id = Guid.Empty;

            _mapper.Map<Venda>(command).Returns(vendaEntity);
            _vendaService.AddAsync(vendaEntity).Returns(vendaEntity);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro ao criar a venda.");
        }

        #region Helper Methods

        private CreateCompraCommand CreateFakeCreateCompraCommand()
        {
            return new Faker<CreateCompraCommand>()
                .RuleFor(c => c.ClienteId, f => f.Random.Guid())
                .RuleFor(c => c.NomeCliente, f => f.Person.FullName)
                .RuleFor(c => c.Filial, f => f.Company.CompanyName())
                .RuleFor(c => c.Itens, f => new Faker<CreateItemVendaDTO>()
                    .RuleFor(i => i.ProdutoId, f => f.Random.Guid())
                    .RuleFor(i => i.Quantidade, f => f.Random.Int(1, 10))
                    .Generate(3))
                .Generate();
        }

        private Venda CreateFakeVendaFromCommand(CreateCompraCommand command)
        {
            return new Venda
            {
                Id = Guid.NewGuid(),
                ClienteId = command.ClienteId,
                NomeCliente = command.NomeCliente,
                Filial = command.Filial,
                Itens = command.Itens.Select(item => new ItemVenda
                {
                    Id = Guid.NewGuid(),
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    ValorUnitario = 100 
                }).ToList()
            };
        }

        private VendaDTO CreateFakeVendaDTO(Venda venda)
        {
            return new VendaDTO
            {
                Id = venda.Id,
                ClienteId = venda.ClienteId,
                NomeCliente = venda.NomeCliente,
                Filial = venda.Filial,
                Itens = venda.Itens.Select(item => new ItemVendaDTO
                {
                    Id = item.Id,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    ValorTotalItem = item.ValorTotalItem
                }).ToList()
            };
        }

        #endregion
    }
}
