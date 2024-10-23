using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces;
using ProjectTest.Infrastructure.Data.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Infrastructure.Data.Context
{

    public partial class ProjectTestContext : DbContext
    {
        private readonly ICurrentUserInfo _currentUser;

        public ProjectTestContext(DbContextOptions<ProjectTestContext> options, ICurrentUserInfo currentUser) : base(options)
        {
            _currentUser = currentUser;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new AcessoUsuarioMap());            
            modelBuilder.ApplyConfiguration(new VendaMap());
            modelBuilder.ApplyConfiguration(new ItemVendaMap());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public async Task<int> SaveChangesAsync()
        {

            foreach (var entry in ChangeTracker.Entries<Entity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.CreatedByUserId = _currentUser.UserId ?? "sistema_web";
                        break;

                    case EntityState.Modified:
                        if (entry.Entity.IsDeleted)
                        {
                            entry.Entity.DeleteAt = DateTime.Now;
                            entry.Entity.DeletedByUserId = _currentUser.UserId ?? "sistema_web";
                        }
                        else
                        {
                            entry.Entity.UpdateAt = DateTime.Now;
                            entry.Entity.UpdatedByUserId = _currentUser.UserId ?? "sistema_web";
                        }
                        break;
                }
            }

            return await base.SaveChangesAsync();

        }
    }

}
