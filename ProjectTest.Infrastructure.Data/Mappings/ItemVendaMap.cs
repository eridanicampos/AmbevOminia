using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTest.Domain.Entities;

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
                .HasColumnName("valor_unitario")
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.DescontoValorUnitario)
                .HasColumnName("desconto_valor_unitario")
                .HasColumnType("decimal(18,2)")
                .IsRequired(false); 

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
