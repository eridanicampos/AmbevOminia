using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class ItemVenda : EntityGuid
    {
        public Guid ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotalItem => Quantidade * ValorUnitario;

        public decimal? DescontoValorUnitario { get; set; } = null;
        public bool Cancelada { get; set; } = false;

        public virtual Guid VendaId { get; set; }
        public virtual Venda Venda { get; set; }
        public override Task<(bool isValid, List<string> messages)> Validate()
        {
            var messages = new List<string>();

            if (ProdutoId == Guid.Empty)
                messages.Add("O ProdutoId é obrigatório.");

            if (Quantidade <= 0)
                messages.Add("A quantidade deve ser maior que zero.");

            if (ValorUnitario <= 0)
                messages.Add("O valor unitário deve ser maior que zero.");


            bool isValid = !messages.Any();
            return Task.FromResult((isValid, messages));
        }
    }
}
