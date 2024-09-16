using Contracts.Common;
using Contracts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Infrastructures.Common
{
    public class RepositoryBaseAsync<T, K, TContext> : IRepositoryBaseAsync<T, K, TContext> where T : EntityBase<K> where TContext : DbContext
    {
        private readonly TContext _dbcontext;
        private readonly IUnitOfWork<TContext> _unitOfWork;

        public RepositoryBaseAsync(TContext dbContext, IUnitOfWork<TContext> unitOfWork)
        {
            _dbcontext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
        }
        public IQueryable<T> FindAll(bool trackChanges = false) => !trackChanges ? _dbcontext.Set<T>().AsNoTracking() : _dbcontext.Set<T>();

        public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var items = FindAll(trackChanges);
            items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
            return items;
        }

        public IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) => 
            !trackChanges ? _dbcontext.Set<T>().Where(expression).AsNoTracking() : _dbcontext.Set<T>().Where(expression);

        public IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var items = FindAllByCondition(expression, trackChanges);
            items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
            return items;
        }

        public async Task<T?> GetByIdAsync(K id) => await FindAllByCondition(x => x.Id.Equals(id)).FirstOrDefaultAsync();
        public async Task<T?> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties) => 
            await FindAllByCondition(x => x.Id.Equals(id),trackChanges: false, includeProperties).FirstOrDefaultAsync();

        public Task<IDbContextTransaction> BeginTransactionAsync() => _dbcontext.Database.BeginTransactionAsync();

        public async Task EndTransactionAsync()
        {
            await SaveChangesAsync();
            await _dbcontext.Database.CommitTransactionAsync();
        }

        public Task RollBackTransactionAsync() => _dbcontext.Database.RollbackTransactionAsync();
        

        public async Task<K> CreateAsync(T entity)
        {
            await _dbcontext.Set<T>().AddAsync(entity);
            return entity.Id;
        }


        public async Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
        {
            await _dbcontext.Set<T>().AddRangeAsync(entities);
            return entities.Select(x => x.Id).ToList();
        }

        public Task UpdateAsync(T entity)
        {
            if (_dbcontext.Entry(entity).State == EntityState.Unchanged) return Task.CompletedTask;
            T? exist = _dbcontext.Set<T>().Find(entity.Id);
            if (exist != null)
            {
                _dbcontext.Entry(exist).CurrentValues.SetValues(entity);
            }
            return Task.CompletedTask;
        }

        public Task UpdateListAsync(IEnumerable<T> entities) => _dbcontext.Set<T>().AddRangeAsync(entities);

        public Task DeleteAsync(T entity)
        {
            _dbcontext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteListAsync(IEnumerable<T> entities)
        {
            _dbcontext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task<int> SaveChangesAsync() => _unitOfWork.CommitAsync();

    }
}
