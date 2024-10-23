using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Infrastructure.Data.Mappings
{
    public class AcessoUsuarioMap : IEntityTypeConfiguration<AcessoUsuario>
    {
        public void Configure(EntityTypeBuilder<AcessoUsuario> builder)
        {
            builder.ToTable("acesso_usuario");

            builder.Property(c => c.UsuarioId)
            .IsRequired()
            .HasColumnName("usuario_id");

            builder.HasOne(oi => oi.Usuario)
                .WithMany(oi => oi.AcessosUsuarios)
                .HasForeignKey(oi => oi.UsuarioId).OnDelete(DeleteBehavior.Cascade);


            new EntityGuidMap<AcessoUsuario>().AddCommonConfiguration(builder);
        }
    }
}
