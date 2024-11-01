using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using ProjectTest.Application.Compra.Commands.UpdateCompra;
using ProjectTest.Application.DTO.ItemVenda;
using ProjectTest.Application.DTO.Venda;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Service;
using Xunit;

namespace ProjectTest.Test.Application.Compra.Commands.UpdateCompra
{
    public class UpdateCompraCommandHandlerTest
    {
        private readonly IVendaService _vendaService;
        private readonly IMapper _mapper;
        private readonly UpdateCompraCommandHandler _handler;

        public UpdateCompraCommandHandlerTest()
        {
            _vendaService = Substitute.For<IVendaService>();
            _mapper = Substitute.For<IMapper>();
            _handler = new UpdateCompraCommandHandler(_vendaService, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldUpdateVendaSuccessfully()
        {
            // Arrange
            var updateCompraCommand = CreateFakeUpdateCompraCommand();
            var vendaEntity = CreateFakeVendaFromCommand(updateCompraCommand);
            var vendaAtualizada = CreateFakeVendaFromCommand(updateCompraCommand);
            var vendaDto = CreateFakeVendaDTO(vendaAtualizada);

            _vendaService.GetByIdAsync(updateCompraCommand.VendaId).Returns(vendaEntity);
            _vendaService.UpdateAsync(vendaEntity).Returns(vendaAtualizada);
            _mapper.Map(updateCompraCommand, vendaEntity).Returns(vendaEntity);
            _mapper.Map<VendaDTO>(vendaAtualizada).Returns(vendaDto);

            // Act
            var result = await _handler.Handle(updateCompraCommand, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(vendaDto.Id);
            result.ClienteId.Should().Be(vendaDto.ClienteId);
            result.NomeCliente.Should().Be(vendaDto.NomeCliente);
            result.Filial.Should().Be(vendaDto.Filial);
            await _vendaService.Received(1).UpdateAsync(vendaEntity);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenVendaNotFound()
        {
            // Arrange
            var updateCompraCommand = CreateFakeUpdateCompraCommand();
            _vendaService.GetByIdAsync(updateCompraCommand.VendaId).Returns((Venda)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(updateCompraCommand, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro ao atualizar a venda.");
        }

        #region Helper Methods

        private UpdateCompraCommand CreateFakeUpdateCompraCommand()
        {
            return new Faker<UpdateCompraCommand>()
                .RuleFor(v => v.VendaId, f => f.Random.Guid())
                .RuleFor(v => v.ClienteId, f => f.Random.Guid())
                .RuleFor(v => v.NomeCliente, f => f.Person.FullName)
                .RuleFor(v => v.Filial, f => f.Company.CompanyName())
                .RuleFor(v => v.Itens, f => new Faker<CreateItemVendaDTO>()
                    .RuleFor(i => i.ProdutoId, f => f.Random.Guid())
                    .RuleFor(i => i.Quantidade, f => f.Random.Int(1, 10))
                    .Generate(3))
                .Generate();
        }

        private Venda CreateFakeVendaFromCommand(UpdateCompraCommand command)
        {
            return new Venda
            {
                Id = command.VendaId,
                ClienteId = command.ClienteId,
                NomeCliente = command.NomeCliente,
                Filial = command.Filial,
                Itens = command.Itens.Select(item => new ItemVenda
                {
                    Id = Guid.NewGuid(),
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    ValorUnitario = 100 // Valor arbitrário para o teste
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
