using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Interfaces.Common
{
    public interface IGenericAsyncRepository<TEntity> where TEntity : class
    {
        /// <summary>
        ///     Iqueryable entity of Entity Framework. Use this to execute query in database level.
        /// </summary>
        IQueryable<TEntity> Entity { get; }
        Task<List<TEntity>> GetAllAsyncWithChildren(params Expression<Func<TEntity, object>>[] children);
        Task<TEntity> GetByGuidAsyncWithChildren(Guid id, params Expression<Func<TEntity, object>>[] children);
        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> GetByIdsAsync(List<int> ids);
        Task<TEntity> GetByGuidAsync(Guid id);
        Task<int> CountTotalAsync();
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> AddListAsync(List<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAndSaveAsync(TEntity entity);
        Task<TEntity> AddAndSaveAsync(TEntity entity);
        Task DeleteListAsync(List<TEntity> entities);
        Task DeleteListAndSaveAsync(List<TEntity> entities);
        Task<List<TEntity>> UpdateListAsync(List<TEntity> entities);
        Task<List<TEntity>> ToListAsync();
        Task DeleteAllAsync();
        Task SoftDeleteAllAsync();

        Task SoftDeleteAsync(int id);

        Task SoftDeleteAsync(Guid id);

        Task<int> SaveAsync();
        int Save();

    }
}
