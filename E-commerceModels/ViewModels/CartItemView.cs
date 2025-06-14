using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_commerce.Models;

namespace E_commerce.Models.ViewModels
{
    public class CartItemView
    {
        public int Id { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ApplicationUserId { get; set; }
        public  ApplicationUser User { get; set; }
        public int UnitPrice
        {
            get
            {
                if (Quantity < 50)
                    return Product.Price;
                if (Quantity < 100)
                    return Product.Price50;
                if (Quantity >= 100)
                    return Product.Price100;
                return 0;
            }
        }
        public int TotalPrice => UnitPrice * Quantity;

    }
}
