using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using ProjectTest.Application.Events;
using ProjectTest.Application.Services;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Common;
using ProjectTest.Domain.Interfaces;
using Xunit;
using Bogus;
using Microsoft.Extensions.Logging;
using FluentValidation;
using ProjectTest.Application.Interfaces;
using System.Linq.Expressions;

namespace ProjectTest.Tests.Application.Services
{
    public class VendaServiceTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<VendaService> _logger;
        private readonly IValidator<Venda> _validator;
        private readonly VendaService _vendaService;

        public VendaServiceTest()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _logger = Substitute.For<ILogger<VendaService>>();
            _validator = Substitute.For<IValidator<Venda>>();

            _vendaService = new VendaService(_unitOfWork, _logger, _eventPublisher, _validator);
        }

        #region AddAsync Tests

        [Fact]
        public async Task AddAsync_ShouldAddVendaSuccessfully()
        {
            // Arrange
            var venda = CreateFakeVenda();
            _unitOfWork.VendaRepository.GetByGuidAsync(venda.Id).Returns((Venda)null);
            _unitOfWork.VendaRepository.AddAsync(venda).Returns(venda);
            _validator.ValidateAsync(venda).Returns(new FluentValidation.Results.ValidationResult());

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
            var venda = CreateFakeVenda();
            _unitOfWork.VendaRepository.GetByGuidAsync(venda.Id).Returns(venda);

            // Act
            Func<Task> act = async () => await _vendaService.AddAsync(venda);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Venda já existe.");
        }

        [Fact]
        public async Task AddAsync_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var venda = CreateFakeVenda();
            _unitOfWork.VendaRepository.GetByGuidAsync(venda.Id).Returns((Venda)null);
            _validator.ValidateAsync(venda).Returns(new FluentValidation.Results.ValidationResult(
                new List<FluentValidation.Results.ValidationFailure> { new("Field", "Validation failed") }));

            // Act
            Func<Task> act = async () => await _vendaService.AddAsync(venda);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro de validação: Validation failed");
        }

        #endregion

        #region UpdateAsync Tests

        [Fact]
        public async Task UpdateAsync_ShouldUpdateVendaSuccessfully()
        {
            // Arrange
            var venda = CreateFakeVenda();
            _unitOfWork.VendaRepository.GetByGuidAsyncWithChildren(venda.Id, Arg.Any<Expression<Func<Venda, object>>>()).Returns(venda);
            _validator.ValidateAsync(venda).Returns(new FluentValidation.Results.ValidationResult());

            // Act
            var result = await _vendaService.UpdateAsync(venda);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(venda.Id);
            await _unitOfWork.Received(1).CommitAsync();
            _eventPublisher.Received(1).Publish(Arg.Any<CompraAlteradaEvent>());
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var venda = CreateFakeVenda();
            _validator.ValidateAsync(venda).Returns(new FluentValidation.Results.ValidationResult(
                new List<FluentValidation.Results.ValidationFailure> { new("Field", "Validation failed") }));

            // Act
            Func<Task> act = async () => await _vendaService.UpdateAsync(venda);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro de validação: Validation failed");
        }

        #endregion

        #region CancelarVendaAsync Tests

        [Fact]
        public async Task CancelarVendaAsync_ShouldCancelVendaSuccessfully()
        {
            // Arrange
            var venda = CreateFakeVenda();
            _unitOfWork.VendaRepository.GetByGuidAsync(venda.Id).Returns(venda);

            // Act
            await _vendaService.CancelarVendaAsync(venda.Id);

            // Assert
            venda.Cancelada.Should().BeTrue();
            await _unitOfWork.Received(1).CommitAsync();
            _eventPublisher.Received(1).Publish(Arg.Any<CompraCanceladaEvent>());
        }

        [Fact]
        public async Task CancelarVendaAsync_ShouldThrowException_WhenVendaNotFound()
        {
            // Arrange
            var vendaId = Guid.NewGuid();
            _unitOfWork.VendaRepository.GetByGuidAsync(vendaId).Returns((Venda)null);

            // Act
            Func<Task> act = async () => await _vendaService.CancelarVendaAsync(vendaId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Venda não encontrada.");
        }

        #endregion

        #region CancelarItemAsync Tests

        [Fact]
        public async Task CancelarItemAsync_ShouldCancelItemSuccessfully()
        {
            // Arrange
            var venda = CreateFakeVenda();
            var item = venda.Itens.First();
            _unitOfWork.VendaRepository.GetByGuidAsyncWithChildren(venda.Id, Arg.Any<Expression<Func<Venda, object>>>()).Returns(venda);
            _unitOfWork.ItemVendaRepository.GetByGuidAsync(item.Id).Returns(item);

            // Act
            await _vendaService.CancelarItemAsync(venda.Id, item.Id);

            // Assert
            item.Cancelada.Should().BeTrue();
            await _unitOfWork.Received(1).CommitAsync();
            _eventPublisher.Received(1).Publish(Arg.Any<ItemCanceladoEvent>());
        }

        [Fact]
        public async Task CancelarItemAsync_ShouldThrowException_WhenVendaNotFound()
        {
            // Arrange
            var vendaId = Guid.NewGuid();
            var itemId = Guid.NewGuid(); 

            _unitOfWork.VendaRepository.GetByGuidAsyncWithChildren(vendaId, Arg.Any<Expression<Func<Venda, object>>>()).Returns((Venda)null);


            // Act
            Func<Task> act = async () => await _vendaService.CancelarItemAsync(vendaId, itemId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Venda não encontrada.");
        }

        [Fact]
        public async Task CancelarItemAsync_ShouldThrowException_WhenItemNotFound()
        {
            // Arrange
            var venda = CreateFakeVenda();
            _unitOfWork.VendaRepository.GetByGuidAsyncWithChildren(venda.Id, Arg.Any<Expression<Func<Venda, object>>>()).Returns(venda);

            // Act
            Func<Task> act = async () => await _vendaService.CancelarItemAsync(venda.Id, Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Item não encontrado.");
        }

        #endregion

        #region GetAllAsync Tests

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllVendasSuccessfully()
        {
            // Arrange
            var vendas = new List<Venda> { CreateFakeVenda(), CreateFakeVenda() };
            _unitOfWork.VendaRepository.GetAllAsyncWithChildren(Arg.Any<Expression<Func<Venda, object>>>()).Returns(vendas);


            // Act
            var result = await _vendaService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        #endregion

        #region Helper Methods

        private Venda CreateFakeVenda()
        {
            return new Faker<Venda>()
                .RuleFor(v => v.Id, f => f.Random.Guid())
                .RuleFor(v => v.ClienteId, f => f.Random.Guid())
                .RuleFor(v => v.NomeCliente, f => f.Person.FullName)
                .RuleFor(v => v.Filial, f => f.Company.CompanyName())
                .RuleFor(v => v.Itens, f => new Faker<ItemVenda>()
                    .RuleFor(i => i.Id, f => f.Random.Guid())
                    .RuleFor(i => i.ProdutoId, f => f.Random.Guid())
                    .RuleFor(i => i.Quantidade, f => f.Random.Int(1, 10))
                    .RuleFor(i => i.ValorUnitario, f => f.Finance.Amount(10, 100))
                    .Generate(3))
                .Generate();
        }

        #endregion
    }
}
