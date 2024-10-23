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
    public class TarefaMap : IEntityTypeConfiguration<Tarefa>
    {
        public void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            builder.ToTable("tarefa");

            builder.Property(x => x.Titulo)
                .HasColumnName("titulo")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(500)
                .IsRequired(false); 

            builder.Property(x => x.DataCriacao)
                .HasColumnName("data_criacao")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(x => x.DataConclusao)
                .HasColumnName("data_conclusao")
                .HasColumnType("datetime2")
                .IsRequired(false); 

            builder.Property(x => x.DataLimite)
                .HasColumnName("data_limite")
                .HasColumnType("datetime2")
                .IsRequired(false); 

            builder.Property(x => x.Status)
                .HasConversion<string>(c => c.ToString(), c => (EStatusTarefa)Enum.Parse(typeof(EStatusTarefa), c))
                .HasColumnName("status")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Prioridade)
                .HasConversion<string>(c => c.ToString(), c => (EPrioridade)Enum.Parse(typeof(EPrioridade), c))
                .HasColumnName("prioridade")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.TempoEstimadoHoras)
                .HasColumnName("tempo_estimado_horas")
                .IsRequired(false);

            builder.Property(x => x.Comentarios)
               .HasColumnName("comentarios")
               .IsRequired(false);

            builder.Property(x => x.UsuarioId)
                .HasColumnName("usuario_id")
                .IsRequired();

            builder.HasOne(x => x.Usuario)
                .WithMany(u => u.Tarefas)  
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade); 

            new EntityGuidMap<Tarefa>().AddCommonConfiguration(builder);
        }
    }
}
