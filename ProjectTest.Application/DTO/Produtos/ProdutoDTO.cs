using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.DTO.Produtos
{
    public class ProdutoDTO
    {
        public Guid Id { get; set; }
        public string NomeProduto { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal? Desconto { get; set; }
    }
}
