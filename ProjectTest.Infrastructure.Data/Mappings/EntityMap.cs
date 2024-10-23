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
    public class EntityMap<T> where T : Entity
    {
        public void AddCommonConfiguration(EntityTypeBuilder<T> builder)
        {
            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetime2")
                .HasComment("When this entity was created in this DB");

            builder.Property(c => c.UpdateAt)
                .HasColumnName("update_at")
                .HasColumnType("datetime2")
                .IsRequired(false)
                .HasComment("When this entity was modified the last time");

            builder.Property(c => c.CreatedByUserId)
                .HasColumnName("created_by_user_id")
                .HasColumnType("VARCHAR(100)")
                .HasComment("The id of the user who did create");

            builder.Property(c => c.UpdatedByUserId)
                .HasColumnName("update_by_user_id")
                .HasColumnType("VARCHAR(100)")
                .IsRequired(false)
                .HasComment("The id of the user who did the last modification");

            builder.Property(c => c.IsDeleted)
                .HasColumnName("is_deleted")
                .HasColumnType("bit")
                .HasDefaultValue(false)
                .HasComment("The field that identifies that the entity was deleted");

            builder.Property(c => c.DeletedByUserId)
                .HasColumnName("deleted_by_user_id")
                .HasColumnType("VARCHAR(100)")
                .IsRequired(false)
                .HasComment("The id of the user who did the delete");

            builder.Property(c => c.DeleteAt)
                .HasColumnName("deleted_at")
                .HasColumnType("datetime2")
                .IsRequired(false)
                .HasComment("When this entity was deleted in this DB");
        }

    }
}
