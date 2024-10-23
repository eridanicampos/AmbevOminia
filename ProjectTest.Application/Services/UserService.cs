using AutoMapper;
using ProjectTest.Application.Auth;
using ProjectTest.Application.DTO.User;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Entities;
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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper mapper;
        private readonly IUserRepository _repository;

        public UserService(IUnitOfWork uow, IMapper mapper, IUserRepository repository)
        {
            this._uow = uow;
            this.mapper = mapper;
            this._repository = repository;
        }

        public async Task<List<UserDTO>> GetAsync()
        {
            IEnumerable<Usuario> _users = await _uow.UserRepository.GetAllAsync();
            return mapper.Map<List<UserDTO>>(_users);
        }

        public async Task<UserDTO> CreateAsync(UserParmersDTO UserDTO)
        {
            try
            {
                Validator.ValidateObject(UserDTO, new ValidationContext(UserDTO), true);

                Usuario _user = mapper.Map<Usuario>(UserDTO);
                _user.Senha = HashPassword(_user.Senha);

                var userEntity = await _uow.UserRepository.AddAndSaveAsync(_user);
                var userDTO = mapper.Map<UserDTO>(userEntity);

                return userDTO;

            }
            catch (Exception ex)
            {
                throw ex;
            }
                        
        }

        public async Task<UserDTO> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("UserID is not valid");

            Usuario _user = await _uow.UserRepository.GetByGuidAsync(userId);
            if (_user == null)
                throw new Exception("User not found");

            return mapper.Map<UserDTO>(_user);
        }

        public async Task<bool> Put(UserModifyDTO UserDTO)
        {
            if (UserDTO.Id == Guid.Empty)
                throw new Exception("ID is invalid");

            Usuario _user = await _uow.UserRepository.GetByGuidAsync(UserDTO.Id);
            if (_user == null)
                throw new Exception("User not found");

            _user = mapper.Map<UserModifyDTO, Usuario>(UserDTO, _user);
            _user.Senha = HashPassword(UserDTO.Senha);

            await _uow.UserRepository.UpdateAsync(_user);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                throw new Exception("UserID is not valid");

            Usuario _user = await _uow.UserRepository.GetByGuidAsync(userId);
            if (_user == null)
                throw new Exception("User not found");

            await _uow.UserRepository.SoftDeleteAsync(userId);
            return true;
        }

        public async Task<UserAuthenticateResponseViewModel> AuthenticateAsync(UserAuthenticateRequestViewModel user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                throw new Exception("Email/Password are required.");

            Usuario _user = await _uow.UserRepository.FindUser(user.Email);

            if (_user == null)
                throw new Exception("User not found");

            bool isPasswordValid = VerifyPassword(user.Password, _user.Senha);

            if (!isPasswordValid)
                throw new Exception("Invalid password");

            try
            {
                var token = SettingsAuthService.GenerateJwtToken(_user);
                var UserDTO = mapper.Map<UserDTO>(_user);

                var result = new UserAuthenticateResponseViewModel(UserDTO, token);

                await _uow.AcessoUsuarioRepository.AddAndSaveAsync(new AcessoUsuario() { UsuarioId = _user.Id});

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetAcessoUltimosDias(string id, int qtdDias)
        {
            return await _uow.AcessoUsuarioRepository.GetAcessoUltimosDias(id, qtdDias);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }
}
