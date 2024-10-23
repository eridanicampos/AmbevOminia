using Microsoft.EntityFrameworkCore;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Infrastructure.Data.Context
{
    public partial class ProjectTestContext
    {
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<AcessoUsuario> Acessos { get; set; }
        public virtual DbSet<Venda> Vendas { get; set; }
        public virtual DbSet<ItemVenda> ItensVendas { get; set; }



    }
}
