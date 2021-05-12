#nullable enable
using Project.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project.Infrastructure
{
    public abstract class GenericRepository<TEntity> where TEntity : class
    {
        private readonly ProjectContext _entities;
        private readonly DbSet<TEntity> _dbSet;

        protected GenericRepository(ProjectContext context)
        {
            _entities = context;
            _dbSet = context.Set<TEntity>();
        }
        
        public int Count()
        {
            return _dbSet.Count();
        }
        
        public int Count(Func<TEntity, bool> predicate)
        {
            return _dbSet.Count(predicate);
        }
        
        public IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<TEntity> GetAllReadOnly()
        {
            return _dbSet.AsNoTracking();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual void MassDelete(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual void MassDelete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual void MassDelete(IQueryable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public TEntity Add(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;
        }
        
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
        
        public void MassAdd(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate) => _dbSet.Where(predicate);

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate, out int totalRecords, int page = 1,
            int qtdRecords = int.MaxValue)
        {
            var result = _dbSet.Where(predicate).AsQueryable();
            totalRecords = _entities.Set<TEntity>().Count(predicate);
            ApplyPagination(ref result, page, qtdRecords);
            return result;
        }

        private static void ApplyPagination(ref IQueryable<TEntity> query, int? page, int? qtdRecords)
        {
            if (page.HasValue && qtdRecords.HasValue)
                query = query.Skip((page.Value - 1) * qtdRecords.Value)
                    .Take(qtdRecords.Value);
        }
    }

}


       