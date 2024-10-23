using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Infrastructure.Data.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario");

            builder.Property(x => x.Nome).HasColumnName("nome").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Senha).HasColumnName("senha").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(200).IsRequired();

            new EntityGuidMap<Usuario>().AddCommonConfiguration(builder);
        }
    }
}
