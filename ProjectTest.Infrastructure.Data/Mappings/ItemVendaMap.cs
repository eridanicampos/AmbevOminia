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
    public class ItemVendaMap : IEntityTypeConfiguration<ItemVenda>
    {
        public void Configure(EntityTypeBuilder<ItemVenda> builder)
        {
            builder.ToTable("item_venda");

            builder.Property(i => i.ProdutoId)
                .IsRequired()
                .HasColumnName("produto_id");

            builder.Property(i => i.NomeProduto)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nome_produto");

            builder.Property(i => i.Quantidade)
                .IsRequired()
                .HasColumnName("quantidade");

            builder.Property(i => i.ValorUnitario)
                .IsRequired()
                .HasColumnName("valor_unitario");

            builder.Property(i => i.Desconto)
                .IsRequired()
                .HasColumnName("desconto");

            // Propriedade calculada (não armazenada no banco de dados)
            builder.Ignore(i => i.ValorTotalItem);


            builder.Property(x => x.VendaId)
                .HasColumnName("venda_id")
                .IsRequired();

            builder.HasOne(x => x.Venda)
                .WithMany(u => u.Itens)
                .HasForeignKey(x => x.VendaId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Property(c => c.Cancelada)
                .HasColumnName("cancelada")
                .HasColumnType("bit")
                .HasDefaultValue(false);

            new EntityGuidMap<ItemVenda>().AddCommonConfiguration(builder);
        }
    }
}
