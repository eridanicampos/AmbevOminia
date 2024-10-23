using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NSubstitute;
using ProjectTest.Application.Events;
using ProjectTest.Application.Services;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces;
using ProjectTest.Domain.Interfaces.Common;
using ProjectTest.Domain.Interfaces.Service;
using Xunit;
using Bogus;
using Microsoft.Extensions.Logging;
using ProjectTest.Application.Interfaces;
using System.Linq.Expressions;

namespace ProjectTest.Test.Application.Services
{
    public class VendaServiceTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;
        private readonly ILogger<VendaService> _logger;
        private readonly VendaService _vendaService;

        public VendaServiceTest()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger<VendaService>>();
            _vendaService = new VendaService(_unitOfWork, _logger, _eventPublisher);
        }

        [Fact]
        public async Task AddAsync_ShouldAddVendaSuccessfully()
        {
            // Arrange
            var venda = new Faker<Venda>()
                .RuleFor(v => v.Id, f => f.Random.Guid())
                .RuleFor(v => v.ClienteId, f => f.Random.Guid())
                .RuleFor(v => v.NomeCliente, f => f.Person.FullName)
                .RuleFor(v => v.Filial, f => f.Company.CompanyName())
                .RuleFor(v => v.Itens, f => new Faker<ItemVenda>()
                    .RuleFor(i => i.Id, f => f.Random.Guid())
                    .RuleFor(i => i.ProdutoId, f => f.Random.Guid())
                    .RuleFor(i => i.Quantidade, f => f.Random.Int(1, 100))
                    .RuleFor(i => i.ValorUnitario, f => f.Finance.Amount())
                    .RuleFor(i => i.Desconto, f => f.Finance.Amount(0, 10))
                    .Generate(3))
                .Generate();

            _unitOfWork.VendaRepository.GetByGuidAsync(venda.Id).Returns((Venda)null);
            _unitOfWork.VendaRepository.AddAsync(venda).Returns(venda);

            // Act
            var result = await _vendaService.AddAsync(venda);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(venda.Id);
            await _unitOfWork.Received(1).CommitAsync();
            _eventPublisher.Received(1).Publish(Arg.Any<CompraCriadaEvent>());
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenVendaAlreadyExists()
        {
            // Arrange
            var venda = new Faker<Venda>()
                .RuleFor(v => v.Id, f => f.Random.Guid())
                .Generate();

            _unitOfWork.VendaRepository.GetByGuidAsync(venda.Id).Returns(venda);

            // Act
            Func<Task> act = async () => await _vendaService.AddAsync(venda);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Venda já existe.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateVendaSuccessfully()
        {
            // Arrange
            var vendaExistente = new Faker<Venda>()
                .RuleFor(v => v.Id, f => f.Random.Guid())
                .RuleFor(v => v.ClienteId, f => f.Random.Guid())
                .RuleFor(v => v.NomeCliente, f => f.Person.FullName)
                .RuleFor(v => v.Filial, f => f.Company.CompanyName())
                .RuleFor(v => v.Itens, f => new Faker<ItemVenda>()
                    .RuleFor(i => i.Id, f => f.Random.Guid())
                    .RuleFor(i => i.ProdutoId, f => f.Random.Guid())
                    .RuleFor(i => i.Quantidade, f => f.Random.Int(1, 100))
                    .RuleFor(i => i.ValorUnitario, f => f.Finance.Amount())
                    .RuleFor(i => i.Desconto, f => f.Finance.Amount(0, 10))
                    .Generate(3))
                .Generate();

            var vendaAtualizada = new Faker<Venda>()
                .RuleFor(v => v.Id, f => vendaExistente.Id)
                .RuleFor(v => v.ClienteId, f => f.Random.Guid())
                .RuleFor(v => v.NomeCliente, f => f.Person.FullName)
                .RuleFor(v => v.Filial, f => f.Company.CompanyName())
                .RuleFor(v => v.Itens, f => new Faker<ItemVenda>()
                    .RuleFor(i => i.Id, f => f.Random.Guid())
                    .RuleFor(i => i.ProdutoId, f => f.Random.Guid())
                    .RuleFor(i => i.Quantidade, f => f.Random.Int(1, 100))
                    .RuleFor(i => i.ValorUnitario, f => f.Finance.Amount())
                    .RuleFor(i => i.Desconto, f => f.Finance.Amount(0, 10))
                    .Generate(3))
                .Generate();

            _unitOfWork.VendaRepository
    .GetByGuidAsyncWithChildren(Arg.Is(vendaExistente.Id), Arg.Any<Expression<Func<Venda, object>>>())
    .Returns(vendaExistente);
            // Act
            var result = await _vendaService.UpdateAsync(vendaAtualizada);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(vendaExistente.Id);
            result.ClienteId.Should().Be(vendaAtualizada.ClienteId);
            result.NomeCliente.Should().Be(vendaAtualizada.NomeCliente);
            await _unitOfWork.Received(1).CommitAsync();
            _eventPublisher.Received(1).Publish(Arg.Any<CompraAlteradaEvent>());
        }
    }
}
