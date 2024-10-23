using MediatR;
using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO.ItemVenda;
using ProjectTest.Application.DTO.Venda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectTest.Application.Compra.Commands.CreateCompra
{
    public class CreateCompraCommand : ICommand<VendaDTO>
    {
        public Guid ClienteId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string Filial { get; set; } = string.Empty;
        public List<CreateItemVendaDTO> Itens { get; set; } = new();
    }
}
