using ProjectTest.Application.DTO.Pedido;
using ProjectTest.Application.DTO.Tarefa;
using ProjectTest.Application.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Interfaces
{
    public interface ITarefaService
    {
        Task<List<TarefaDTO>> GetAsync();

        Task<TarefaDTO> CreateAsync(TarefaParamDTO paramDTO);

        Task<TarefaDTO> GetById(string id);

        Task<bool> UpdateAsync(TarefaModifyDTO paramDTO);

        Task<bool> DeleteAsync(string id);
    }
}
