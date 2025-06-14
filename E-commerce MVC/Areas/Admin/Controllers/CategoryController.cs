using System;
using E_commerce.Business.Admin.CategoryServices;
using E_commerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace E_commerce_MVC.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesViewAsync();

            return View(categories);
        }

        
        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public async Task<IActionResult> Create(CategoryView categoryView)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategoryAsync(categoryView);
                return RedirectToAction("index");
            }
            return View(categoryView);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var categoryView = await _categoryService.GetCategoryViewByIdAsync(id.Value);
            if (categoryView == null)
            {
                return NotFound();
            }
            return View(categoryView);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int? id,  CategoryView categoryView)
        {
            if (id != categoryView.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    categoryView = await _categoryService.UpdateCategoryAsync(categoryView);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await CategoryExists(categoryView.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoryView);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var categoryView = await _categoryService.GetCategoryViewByIdAsync(id.Value);
            if (categoryView == null)
            {
                return NotFound();
            }
            return View(categoryView);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var categoryView = await _categoryService.GetCategoryViewByIdAsync(id.Value);
            if (categoryView == null)
            {
                return BadRequest();
            }
            await _categoryService.DeleteCategoryAsync(id.Value);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(int? id)
        {
            bool isExist = true;
            if (await _categoryService.GetCategoryViewByIdAsync(id.Value)  == null) 
                isExist = false;
            return isExist;
        }
    }
}
