using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_commerce.Models;
using E_commerce.Models;

namespace E_commerce.DataAccess.Repositories
{
    public interface IUnitOfWork
    {
        IBaseRepository<Category> Category { get; set; }
        IBaseRepository<Product> Product { get; set; }
        IBaseRepository<Company> Company { get; set; }
        IBaseRepository<CartItem> CartItem { get; set; }
        IBaseRepository<OrderDetails> OrderDetails { get; set; }
        IBaseRepository<OrderHeader> OrderHeader { get; set; }
        Task Save();

    }
}
