using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_commerce.Models;
using E_commerce.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace E_commerce.Business.Admin.ProductServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductView>> GetProductsViewAsync(string[] includes = null);
        Task<ProductView> GetProductViewByIdAsync(int id, string[] includes = null);
        Task<ProductView> CreateProductAsync(ProductView productView, string rootPath = null, IFormFile file = null);
        Task<ProductView> UpdateProductAsync(ProductView productView, string rootPath = null, IFormFile file = null);
        Task<int> DeleteProductAsync(int id);
        Task AddImage(Product product, string rootPath, IFormFile file);
        Task DeleteImage(Product product, string path);
    }
}
