using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_commerce.DataAccess.Repositories;
using E_commerce.Models;
using E_commerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_commerce.Business.Admin.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryView>> GetCategoriesViewAsync(string[] includes)
        {
            IEnumerable<Category> categories = await _unitOfWork.Category.GetAllAsync(includes);

            IEnumerable<CategoryView> categoriesView = _mapper.Map<IEnumerable<CategoryView>>(categories);
            return categoriesView;
        }

        public async Task<CategoryView> CreateCategoryAsync(CategoryView categoryView)
        {
            if (categoryView == null) return null;
            Category category = _mapper.Map<Category>(categoryView);
            category = await _unitOfWork.Category.CreateAsync(category);
            await _unitOfWork.Save();
            categoryView = _mapper.Map<CategoryView>(category);
            return categoryView;
        }

        public async Task<int> DeleteCategoryAsync(int id)
        {
            Category category = await _unitOfWork.Category.GetByIdAsync(id);
            if (category == null) return 0;
            _unitOfWork.Category.Delete(id);
            await _unitOfWork.Save();
            return 1;
        }

        public async Task<CategoryView> GetCategoryViewByIdAsync(int id, string[] includes)
        {
            Category category = await _unitOfWork.Category.GetByIdAsync(id, includes);
            CategoryView categoryView = _mapper.Map<CategoryView>(category);
            return categoryView;
        }

        public async Task<CategoryView> UpdateCategoryAsync(CategoryView categoryView)
        {
            if (categoryView == null) return null;
            Category category = _mapper.Map<Category>(categoryView);
            category = _unitOfWork.Category.Update(category);
            await _unitOfWork.Save();
            categoryView = _mapper.Map<CategoryView>(category);
            return categoryView;
        }

        public async Task<IEnumerable<SelectListItem>> CreateCategorySelectList()
        {
            IEnumerable<Category> companies = await _unitOfWork.Category.GetAllAsync();
            IEnumerable<SelectListItem> categoryList = companies.Select(i => new SelectListItem { Text = i.Name, Value = i.Id.ToString() });
            return categoryList;
        }
    }
}
