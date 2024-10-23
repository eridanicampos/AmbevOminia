using ProjectTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Interfaces.Service
{
    public interface IVendaService
    {
        Task<Venda> AddAsync(Venda vendaEntity);
        Task CancelarItemAsync(Guid vendaId, Guid itemId);
        Task CancelarVendaAsync(Guid vendaId);
        Task<List<Venda>> GetAllAsync();
        Task<Venda> UpdateAsync(Venda vendaEntity);
    }
}
