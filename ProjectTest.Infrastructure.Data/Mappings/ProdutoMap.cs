using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Infrastructure.Data.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("produto");

            builder.Property(p => p.NomeProduto)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("nome_produto");

            builder.Property(p => p.ValorUnitario)
                .IsRequired()
                .HasColumnName("valor_unitario")
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Desconto)
                .HasColumnName("desconto")
                .HasColumnType("decimal(18,2)");

            new EntityGuidMap<Produto>().AddCommonConfiguration(builder);
        }
    }
}
