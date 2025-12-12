using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey> (IQueryable<TEntity> baseQuery, ISpecifications<TEntity, TKey> specifications)  where TEntity : BaseEntity<TKey>
        {
            var query = baseQuery;
            if (specifications.Criteria is not null)
            {
                query = query.Where(specifications.Criteria);
            }
            if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count > 0)
            {
                //foreach (var include in specifications.IncludeExpressions)
                //{
                //    query = query.Include(include);
                //}

                //Or
                query = specifications.IncludeExpressions.Aggregate(query, (currentQuery, includeExp) => currentQuery.Include(includeExp));
            }

            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);
            
            if (specifications.OrderByDescending is not null)
                query = query.OrderByDescending(specifications.OrderByDescending);
            
            if (specifications.IsPaginated)
            {
                query = query.Skip(specifications.Skip).Take(specifications.Take);
            }

            return query;
        }
    }
}
