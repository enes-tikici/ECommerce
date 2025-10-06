using ECommerce.Core.Entities;
using ECommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ECommerceDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ECommerceDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate!=null)
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

    }
}
