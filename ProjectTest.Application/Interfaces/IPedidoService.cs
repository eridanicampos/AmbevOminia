using ProjectTest.Application.DTO.Pedido;
using ProjectTest.Application.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<List<PedidoDTO>> GetAsync();
        Task<PedidoDTO> CreateAsync(PedidoParamDTO UserDTO);
        Task<PedidoDTO> GetById(string id);
        Task<bool> UpdateAsync(PedidoModifyDTO UserDTO);
        Task<bool> Delete(string id);
    }
}
