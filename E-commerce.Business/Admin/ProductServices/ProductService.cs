using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_commerce.DataAccess.Repositories;
using E_commerce.Models.ViewModels;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using E_commerce.Business.Admin.CategoryServices;

namespace E_commerce.Business.Admin.ProductServices
{
    public class ProductService : IProductService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ICategoryService _categoryService;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<ProductView>> GetProductsViewAsync(string[] includes)
        {
            IEnumerable<Product> products = await _unitOfWork.Product.GetAllAsync(includes);
            IEnumerable<ProductView> productsView = _mapper.Map<IEnumerable<ProductView>>(products);            
            return productsView;
        }

        public async Task<ProductView> CreateProductAsync(ProductView productView, string rootPath, IFormFile file)
        {
            if (productView == null) return null;
            Product product = _mapper.Map<Product>(productView);
            if (file != null && rootPath != null)
            {
                await AddImage(product, rootPath, file);
            }
            product = await _unitOfWork.Product.CreateAsync(product);
            await _unitOfWork.Save();
            productView = _mapper.Map<ProductView>(product);

            return productView;
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            Product product = await _unitOfWork.Product.GetByIdAsync(id);
            if (product == null) return 0;
            _unitOfWork.Product.Delete(id);
            await _unitOfWork.Save();
            return 1;
        }

        public async Task<ProductView> GetProductViewByIdAsync(int id, string[] includes)
        {
            Product product = await _unitOfWork.Product.GetByIdAsync(id, includes);
            ProductView productView = _mapper.Map<ProductView>(product);
            IEnumerable<SelectListItem> categoryList = await _categoryService.CreateCategorySelectList();
            productView.CategoryList = categoryList;
            return productView;
        }

        public async Task<ProductView> UpdateProductAsync(ProductView productView, string rootPath, IFormFile file)
        {
            if (productView == null) return null;
            Product product = _mapper.Map<Product>(productView);
            if (file != null && rootPath != null)
            {
                await DeleteImage(product, rootPath);
                await AddImage(product, rootPath, file);
            }
            product = _unitOfWork.Product.Update(product);
            await _unitOfWork.Save();
            productView = _mapper.Map<ProductView>(product);

            return productView;
        }

        public  Task AddImage(Product product, string rootPath, IFormFile file)
        {
            string imagePath = Path.Combine(rootPath, "images", "product");
            using (var fileStream = new FileStream(Path.Combine(imagePath, file.FileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            product.Image = $@"\images\product\{file.FileName}";
            return Task.CompletedTask;
        }

        public Task DeleteImage(Product product, string rootPath)
        {

            if (!string.IsNullOrEmpty(product.Image))
            {
                var oldImagePath = Path.Combine(rootPath, product.Image.TrimStart('\\'));
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
            }
            return Task.CompletedTask;
        }
    }
}
