using AutoMapper;
using ProjectTest.Application.DTO.EnderecoEntrega;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces;
using ProjectTest.Domain.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProjectTest.Application.Services
{
    public class EnderecoEntregaService : IEnderecoEntregaService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICurrentUserInfo _currentUser;
        private readonly HttpClient _httpClient;

        public EnderecoEntregaService(IUnitOfWork uow, IMapper mapper, ICurrentUserInfo currentUser, HttpClient httpClient)
        {
            _uow = uow;
            _mapper = mapper;
            _currentUser = currentUser;
            _httpClient = httpClient;
        }

        public async Task<List<EnderecoEntregaDTO>> GetAsync()
        {
            IEnumerable<EnderecoEntrega> _entities = await _uow.EnderecoEntregaRepository.GetAllAsync();
            return _mapper.Map<List<EnderecoEntregaDTO>>(_entities);
        }

        public async Task<EnderecoEntregaDTO> CreateAsync(EnderecoEntregaParamDTO paramersDTO)
        {
            var _entity = _mapper.Map<EnderecoEntrega>(paramersDTO);
            _entity.UsuarioId = Guid.Parse(_currentUser.UserId);
            var entitySave = await _uow.EnderecoEntregaRepository.AddAndSaveAsync(_entity);
            var objDTO = _mapper.Map<EnderecoEntregaDTO>(entitySave);

            return objDTO;
        }

        public async Task<EnderecoEntregaDTO> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                throw new Exception("ID é invalido");

            var _entity = await _uow.EnderecoEntregaRepository.GetByGuidAsync(guidId);
            if (_entity == null)
                throw new Exception("Não achou!");

            return _mapper.Map<EnderecoEntregaDTO>(_entity);
        }

        public async Task<bool> UpdateAsync(EnderecoEntregaModifyDTO paramersDTO)
        {
            if (paramersDTO.Id == Guid.Empty)
                throw new Exception("ID é invalido");

            EnderecoEntrega _entity = await _uow.EnderecoEntregaRepository.GetByGuidAsync(paramersDTO.Id);
            if (_entity == null)
                throw new Exception("Não achou!");

            _entity = _mapper.Map<EnderecoEntregaModifyDTO, EnderecoEntrega>(paramersDTO, _entity);
            _entity.UsuarioId = Guid.Parse(_currentUser.UserId);

            await _uow.EnderecoEntregaRepository.UpdateAsync(_entity);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                throw new Exception("UserID is not valid");

            EnderecoEntrega _entity = await _uow.EnderecoEntregaRepository.GetByGuidAsync(guidId);
            if (_entity == null)
                throw new Exception("Não achou!");

            await _uow.EnderecoEntregaRepository.SoftDeleteAsync(guidId);
            return true;
        }

        public async Task<EnderecoEntregaDTO> ObterEnderecoPorCepAsync(string cep)
        {
            var endereco = await BuscarEnderecoViaCepAsync(cep) ?? await BuscarEnderecoPostmonAsync(cep);
            if (endereco == null)
            {
                throw new Exception("Não foi possível obter o endereço a partir do CEP informado.");
            }

            return endereco;
        }

        private async Task<EnderecoEntregaDTO> BuscarEnderecoViaCepAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            if (response.IsSuccessStatusCode)
            {
                var endereco = await response.Content.ReadFromJsonAsync<ViaCepResponse>();
                if (endereco != null && !endereco.Erro)
                {
                    return new EnderecoEntregaDTO
                    {
                        CEP = endereco.Cep,
                        Rua = endereco.Logradouro,
                        Bairro = endereco.Bairro,
                        Cidade = endereco.Localidade,
                        Estado = endereco.Uf,
                        Complemento = endereco.Complemento
                    };
                }
            }
            return null;
        }

        private async Task<EnderecoEntregaDTO> BuscarEnderecoPostmonAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"https://api.postmon.com.br/v1/cep/{cep}");
            if (response.IsSuccessStatusCode)
            {
                var endereco = await response.Content.ReadFromJsonAsync<PostmonResponse>();
                if (endereco != null)
                {
                    return new EnderecoEntregaDTO
                    {
                        CEP = endereco.Cep,
                        Rua = endereco.Logradouro,
                        Bairro = endereco.Bairro,
                        Cidade = endereco.Cidade,
                        Estado = endereco.Estado,
                        Complemento = endereco.Complemento
                    };
                }
            }
            return null;
        }

    }
}
