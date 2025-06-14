using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_commerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_commerce.Business.Admin.CategoryServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryView>> GetCategoriesViewAsync(string[] includes = null);
        Task<CategoryView> GetCategoryViewByIdAsync(int id, string[] includes = null);
        Task<CategoryView> CreateCategoryAsync(CategoryView categoryView);
        Task<CategoryView> UpdateCategoryAsync(CategoryView categoryView);
        Task<int> DeleteCategoryAsync(int id);
        Task<IEnumerable<SelectListItem>> CreateCategorySelectList();
    }
}
