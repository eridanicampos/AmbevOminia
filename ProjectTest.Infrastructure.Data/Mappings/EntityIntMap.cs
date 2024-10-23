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
    public class EntityIntMap<T> where T : EntityInt
    {

        public void AddCommonConfiguration(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id");

            new EntityMap<T>().AddCommonConfiguration(builder);
        }
    }
}
