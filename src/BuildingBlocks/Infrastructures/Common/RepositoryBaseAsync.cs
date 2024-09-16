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

        public Task EndTransactionAsync()
        {
            throw new NotImplementedException();
        }


        public Task RollBackTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateListAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }


        public Task<K> CreateListAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteListAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

   
        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<K> CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

    }
}
