using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class Produto : EntityGuid
    {
        public string NomeProduto { get; set; } 

        public decimal ValorUnitario { get; set; }
        public decimal? Desconto { get; set; }

        public override Task<(bool isValid, List<string> messages)> Validate()
        {
            var messages = new List<string>();

            if (string.IsNullOrWhiteSpace(NomeProduto))
                messages.Add("O nome do produto é obrigatório.");

            if (ValorUnitario <= 0)
                messages.Add("O valor unitário deve ser maior que zero.");

            if (Desconto.HasValue && Desconto.Value < 0)
                messages.Add("O desconto não pode ser negativo.");

            bool isValid = !messages.Any();
            return Task.FromResult((isValid, messages));
        }
    }

}
