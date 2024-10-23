using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class Venda : EntityGuid
    {
        public long NumeroVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public Guid ClienteId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string Filial { get; set; } = string.Empty;
        public virtual ICollection<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
        public bool Cancelada { get; set; } = false;
        public decimal ValorTotal => Itens.Where(item => !item.Cancelada).Sum(item => item.ValorTotalItem);
        

        public override Task<(bool isValid, List<string> messages)> Validate()
        {
            var messages = new List<string>();

            if (DataVenda > DateTime.Now)
                messages.Add("A data da venda não pode estar no futuro.");

            if (ClienteId == Guid.Empty)
                messages.Add("O ClienteId é obrigatório.");

            if (string.IsNullOrWhiteSpace(Filial))
                messages.Add("A filial é obrigatória.");

            if (Itens == null || !Itens.Any())
                messages.Add("A venda deve conter pelo menos um item.");

            foreach (var item in Itens)
            {
                var (itemValid, itemMessages) = item.Validate().Result;
                if (!itemValid)
                {
                    messages.AddRange(itemMessages);
                }
            }

            bool isValid = !messages.Any();
            return Task.FromResult((isValid, messages));
        }
    }
}
