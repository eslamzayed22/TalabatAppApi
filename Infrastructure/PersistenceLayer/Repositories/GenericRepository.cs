using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _storeDbContext;

        public GenericRepository(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
           return await _storeDbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _storeDbContext.Set<TEntity>().FindAsync(id);
        }
        public async Task AddAsync(TEntity entity)
        {
            await _storeDbContext.Set<TEntity>().AddAsync(entity);
        }
         
        public void Update(TEntity entity)
        {
            _storeDbContext.Set<TEntity>().Update(entity);
        }

        public void Remove(TEntity entity)
        {
            _storeDbContext.Set<TEntity>().Remove(entity);
        }

    }
}
