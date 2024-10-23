using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Common;
using ProjectTest.Infrastructure.Data.Context;
using System.Linq.Expressions;

namespace ProjectTest.Infrastructure.Data.Repositories.Common
{
    public class GenericAsyncRepository<TEntity> : IGenericAsyncRepository<TEntity> where TEntity : class
    {
        protected readonly ProjectTestContext _dbContext;

        protected DbSet<TEntity> DbSet
        {
            get { return _dbContext.Set<TEntity>(); }
        }
        public GenericAsyncRepository(ProjectTestContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public virtual IQueryable<TEntity> Entity => _dbContext.Set<TEntity>();
        public virtual async Task<List<TEntity>> GetAllAsyncWithChildren(params Expression<Func<TEntity, object>>[] children)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            foreach (var child in children)
            {
                query = query.Include(child);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public virtual async Task<TEntity> GetByGuidAsyncWithChildren(Guid id, params Expression<Func<TEntity, object>>[] children)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            foreach (var child in children)
            {
                query = query.Include(child);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity is Entity baseEntity && baseEntity.IsDeleted)
            {
                return null;
            }

            _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        public virtual async Task<TEntity> GetByGuidAsync(Guid id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity is Entity baseEntity && baseEntity.IsDeleted)
            {
                return null;
            }

            _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        public async Task<List<TEntity>> GetByIdsAsync(List<int> ids)
        {
            List<TEntity> toReturn = new();
            foreach (var id in ids)
            {
                var entity = await _dbContext.Set<TEntity>().FindAsync(id);
                _dbContext.Entry(entity).State = EntityState.Detached;
                if (entity is Entity baseEntity)
                {
                    if (!baseEntity.IsDeleted)
                    {
                        toReturn.Add(entity);
                    }
                }
                else
                {
                    toReturn.Add(entity);
                }
            }
            return toReturn;
        }
        public async Task<List<TEntity>> GetByIdsAsNoTrackingAsync(List<int> ids)
        {
            List<TEntity> toReturn = new();
            foreach (var id in ids)
            {
                var elementFound = await _dbContext.Set<TEntity>().FindAsync(id);
                _dbContext.Entry(elementFound).State = EntityState.Detached;
                toReturn.Add(elementFound);
            }

            return toReturn;
        }
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            if (typeof(Entity).IsAssignableFrom(typeof(TEntity)))
            {
                return await _dbContext.Set<TEntity>()
                    .AsNoTracking()
                    .OfType<Entity>()
                    .Where(entity => !entity.IsDeleted)
                    .Cast<TEntity>()
                    .ToListAsync();
            }
            else
            {
                return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
            }
        }
        public async Task<List<TEntity>> ToListAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }
        public virtual async Task<int> CountTotalAsync()
        {
            return await EntityFrameworkQueryableExtensions.CountAsync(_dbContext.Set<TEntity>());
        }
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            return entity;
        }
        public virtual async Task<TEntity> AddAndSaveAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<List<TEntity>> AddListAsync(List<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
            return entities;
        }
        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public virtual async Task<List<TEntity>> UpdateListAsync(List<TEntity> entities)
        {
            await Task.Run(() => _dbContext.Set<TEntity>().UpdateRange(entities));
            return entities;
        }
        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await Task.CompletedTask;
        }
        public virtual async Task DeleteListAsync(List<TEntity> entities)
        {
            await Task.Run(() => _dbContext.Set<TEntity>().RemoveRange(entities));
        }
        public virtual async Task DeleteListAndSaveAsync(List<TEntity> entities)
        {
            await Task.Run(() => _dbContext.Set<TEntity>().RemoveRange(entities));
            await _dbContext.SaveChangesAsync();
        }
        public virtual async Task DeleteAndSaveAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public virtual async Task DeleteAllAsync()
        {
            var elements = await _dbContext.Set<TEntity>().ToListAsync();
            _dbContext.Set<TEntity>().RemoveRange(elements);
            await _dbContext.SaveChangesAsync();
        }
        public virtual async Task SoftDeleteAllAsync()
        {
            var entities = await _dbContext.Set<TEntity>().ToListAsync();
            entities.ForEach(model =>
            {
                if (model is Entity baseEntity)
                {
                    baseEntity.IsDeleted = true;
                    EntityEntry<TEntity> _entry = _dbContext.Entry(model);
                    DbSet.Attach(model);
                    _entry.State = EntityState.Modified;
                }
            });

            await _dbContext.SaveChangesAsync();
        }
        public async Task SoftDeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is Entity baseEntity)
            {
                baseEntity.IsDeleted = true;
                await UpdateAsync(entity);
            }
        }
        public async Task SoftDeleteAsync(Guid id)
        {
            var entity = await GetByGuidAsync(id);
            if (entity is Entity baseEntity)
            {
                baseEntity.IsDeleted = true;
                await UpdateAsync(entity);
            }
        }

        public async Task<int> SaveAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int Save()
        {
            try
            {
                return  _dbContext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected List<string> messages = new List<string>();
    }
}
