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

namespace ProjectTest.Application.Services
{
    public class VendaService : IVendaService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<VendaService> _logger;
        private readonly IEventPublisher _eventPublisher;

        public VendaService(IUnitOfWork uow, ILogger<VendaService> logger, IEventPublisher eventPublisher)
        {
            this._uow = uow;
            this._logger = logger;
            _eventPublisher = eventPublisher;
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
                vendaEntity.Cancelada = false;

                var (isValid, messages) = await vendaEntity.Validate();
                if (!isValid)
                {
                    throw new Exception("Erro de validação: " + string.Join(", ", messages));
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

                var vendaExistente = await _uow.VendaRepository.GetByGuidAsyncWithChildren(vendaEntity.Id, v => v.Itens);
                if (vendaExistente == null)
                {
                    throw new Exception("Venda não encontrada.");
                }

                vendaExistente.ClienteId = vendaEntity.ClienteId;
                vendaExistente.NomeCliente = vendaEntity.NomeCliente;
                vendaExistente.Filial = vendaEntity.Filial;

                AtualizarItensVenda(vendaExistente, vendaEntity);

                var (isValid, messages) = await vendaExistente.Validate();
                if (!isValid)
                {
                    throw new Exception("Erro de validação: " + string.Join(", ", messages));
                }

                await _uow.VendaRepository.UpdateAsync(vendaExistente);
                await _uow.CommitAsync();

                _eventPublisher.Publish(new CompraAlteradaEvent(vendaExistente.Id));

                _logger.LogInformation("Venda atualizada com sucesso. ID da venda: {VendaId}", vendaExistente.Id);

                return vendaExistente;
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

        private void AtualizarItensVenda(Venda vendaExistente, Venda vendaAtualizada)
        {
            var itensARemover = vendaExistente.Itens.Where(i => !vendaAtualizada.Itens.Any(vi => vi.Id == i.Id)).ToList();
            foreach (var item in itensARemover)
            {
                vendaExistente.Itens.Remove(item);
            }

            foreach (var item in vendaAtualizada.Itens)
            {
                var itemExistente = vendaExistente.Itens.FirstOrDefault(i => i.Id == item.Id);
                if (itemExistente != null)
                {
                    itemExistente.ProdutoId = item.ProdutoId;
                    itemExistente.Quantidade = item.Quantidade;
                    itemExistente.ValorUnitario = item.ValorUnitario;
                    itemExistente.Desconto = item.Desconto;
                }
                else
                {
                    vendaExistente.Itens.Add(item);
                }
            }
        }

        private long GerarNumeroVenda()
        {
            return DateTime.Now.Ticks;
        }

    }
}
