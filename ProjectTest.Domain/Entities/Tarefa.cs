using ProjectTest.Domain.Entities.Enum;

namespace ProjectTest.Domain.Entities
{
    public class Tarefa : EntityGuid
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;
        public DateTime? DataConclusao { get; set; }
        public DateTime? DataLimite { get; set; }
        public EPrioridade Prioridade { get; set; } = EPrioridade.Baixa;
        public EStatusTarefa Status { get; set; } = EStatusTarefa.Pendente;

        public virtual Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public int? TempoEstimadoHoras { get; set; }
        public List<string> Comentarios { get; set; } = new List<string>();

        public override Task<(bool isValid, List<string> messages)> Validate()
        {
            var messages = new List<string>();

            if (string.IsNullOrEmpty(Titulo) || Titulo.Length < 5 || Titulo.Length > 100)
                messages.Add("O título deve conter entre 5 e 100 caracteres.");

            if (DataLimite.HasValue && DataLimite.Value < DateTime.UtcNow)
                messages.Add("A data limite não pode ser menor que a data atual.");

            if (DataConclusao.HasValue && Status != EStatusTarefa.Concluida)
                messages.Add("A tarefa não pode ter data de conclusão se o status não for 'Concluída'.");

            if (TempoEstimadoHoras.HasValue && TempoEstimadoHoras <= 0)
                messages.Add("O tempo estimado deve ser maior que zero, se definido.");

            bool isValid = !messages.Any();
            return Task.FromResult((isValid, messages));
        }
    }

}
