using AutoMapper;
using Microsoft.Extensions.Logging;
using ProjectTest.Application.Auth;
using ProjectTest.Application.DTO.Pedido;
using ProjectTest.Application.DTO.Tarefa;
using ProjectTest.Application.DTO.User;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;
using ProjectTest.Domain.Interfaces;
using ProjectTest.Domain.Interfaces.Common;
using ProjectTest.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICurrentUserInfo _currentUser;
        private readonly ILogger<TarefaService> _logger;

        public TarefaService(IUnitOfWork uow, IMapper mapper, ICurrentUserInfo currentUser, ILogger<TarefaService> logger)
        {
            this._uow = uow;
            this._mapper = mapper;
            this._currentUser = currentUser;
            this._logger = logger;
        }

        public async Task<List<TarefaDTO>> GetAsync()
        {
            try
            {
                _logger.LogInformation("Buscando todas as tarefas...");
                var _entities = await _uow.TarefaRepository.GetAllAsync();
                _logger.LogInformation($"Total de {_entities.Count} tarefas encontradas.");
                return _mapper.Map<List<TarefaDTO>>(_entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as tarefas");
                throw;
            }
        }

        public async Task<TarefaDTO> CreateAsync(TarefaParamDTO paramDTO)
        {
            try
            {
                _logger.LogInformation("Criando uma nova tarefa...");
                var _entity = _mapper.Map<Tarefa>(paramDTO);
                _entity.UsuarioId = Guid.Parse(_currentUser.UserId);
                _entity.Status = EStatusTarefa.Pendente;

                var entitySave = await _uow.TarefaRepository.AddAndSaveAsync(_entity);
                var objDTO = _mapper.Map<TarefaDTO>(entitySave);

                _logger.LogInformation($"Tarefa criada com sucesso. ID: {_entity.Id}");
                return objDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar a tarefa");
                throw;
            }
        }

        public async Task<TarefaDTO> GetById(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                {
                    _logger.LogWarning($"ID inválido: {id}");
                    throw new Exception("ID é inválido");
                }

                _logger.LogInformation($"Buscando a tarefa com ID: {id}");
                var _entity = await _uow.TarefaRepository.GetByGuidAsync(guidId);
                if (_entity == null)
                {
                    _logger.LogWarning($"Tarefa não encontrada para o ID: {id}");
                    throw new Exception("Tarefa não encontrada!");
                }

                return _mapper.Map<TarefaDTO>(_entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar a tarefa com ID: {id}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(TarefaModifyDTO paramDTO)
        {
            try
            {
                if (paramDTO.Id == Guid.Empty)
                {
                    _logger.LogWarning("ID da tarefa é inválido.");
                    throw new Exception("ID é inválido");
                }

                _logger.LogInformation($"Atualizando a tarefa com ID: {paramDTO.Id}");
                var _entity = await _uow.TarefaRepository.GetByGuidAsync(paramDTO.Id);
                if (_entity == null)
                {
                    _logger.LogWarning($"Tarefa não encontrada para o ID: {paramDTO.Id}");
                    throw new Exception("Tarefa não encontrada!");
                }

                _entity.UsuarioId = Guid.Parse(_currentUser.UserId);
                _entity = _mapper.Map<TarefaModifyDTO, Tarefa>(paramDTO, _entity);

                await _uow.TarefaRepository.UpdateAsync(_entity);
                _logger.LogInformation($"Tarefa com ID: {paramDTO.Id} atualizada com sucesso.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar a tarefa com ID: {paramDTO.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                {
                    _logger.LogWarning($"ID inválido para exclusão: {id}");
                    throw new Exception("ID é inválido");
                }

                _logger.LogInformation($"Excluindo a tarefa com ID: {id}");
                var _entity = await _uow.TarefaRepository.GetByGuidAsync(guidId);
                if (_entity == null)
                {
                    _logger.LogWarning($"Tarefa não encontrada para o ID: {id}");
                    throw new Exception("Tarefa não encontrada!");
                }

                await _uow.TarefaRepository.SoftDeleteAsync(guidId);
                _logger.LogInformation($"Tarefa com ID: {id} excluída com sucesso.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir a tarefa com ID: {id}");
                throw;
            }
        }
    }
}
