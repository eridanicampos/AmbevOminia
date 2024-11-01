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
using ProjectTest.Application.DTO.Pedido;

namespace ProjectTest.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<ProdutoService> _logger;

        public ProdutoService(IUnitOfWork uow, ILogger<ProdutoService> logger)
        {
            this._uow = uow;
            this._logger = logger;
        }

        public async Task<Produto> AddAsync(Produto entity)
        {
            try
            {
                _logger.LogInformation("Adicionando novo Produto ao banco de dados.");

                var objExistente = await _uow.ProdutoRepository.ObterProdutoPorNomeAsync(entity.NomeProduto);
                if (objExistente != null)
                {
                    throw new Exception("Produto já existe.");
                }

                var (isValid, messages) = await entity.Validate();
                if (!isValid)
                {
                    throw new Exception("Erro de validação: " + string.Join(", ", messages));
                }
                await _uow.ProdutoRepository.AddAsync(entity);

                await _uow.CommitAsync();

                _logger.LogInformation("Produto adicionado com sucesso. ID do produto: {ProdutoId}", entity.Id);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar produto ao banco de dados.");
                throw;
            }
        }
        public async Task<Produto> UpdateAsync(Produto entity)
        {
            try
            {
                _logger.LogInformation("Atualizando produto no banco de dados. ID do produto: {ProdutoId}", entity.Id);

                var objExistente = await _uow.ProdutoRepository.GetByGuidAsync(entity.Id);
                if (objExistente == null)
                {
                    throw new Exception("Produto não encontrado.");
                }

                var objExistenteNome = await _uow.ProdutoRepository.ObterProdutoPorNomeAsync(entity.NomeProduto);
                if (objExistenteNome != null)
                {
                    throw new Exception("Produto já existe.");
                }

                var (isValid, messages) = await entity.Validate();
                if (!isValid)
                {
                    throw new Exception("Erro de validação: " + string.Join(", ", messages));
                }

                await _uow.ProdutoRepository.UpdateAsync(entity);
                await _uow.CommitAsync();


                _logger.LogInformation("Produto atualizado com sucesso. ID do produto: {ProdutoId}", entity.Id);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto no banco de dados.");
                throw;
            }
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deletando produto. ID do Produto: {ProdutoID}", id);

                var objExistente = await _uow.ProdutoRepository.GetByGuidAsync(id);
                if (objExistente == null)
                {
                    throw new Exception("Produto não encontrado.");
                }

                await _uow.ProdutoRepository.SoftDeleteAsync(id);
                await _uow.CommitAsync();

                _logger.LogInformation("Produto deletado com sucesso. ID da Produto: {ProdutoId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no produto. ID Produto: {ProdutoId}", id);
                throw;
            }
        }

        public async Task<List<Produto>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Obtendo todos produtos do banco de dados.");
                var list = await _uow.ProdutoRepository.GetAllAsync();
                _logger.LogInformation("Todos os produtos foram obtidas com sucesso.");
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos produtos.");
                throw;
            }
        }
        public async Task<Produto> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation("Obtendo todos produtos do banco de dados.");
                var entity = await _uow.ProdutoRepository.GetByGuidAsync(id);
                _logger.LogInformation("Todos os produtos foram obtidas com sucesso.");
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter produto.");
                throw;
            }
        }

    }
}
