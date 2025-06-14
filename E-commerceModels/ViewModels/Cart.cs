using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.ViewModels
{
    public class Cart
    {
        public List<CartItemView> Items { get; set; }
        public int TotalPrice => Items.Sum(i => i.TotalPrice);

    }
}
