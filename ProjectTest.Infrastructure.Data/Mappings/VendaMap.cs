using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Infrastructure.Data.Mappings
{
    public class VendaMap : IEntityTypeConfiguration<Venda>
    {
        public void Configure(EntityTypeBuilder<Venda> builder)
        {
            builder.ToTable("venda");

            builder.Property(v => v.NumeroVenda)
                .IsRequired()
                .HasColumnName("numero_venda");

            builder.Property(v => v.DataVenda)
                .IsRequired()
                .HasColumnName("data_venda");

            builder.Property(v => v.ClienteId)
                .IsRequired()
                .HasColumnName("cliente_id");

            builder.Property(v => v.NomeCliente)
                .HasMaxLength(100)
                .HasColumnName("nome_cliente");

            builder.Property(v => v.Filial)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("filial");

            builder.Property(v => v.Cancelada)
                .HasDefaultValue(false)
                .HasColumnType("bit")
                .HasColumnName("cancelada");

            // Propriedade calculada (não armazenada no banco de dados)
            builder.Ignore(v => v.ValorTotal);

            new EntityGuidMap<Venda>().AddCommonConfiguration(builder);
        }
    }
}
