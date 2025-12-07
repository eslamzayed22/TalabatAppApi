using DomainLayer.Contracts;
using DomainLayer.Models;
using PersistenceLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _storeDbContext;

        public UnitOfWork(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }

        private readonly Dictionary<string, object> _repositories = [];
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name;
            if (_repositories.ContainsKey(typeName))
               return (IGenericRepository<TEntity, TKey>)_repositories[typeName];
            else
            {
                //Create Repo Object
                var Repo = new GenericRepository<TEntity, TKey>(_storeDbContext);
                //Save Repo in Dictionary
                _repositories.Add(typeName, Repo);
                //Return Repo
                return Repo;
            }

        }

        public async Task<int> SaveChangesAsync()
        => await _storeDbContext.SaveChangesAsync();  
        
    }
}
