using DataLayer.Domin.Models.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
#nullable enable
    public interface IBaseRepository<TEntity> where TEntity : class, IDeleteBase
    {
        bool Any(bool deleted = false);

        bool Any(Expression<Func<TEntity, bool>> predicate, bool deleted = false);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool deleted = false);

        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate, bool deleted = false);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool deleted = false);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, bool deleted = false);

        IQueryable<TEntity> AsQueryable(bool deleted = false);

        Task<List<TEntity>> GetListAsync(bool deleted = false);

        List<TEntity> GetList(bool deleted = false);

        IQueryable<TEntity> Take(int count, bool deleted = false);

        Task<TEntity?> FindAsync(bool deleted = false, params object?[]? keyValues);

        Task<TEntity?> FindAsync(params object?[]? keyValues);

        Task Delete(int id);

        EntityEntry<TEntity> Add(TEntity entity);

        Task<EntityEntry<TEntity>> AddAsync(TEntity entity);

        EntityEntry<TEntity> Update(TEntity entity);
        
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool deleted = false);
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
