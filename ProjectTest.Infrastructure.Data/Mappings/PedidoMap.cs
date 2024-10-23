using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Entities.Enum;

namespace ProjectTest.Infrastructure.Data.Mappings
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("pedido");

            builder.Property(x => x.NumeroPedido)
            .HasColumnName("numero_pedido")
            .IsRequired();

            builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(200)
            .IsRequired();

            builder.Property(x => x.Valor)
            .HasColumnName("valor")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

            builder.Property(c => c.DataHoraEntrega)
                .HasColumnName("data_entrega")
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(x => x.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

            builder.HasOne(o => o.Usuario)
                .WithMany(w => w.Pedidos)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Property(x => x.StatusEntrega)
            .HasConversion<string>(c => c.ToString(), c => (EStatusEntrega)Enum.Parse(typeof(EStatusEntrega), c))
            .HasColumnName("status_entrega")
            .HasMaxLength(100)
            .IsRequired();


            new EntityGuidMap<Pedido>().AddCommonConfiguration(builder);
        }
    }
}
