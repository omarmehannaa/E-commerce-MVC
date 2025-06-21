using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.ViewModels
{
    public class OrderDetailsView
    {
        public int? Id { get; set; }
        public int OrderHeaderId { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
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
