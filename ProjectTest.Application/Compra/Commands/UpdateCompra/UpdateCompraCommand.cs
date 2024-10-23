using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO.ItemVenda;
using ProjectTest.Application.DTO.Venda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Compra.Commands.UpdateCompra
{
    public class UpdateCompraCommand : ICommand<VendaDTO>
    {
        public Guid VendaId { get; set; }
        public Guid ClienteId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string Filial { get; set; } = string.Empty;
        public List<CreateItemVendaDTO> Itens { get; set; } = new();
    }
}
