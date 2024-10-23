using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO.Tarefa
{
    public class TarefaDTO : EntityPKDTO
    {
        public string Titulo { get; set; }  

        public string Descricao { get; set; }  

        public DateTime DataCriacao { get; set; }  

        public DateTime? DataConclusao { get; set; }  

        public DateTime? DataLimite { get; set; }  

        public EPrioridade Prioridade { get; set; } 

        public EStatusTarefa Status { get; set; } 
        public int? TempoEstimadoHoras { get; set; }  

        public Guid UsuarioId { get; set; }  

    }
}
