using Moneyger.Entities;
using Moneyger.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> Get(Guid Id);
        Task<bool> Create(Category category);
        Task<bool> Update(Category category);
        Task<bool> Delete(Guid Id);
        Task<int> Count(TransactionFilter filter);
        Task<List<Category>> List(CategoryFilter filter);
    }
    public class CategoryRepository : ICategoryRepository
    {
        public Task<int> Count(TransactionFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Create(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> Get(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> List(CategoryFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Category category)
        {
            throw new NotImplementedException();
        }

        private IQueryable<CategoryDAO> DynamicFilter(IQueryable<CategoryDAO> query, CategoryFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.Id != null)
            {
                if (filter.Id.Equal != null)
                    query = query.Where(c => c.Id.Equals(filter.Id.Equal));
                if (filter.Id.NotEqual != null)
                    query = query.Where(c => c.Id.Equals(filter.Id.NotEqual));
            }
            if (filter.Name != null)
            {
                if (filter.Name.Equal != null)
                    query = query.Where(c => c.Name.Equals(filter.Name.Equal));
                if (filter.Name.NotEqual != null)
                    query = query.Where(c => c.Name.Equals(filter.Name.NotEqual));
                if (filter.Name.Contains != null)
                    query = query.Where(c => c.Name.Contains(filter.Name.Contains));
                if (filter.Name.EndsWith != null)
                    query = query.Where(c => c.Name.EndsWith(filter.Name.EndsWith));
                if (filter.Name.NotEndsWith != null)
                    query = query.Where(c => c.Name.EndsWith(filter.Name.NotEndsWith));
                if (filter.Name.NotContains != null)
                    query = query.Where(c => c.Name.Contains(filter.Name.NotContains));
                if (filter.Name.StartsWith != null)
                    query = query.Where(c => c.Name.StartsWith(filter.Name.StartsWith));
                if (filter.Name.NotStartsWith != null)
                    query = query.Where(c => c.Name.StartsWith(filter.Name.NotStartsWith));
            }
            


            return query;
        }

        private IQueryable<TransactionDAO> DynamicOrder(IQueryable<TransactionDAO> query, TransactionFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case TransactionOrder.WalletId:
                            query = query.OrderBy(g => g.WalletId);
                            break;
                        case TransactionOrder.CategoryId:
                            query = query.OrderBy(g => g.CategoryId);
                            break;
                        case TransactionOrder.Id:
                            query = query.OrderBy(g => g.Id);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case TransactionOrder.WalletId:
                            query = query.OrderByDescending(g => g.WalletId);
                            break;
                        case TransactionOrder.CategoryId:
                            query = query.OrderByDescending(g => g.CategoryId);
                            break;
                        case TransactionOrder.Id:
                            query = query.OrderByDescending(g => g.Id);
                            break;
                    }
                    break;
                default:
                    query = query.OrderBy(e => e.Id);
                    break;
            }
            return query.Skip(filter.Skip).Take(filter.Take);
        }
    }
}
