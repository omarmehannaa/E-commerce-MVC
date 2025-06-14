using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models.ViewModels
{
    public class CategoryView
    {
        public int? Id { get; set; }

        [Required]
        [DisplayName("Category name")]
        public string Name { get; set; }

        [Required]
        [Range(1,100)]
        [DisplayName("Display order")]
        public int DisplayOrder { get; set; }
    }
}
