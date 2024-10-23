using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Infrastructure.Data.Mappings
{
    public class EnderecoEntregaMap : IEntityTypeConfiguration<EnderecoEntrega>
    {
        public void Configure(EntityTypeBuilder<EnderecoEntrega> builder)
        {
            builder.ToTable("endereco_entrega");

            builder.Property(x => x.Rua)
            .HasColumnName("rua")
            .HasMaxLength(200)
            .IsRequired();

            builder.Property(x => x.Bairro)
            .HasColumnName("bairro")
            .HasMaxLength(200)
            .IsRequired();

            builder.Property(x => x.CEP)
            .HasColumnName("cep")
            .HasMaxLength(10)
            .IsRequired();

            builder.Property(x => x.Numero)
            .HasColumnName("numero")
            .HasMaxLength(50)
            .IsRequired(false);

            builder.Property(x => x.Cidade)
            .HasColumnName("cidade")
            .HasMaxLength(200)
            .IsRequired();

            builder.Property(x => x.Complemento)
            .HasColumnName("complemento")
            .HasMaxLength(200)
            .IsRequired();

            builder.Property(x => x.Estado)
            .HasColumnName("estado")
            .HasMaxLength(200)
            .IsRequired();

            builder.Property(x => x.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

            builder.HasOne(o => o.Usuario)
                .WithMany(w => w.Enderecos)
                .OnDelete(DeleteBehavior.Cascade);

        new EntityGuidMap<EnderecoEntrega>().AddCommonConfiguration(builder);
        }
    }
}
