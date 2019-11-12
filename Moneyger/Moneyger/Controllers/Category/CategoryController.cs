using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moneyger.Entities;
using Moneyger.Services.MCategory;

namespace Moneyger.Controllers.Category
{
    public class CategoryRoute : Root
    {
        public const string Default = Base + "/category";
        public const string List = Default + "/list";
    }

    public class CategoryController : ApiController
    {
        private ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [Route(CategoryRoute.List), HttpPost]
        public async Task<List<CategoryDTO>> List()
        {
            CategoryFilter filter = new CategoryFilter
            {
                OrderBy = CategoryOrder.Name
            };

            List<Entities.Category> category = await categoryService.List(filter);
            List<CategoryDTO> res = new List<CategoryDTO>();
            category.ForEach(c =>
            {
                res.Add(new CategoryDTO
                {
                    Errors = c.Errors,
                    Type = c.Type,
                    Name = c.Name
                });
            });
            return res;
        }
    }
}