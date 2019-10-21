using Moneyger.Common;
using Moneyger.Entities;
using Moneyger.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneyger.Services.MCategory
{
    public interface ICategoryService : IServiceScoped
    {
        Task<Category> Get(Guid Id);
        Task<int> Count(CategoryFilter filter);
        Task<List<Category>> List(CategoryFilter filter);
    }
    public class CategoryService : ICategoryService
    {
        private IUOW UOW;
        public CategoryService(IUOW UnitOfWork)
        {
            this.UOW = UnitOfWork;
        }

        public async Task<int> Count(CategoryFilter filter)
        {
            int count = await UOW.categoryRepository.Count(filter);
            return count;
        }

        public async Task<Category> Get(Guid Id)
        {
            if (Id == Guid.Empty) return null;
            Category categories = await UOW.categoryRepository.Get(Id);
            if (categories == null) return null;
            return categories;
        }

        public async Task<List<Category>> List(CategoryFilter filter)
        {
            List<Category> list = await UOW.categoryRepository.List(filter);
            return list;
        }
    }
}
