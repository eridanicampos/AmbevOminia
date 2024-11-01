using AutoMapper;
using Microsoft.Extensions.Logging;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Interfaces.Common;
using ProjectTest.Domain.Interfaces;
using ProjectTest.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTest.Domain.Entities;
using ProjectTest.Application.Events;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace ProjectTest.Application.Services
{
    public class VendaService : IVendaService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<VendaService> _logger;
        private readonly IEventPublisher _eventPublisher;
        private readonly IValidator<Venda> _validator;

        public VendaService(IUnitOfWork uow, ILogger<VendaService> logger, IEventPublisher eventPublisher, IValidator<Venda> validator)
        {
            this._uow = uow;
            this._logger = logger;
            _eventPublisher = eventPublisher;
            _validator = validator;

        }

        public async Task<Venda> AddAsync(Venda vendaEntity)
        {
            try
            {
                _logger.LogInformation("Adicionando nova venda ao banco de dados.");

                var vendaExistente = await _uow.VendaRepository.GetByGuidAsync(vendaEntity.Id);
                if (vendaExistente != null)
                {
                    throw new Exception("Venda já existe.");
                }

                vendaEntity.NumeroVenda = GerarNumeroVenda();
                vendaEntity.DataVenda = DateTime.Now;
                await GerarDesconto(vendaEntity);

                var validationResult = await _validator.ValidateAsync(vendaEntity);
                if (!validationResult.IsValid)
                {
                    throw new Exception("Erro de validação: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                }


                var venda = await _uow.VendaRepository.AddAsync(vendaEntity);

                await _uow.CommitAsync();

                _eventPublisher.Publish(new CompraCriadaEvent(venda.Id));

                _logger.LogInformation("Venda adicionada com sucesso. ID da venda: {VendaId}", vendaEntity.Id);

                return vendaEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar a venda ao banco de dados.");
                throw;
            }
        }
        public async Task<Venda> UpdateAsync(Venda vendaEntity)
        {
            try
            {
                _logger.LogInformation("Atualizando venda no banco de dados. ID da venda: {VendaId}", vendaEntity.Id);

                await GerarDesconto(vendaEntity);

                var validationResult = await _validator.ValidateAsync(vendaEntity);
                if (!validationResult.IsValid)
                {
                    throw new Exception("Erro de validação: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                }

                await _uow.VendaRepository.UpdateAsync(vendaEntity);
                await _uow.CommitAsync();

                _eventPublisher.Publish(new CompraAlteradaEvent(vendaEntity.Id));

                _logger.LogInformation("Venda atualizada com sucesso. ID da venda: {VendaId}", vendaEntity.Id);

                return vendaEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar a venda no banco de dados.");
                throw;
            }
        }
        public async Task CancelarVendaAsync(Guid vendaId)
        {
            try
            {
                _logger.LogInformation("Cancelando venda. ID da venda: {VendaId}", vendaId);

                var vendaExistente = await _uow.VendaRepository.GetByGuidAsync(vendaId);
                if (vendaExistente == null)
                {
                    throw new Exception("Venda não encontrada.");
                }

                vendaExistente.Cancelada = true;
                await _uow.VendaRepository.UpdateAsync(vendaExistente);
                await _uow.CommitAsync();

                _eventPublisher.Publish(new CompraCanceladaEvent(vendaExistente.Id));

                _logger.LogInformation("Venda cancelada com sucesso. ID da venda: {VendaId}", vendaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cancelar a venda. ID da venda: {VendaId}", vendaId);
                throw;
            }
        }

        public async Task CancelarItemAsync(Guid vendaId, Guid itemId)
        {
            try
            {
                _logger.LogInformation("Cancelando item da venda. ID da venda: {VendaId}, ID do item: {ItemId}", vendaId, itemId);

                var vendaExistente = await _uow.VendaRepository.GetByGuidAsyncWithChildren(vendaId, v => v.Itens);
                if (vendaExistente == null)
                {
                    throw new Exception("Venda não encontrada.");
                }

                var itemExistente = vendaExistente.Itens.FirstOrDefault(i => i.Id == itemId);
                if (itemExistente == null)
                {
                    throw new Exception("Item não encontrado.");
                }
                var itemEntity = await _uow.ItemVendaRepository.GetByGuidAsync(itemExistente.Id);

                itemEntity.Cancelada = true;
                await _uow.ItemVendaRepository.UpdateAsync(itemEntity);
                await _uow.CommitAsync();

                _eventPublisher.Publish(new ItemCanceladoEvent(vendaId, itemId));

                _logger.LogInformation("Item cancelado com sucesso. ID da venda: {VendaId}, ID do item: {ItemId}", vendaId, itemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cancelar o item da venda. ID da venda: {VendaId}, ID do item: {ItemId}", vendaId, itemId);
                throw;
            }
        }
        

        public async Task<List<Venda>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Obtendo todas as vendas do banco de dados.");
                var vendas = await _uow.VendaRepository.GetAllAsyncWithChildren(v => v.Itens);
                _logger.LogInformation("Todas as vendas foram obtidas com sucesso.");
                return vendas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as vendas.");
                throw;
            }
        }

        public Task<Venda> GetByIdAsync(Guid id)
        {
            var result = _uow.VendaRepository.GetByGuidAsyncWithChildren(id, v => v.Itens);

            if (result == null)
            {
                _logger.LogError("Erro ao obter a venda - {id}.", id);

                throw new Exception("Essa venda não existe");
            }

            _logger.LogInformation("Venda achada com sucesso. ID da venda: {VendaId}", id);

            return result;
        }


        private long GerarNumeroVenda()
        {
            return DateTime.Now.Ticks;
        }

        public async Task GerarDesconto(Venda vendaEntity)
        {
            var itensAgrupados = vendaEntity.Itens
                .Where(i => !i.Cancelada)
                .GroupBy(i => i.ProdutoId);

            foreach (var grupo in itensAgrupados)
            {
                var quantidadeTotal = grupo.Sum(i => i.Quantidade);

                if (quantidadeTotal > 20)
                {
                    throw new InvalidOperationException("Não é permitido vender acima de 20 itens iguais.");
                }

                decimal desconto = 0;
                if (quantidadeTotal >= 4 && quantidadeTotal < 10)
                {
                    desconto = 0.10m; 
                }
                else if (quantidadeTotal >= 10 && quantidadeTotal <= 20)
                {
                    desconto = 0.20m; 
                }

                foreach (var item in grupo)
                {
                    item.ValorUnitario -= item.ValorUnitario * desconto;
                    item.DescontoValorUnitario = desconto;
                }
            }
        }

    }
}
