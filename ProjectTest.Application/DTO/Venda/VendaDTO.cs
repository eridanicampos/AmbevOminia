using ProjectTest.Application.DTO.ItemVenda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO.Venda
{
    public class VendaDTO
    {
        public Guid Id { get; set; }
        public long NumeroVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public Guid ClienteId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string Filial { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public bool Cancelada { get; set; }
        public List<ItemVendaDTO> Itens { get; set; } = new();
    }
}
