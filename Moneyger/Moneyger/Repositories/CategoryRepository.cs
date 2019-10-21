using Microsoft.EntityFrameworkCore;
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
        Task<int> Count(CategoryFilter filter);
        Task<List<Category>> List(CategoryFilter filter);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private WASContext wASContext;

        public CategoryRepository(WASContext wASContext)
        {
            this.wASContext = wASContext;
        }
        public async Task<int> Count(CategoryFilter filter)
        {
            IQueryable<CategoryDAO> categories = wASContext.Category;
            categories = DynamicFilter(categories, filter);
            return await categories.CountAsync();
        }

        /*public async Task<bool> Create(Category category)
        {
            wASContext.Add(new CategoryDAO
            {
                Id = category.Id,
                CX = category.CX,
                Name = category.Name,
                Type = category.Type,
                Image = category.Image
            });
            wASContext.SaveChangesAsync();
            return true;
        }*/

        /*public async Task<bool> Delete(Guid Id)
        {
            try
            {
                CategoryDAO category = wASContext.Category.Where(c => c.Id == Id).Select(c => new CategoryDAO()
                {

                    Id = c.Id,
                    CX = c.CX,
                    Name = c.Name,
                    Type = c.Type,
                    Image = c.Image
                }).FirstOrDefault();
                wASContext.Category.Remove(category);
                wASContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }*/

        public async Task<Category> Get(Guid Id)
        {
            CategoryDAO category = wASContext.Category
                .Where(c => c.Id.Equals(Id))
                .AsNoTracking()
                .FirstOrDefault();
            return new Category
            {
                Id = category.Id,
                CX = category.CX,
                Name = category.Name,
                Type = category.Type,
                Image = category.Image
            };
        }

        public async Task<List<Category>> List(CategoryFilter filter)
        {
            IQueryable<CategoryDAO> query = wASContext.Category;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);
            List<Category> list = await query.Select(q => new Category()
                {
                    Id = q.Id,
                    CX = q.CX,
                    Name = q.Name,
                    Type = q.Type,
                    Image = q.Image
                })
                .ToListAsync();
            return list;
        }

        /*public async Task<bool> Update(Category category)
        {
            wASContext.Category
                .Where(u => u.Id.Equals(category.Id))
                .UpdateFromQuery(u => new CategoryDAO
                {
                    Id = category.Id,
                    CX = category.CX,
                    Name = category.Name,
                    Type = category.Type,
                    Image = category.Image
                });
            wASContext.SaveChanges();
            return true;
        }*/

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

        private IQueryable<CategoryDAO> DynamicOrder(IQueryable<CategoryDAO> query, CategoryFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case CategoryOrder.Name:
                            query = query.OrderBy(g => g.Name);
                            break;
                        case CategoryOrder.Type:
                            query = query.OrderBy(g => g.Type);
                            break;
                        default:
                            query = query.OrderBy(e => e.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case CategoryOrder.Name:
                            query = query.OrderByDescending(g => g.Name);
                            break;
                        case CategoryOrder.Type:
                            query = query.OrderByDescending(g => g.Type);
                            break;
                        default:
                            query = query.OrderByDescending(e => e.CX);
                            break;
                    }
                    break;
                default:
                    query = query.OrderBy(e => e.CX);
                    break;
            }
            return query.Skip(filter.Skip).Take(filter.Take);
        }
    }
}
