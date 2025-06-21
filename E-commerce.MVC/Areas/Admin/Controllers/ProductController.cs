using E_commerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using E_commerce.Business.Admin.ProductServices;
using E_commerce.Business.Admin.CategoryServices;

namespace E_commerce.MVC.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductService _productService; 
        private readonly ICategoryService _categoryService;

        public ProductController(IWebHostEnvironment webHostEnvironment, IProductService productService, ICategoryService categoryService)
        {
            _webHostEnvironment = webHostEnvironment;
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductView> productsView = await _productService.GetProductsViewAsync(["Category"]);
            productsView = productsView.Take(10);
            return View(productsView);
        }

        public async Task<IActionResult> UpSert(int? id)
       {
            ProductView productView = new ProductView();
            productView.CategoryList = await _categoryService.CreateCategorySelectList();
            if (id != null && id != 0)
            {
                productView = await _productService.GetProductViewByIdAsync(id.Value);
                if (productView == null)
                    return NotFound();
            }
            return View(productView);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ProductView productView, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath;
                if (productView.Id == null || productView.Id == 0)
                {
                    await _productService.CreateProductAsync(productView, rootPath, file);

                }
                else
                {
                     await _productService.UpdateProductAsync(productView, rootPath, file);
                }
                return RedirectToAction("Index");
            }
            return View(productView);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            ProductView productView = await _productService.GetProductViewByIdAsync(id.Value, ["Category"]);
            if (productView == null)
                return NotFound();
            return View(productView);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
                return BadRequest();
            await _productService.DeleteProductAsync(id.Value);
            return RedirectToAction("Index");
        }
    }
}
