using AutoMapper;
using ProjectTest.Application.Auth;
using ProjectTest.Application.DTO.Pedido;
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
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper mapper;
        private readonly ICurrentUserInfo _currentUser;

        public PedidoService(IUnitOfWork uow, IMapper mapper, ICurrentUserInfo currentUser)
        {
            this._uow = uow;
            this.mapper = mapper;
            this._currentUser = currentUser;
        }

        public async Task<List<PedidoDTO>> GetAsync()
        {
            var _entities = await _uow.PedidoRepository.GetAllAsync();
            return mapper.Map<List<PedidoDTO>>(_entities);
        }

        public async Task<PedidoDTO> CreateAsync(PedidoParamDTO paramersDTO)
        {
            var _entity = mapper.Map<Pedido>(paramersDTO);
            _entity.NumeroPedido = GerarNumeroPedido();
            _entity.DataHoraEntrega = DateTime.Now.AddDays(10);
            _entity.UsuarioId = Guid.Parse(_currentUser.UserId);
            _entity.StatusEntrega = EStatusEntrega.EmProcessamento;
            var entitySave = await _uow.PedidoRepository.AddAndSaveAsync(_entity);
            var objDTO = mapper.Map<PedidoDTO>(entitySave);

            return objDTO;
        }

        public async Task<PedidoDTO> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                throw new Exception("ID é invalido");

            var _entity = await _uow.PedidoRepository.GetByGuidAsync(guidId);
            if (_entity == null)
                throw new Exception("Não achou!");

            return mapper.Map<PedidoDTO>(_entity);
        }

        public async Task<bool> UpdateAsync(PedidoModifyDTO paramersDTO)
        {
            if (paramersDTO.Id == Guid.Empty)
                throw new Exception("ID é invalido");

            var _entity = await _uow.PedidoRepository.GetByGuidAsync(paramersDTO.Id);
            if (_entity == null)
                throw new Exception("Não achou!");

            _entity.UsuarioId = Guid.Parse(_currentUser.UserId);
            _entity = mapper.Map<PedidoModifyDTO, Pedido>(paramersDTO, _entity);

            await _uow.PedidoRepository.UpdateAsync(_entity);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                throw new Exception("UserID is not valid");

            var _entity = await _uow.PedidoRepository.GetByGuidAsync(guidId);
            if (_entity == null)
                throw new Exception("Não achou!");

            await _uow.PedidoRepository.SoftDeleteAsync(guidId);
            return true;
        }

        private static string GerarNumeroPedido()
        {
            string dataHoje = DateTime.Now.ToString("yyyyMMdd");
            Random random = new Random();
            int numeroAleatorio = random.Next(100, 1000); 

            return $"{dataHoje}{numeroAleatorio}";
        }

    }
}
