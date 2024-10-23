using ProjectTest.Application.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAsync();
        Task<UserDTO> CreateAsync(UserParmersDTO UserDTO);
        Task<UserDTO> GetById(string id);
        Task<bool> Put(UserModifyDTO UserDTO);
        Task<bool> Delete(string id);
        Task<UserAuthenticateResponseViewModel> AuthenticateAsync(UserAuthenticateRequestViewModel user);
        Task<int> GetAcessoUltimosDias(string id, int qtdDias);
    }
}
