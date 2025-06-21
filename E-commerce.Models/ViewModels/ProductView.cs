using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_commerce.Models.ViewModels
{
    public class ProductView
    {
        [Required(ErrorMessage = "*")]
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        public string Author { get; set; }

        [Required(ErrorMessage = "*")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(1, 1000, ErrorMessage = "Price must be between 1 and 1000")]
        public int price { get; set; }

        [Required(ErrorMessage = "*")]
        public int Price50 { get; set; }

        [Required(ErrorMessage = "*")]
        public int Price100 { get; set; }

        [Display(Name = "Category name")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? Image { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }


    }
}
