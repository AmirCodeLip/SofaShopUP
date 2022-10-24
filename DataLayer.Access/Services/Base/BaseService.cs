using DataLayer.Access.Data;
using DataLayer.Access.Services.Base;
using DataLayer.Domin.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace DataLayer.Access.Services
{
    public class BaseService<TEntity> : IBaseRepository<TEntity> where TEntity : class, IDeleteBase
    {
        protected readonly ApplicationDbContext _context;

        protected readonly DbSet<TEntity> _dbSet;
        public BaseService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public bool Any(bool deleted = false)
        {
            if (deleted)
                return _dbSet.Deleted().Any();
            else
                return _dbSet.NotDeleted().Any();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate, bool deleted = false)
        {
            if (deleted)
                return _dbSet.Deleted().Any(predicate);
            else
                return _dbSet.NotDeleted().Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool deleted = false)
        {
            if (deleted)
                return await _dbSet.Deleted().AnyAsync(predicate);
            else
                return await _dbSet.NotDeleted().AnyAsync(predicate);
        }

        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate, bool deleted = false)
        {
            if (deleted)
                return _dbSet.Deleted().FirstOrDefault(predicate);
            else
                return _dbSet.NotDeleted().FirstOrDefault(predicate);
        }

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool deleted = false)
        {
            if (deleted)
                return await _dbSet.Deleted().SingleOrDefaultAsync(predicate);
            else
                return await _dbSet.NotDeleted().SingleOrDefaultAsync(predicate);
        }
        
        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool deleted = false)
        {
            if (deleted)
                return await _dbSet.Deleted().FirstOrDefaultAsync(predicate);
            else
                return await _dbSet.NotDeleted().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, bool deleted = false)
        {
            if (deleted)
                return _dbSet.Deleted().Where(predicate);
            else
                return _dbSet.NotDeleted().Where(predicate);
        }

        public IQueryable<TEntity> AsQueryable(bool deleted = false)
        {
            if (deleted)
                return _dbSet.Deleted();
            else
                return _dbSet.NotDeleted();
        }

        public async Task<List<TEntity>> GetListAsync(bool deleted = false)
        {
            if (deleted)
                return await _dbSet.Deleted().ToListAsync();
            else
                return await _dbSet.NotDeleted().ToListAsync();
        }

        public List<TEntity> GetList(bool deleted = false)
        {
            if (deleted)
                return _dbSet.Deleted().ToList();
            else
                return _dbSet.NotDeleted().ToList();
        }

        public EntityEntry<TEntity> Add(TEntity entity)
        {
            return _dbSet.Add(entity);
        }

        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            return await _dbSet.AddAsync(entity);
        }

        public EntityEntry<TEntity> Update(TEntity entity)
        {
            return _dbSet.Update(entity);
        }

        public IQueryable<TEntity> Take(int count, bool deleted = false)
        {
            if (deleted)
                return _dbSet.Deleted().Take(count);
            else
                return _dbSet.NotDeleted().Take(count);
        }
        public async Task<TEntity?> FindAsync(bool deleted = false, params object?[]? keyValues)
        {
            var entityItem = await _dbSet.FindAsync(keyValues);
            if (entityItem != null)
            {
                if (deleted && !entityItem.IsDeleted)
                {
                    return null;
                }
                else if (!deleted && entityItem.IsDeleted)
                {
                    return null;
                }
            }
            return entityItem;
        }

        public async Task<TEntity?> FindAsync(params object?[]? keyValues) =>
          await this.FindAsync(false, keyValues);

        public async Task Delete(int id)
        {
            var item = await FindAsync(id);
            if (item != null)
            {
                item.Delete();
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
