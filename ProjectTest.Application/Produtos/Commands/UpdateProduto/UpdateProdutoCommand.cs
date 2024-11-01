using ProjectTest.Application.Abstractions.Messaging;
using ProjectTest.Application.DTO.ItemVenda;
using ProjectTest.Application.DTO.Produtos;
using ProjectTest.Application.DTO.Venda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Produtos.Commands.UpdateProduto
{
    public class UpdateProdutoCommand : ICommand<ProdutoDTO>
    {
        public string NomeProduto { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal? Desconto { get; set; }
    }
}
