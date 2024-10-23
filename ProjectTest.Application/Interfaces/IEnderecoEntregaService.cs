using ProjectTest.Application.DTO.EnderecoEntrega;
using ProjectTest.Application.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Interfaces
{
    public interface IEnderecoEntregaService
    {
        Task<List<EnderecoEntregaDTO>> GetAsync();
        Task<EnderecoEntregaDTO> CreateAsync(EnderecoEntregaParamDTO UserDTO);
        Task<EnderecoEntregaDTO> GetById(string id);
        Task<bool> UpdateAsync(EnderecoEntregaModifyDTO UserDTO);
        Task<bool> Delete(string id);
        Task<EnderecoEntregaDTO> ObterEnderecoPorCepAsync(string cep);
    }
}
