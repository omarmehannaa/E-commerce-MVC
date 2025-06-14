using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_commerce.DataAccess.Repositories;
using E_commerce.Models;

namespace E_commerce.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IBaseRepository<Category> Category { get; set; }
        public IBaseRepository<Product> Product { get; set; }
        public IBaseRepository<Company> Company { get; set; }
        public IBaseRepository<CartItem> CartItem { get; set; }
        public IBaseRepository<OrderDetails> OrderDetails { get; set; }
        public IBaseRepository<OrderHeader> OrderHeader { get; set; }


        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new BaseRepository<Category>(_context);
            Product = new BaseRepository<Product>(_context);
            Company = new BaseRepository<Company>(_context);
            CartItem = new BaseRepository<CartItem>(_context);
            OrderDetails = new BaseRepository<OrderDetails>(_context);
            OrderHeader = new BaseRepository<OrderHeader>(_context);
        }
        public Task Save()
        {
            return _context.SaveChangesAsync();
        }
    }
}
