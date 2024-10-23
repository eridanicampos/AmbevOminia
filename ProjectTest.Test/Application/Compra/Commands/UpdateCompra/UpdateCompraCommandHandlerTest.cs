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
            var updateCompraCommand = new Faker<UpdateCompraCommand>()
                .RuleFor(v => v.VendaId, f => f.Random.Guid())
                .RuleFor(v => v.ClienteId, f => f.Random.Guid())
                .RuleFor(v => v.NomeCliente, f => f.Person.FullName)
                .RuleFor(v => v.Filial, f => f.Company.CompanyName())
                .RuleFor(v => v.Itens, f => new Faker<CreateItemVendaDTO>()
                    .RuleFor(i => i.ProdutoId, f => f.Random.Guid())
                    .RuleFor(i => i.Quantidade, f => f.Random.Int(1, 100))
                    .RuleFor(i => i.ValorUnitario, f => f.Finance.Amount())
                    .RuleFor(i => i.Desconto, f => f.Finance.Amount(0, 10))
                    .Generate(3))
                .Generate();

            var vendaEntity = new Faker<Venda>()
                .RuleFor(v => v.Id, f => updateCompraCommand.VendaId)
                .RuleFor(v => v.ClienteId, f => updateCompraCommand.ClienteId)
                .RuleFor(v => v.NomeCliente, f => updateCompraCommand.NomeCliente)
                .RuleFor(v => v.Filial, f => updateCompraCommand.Filial)
                .RuleFor(v => v.Itens, f => new Faker<ItemVenda>()
                    .RuleFor(i => i.Id, f => f.Random.Guid())
                    .RuleFor(i => i.ProdutoId, f => f.Random.Guid())
                    .RuleFor(i => i.Quantidade, f => f.Random.Int(1, 100))
                    .RuleFor(i => i.ValorUnitario, f => f.Finance.Amount())
                    .RuleFor(i => i.Desconto, f => f.Finance.Amount(0, 10))
                    .Generate(3))
                .Generate();

            var vendaAtualizada = new Faker<Venda>()
                .RuleFor(v => v.Id, f => updateCompraCommand.VendaId)
                .RuleFor(v => v.ClienteId, f => updateCompraCommand.ClienteId)
                .RuleFor(v => v.NomeCliente, f => updateCompraCommand.NomeCliente)
                .RuleFor(v => v.Filial, f => updateCompraCommand.Filial)
                .RuleFor(v => v.Itens, f => new Faker<ItemVenda>()
                    .RuleFor(i => i.Id, f => f.Random.Guid())
                    .RuleFor(i => i.ProdutoId, f => f.Random.Guid())
                    .RuleFor(i => i.Quantidade, f => f.Random.Int(1, 100))
                    .RuleFor(i => i.ValorUnitario, f => f.Finance.Amount())
                    .RuleFor(i => i.Desconto, f => f.Finance.Amount(0, 10))
                    .Generate(3))
                .Generate();

            var vendaDto = new Faker<VendaDTO>()
                .RuleFor(v => v.Id, f => updateCompraCommand.VendaId)
                .RuleFor(v => v.ClienteId, f => updateCompraCommand.ClienteId)
                .RuleFor(v => v.NomeCliente, f => updateCompraCommand.NomeCliente)
                .RuleFor(v => v.Filial, f => updateCompraCommand.Filial)
                .Generate();

            _mapper.Map<Venda>(updateCompraCommand).Returns(vendaEntity);
            _vendaService.UpdateAsync(vendaEntity).Returns(vendaAtualizada);
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
            var updateCompraCommand = new Faker<UpdateCompraCommand>()
                .RuleFor(v => v.VendaId, f => f.Random.Guid())
                .Generate();

            var vendaEntity = new Faker<Venda>()
                .RuleFor(v => v.Id, f => updateCompraCommand.VendaId)
                .Generate();

            _mapper.Map<Venda>(updateCompraCommand).Returns(vendaEntity);
            _vendaService.UpdateAsync(vendaEntity).Returns((Venda)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(updateCompraCommand, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro ao atualizar a venda.");
        }
    }
}
