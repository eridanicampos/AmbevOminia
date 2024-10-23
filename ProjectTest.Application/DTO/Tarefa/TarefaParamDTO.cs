using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO.Tarefa
{
        public class TarefaParamDTO
        {
            [Required(ErrorMessage = "O título é obrigatório.")]
            [StringLength(100, MinimumLength = 5, ErrorMessage = "O título deve conter entre 5 e 100 caracteres.")]
            public string Titulo { get; set; }  

            [StringLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres.")]
            public string Descricao { get; set; }  

            [Required(ErrorMessage = "A prioridade é obrigatória.")]
            public EPrioridade Prioridade { get; set; } 

            public DateTime? DataLimite { get; set; }  

            [Range(0, int.MaxValue, ErrorMessage = "O tempo estimado deve ser maior que zero.")]
            public int? TempoEstimadoHoras { get; set; }  
        }

    
}
