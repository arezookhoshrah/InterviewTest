
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Test.Application.Contracts.Persistence;
using Test.Domain.Common;
using Test.Infrastructure.Persistence;

namespace Test.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly TestDbContext _testDbContext;
        private readonly DbSet<T> _dbSet;
        public RepositoryBase( TestDbContext testDbContext)
        {
            _testDbContext = testDbContext;
            _dbSet = _testDbContext.Set<T>();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, bool tracking=false)
        {
            if (!tracking)
            {
                return await _dbSet.FindAsync(id);
            }
            else
            {
               return  _dbSet.Where(x => x.Id == id).AsNoTracking().SingleOrDefault();
            }
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _testDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Entry(entity).State = EntityState.Modified;
            await _testDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity )
        {
            try
            {
                _dbSet.Remove(entity);
                await _testDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }



       
    }
}
