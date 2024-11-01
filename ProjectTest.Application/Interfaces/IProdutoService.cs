using ProjectTest.Application.DTO.Pedido;
using ProjectTest.Application.DTO.User;
using ProjectTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<List<Produto>> GetAllAsync();
        Task<Produto> AddAsync(Produto produto);
        Task<Produto> GetById(Guid id);
        Task<Produto> UpdateAsync(Produto UserDTO);
        Task<bool> DeleteAsync(Guid id);
    }
}
