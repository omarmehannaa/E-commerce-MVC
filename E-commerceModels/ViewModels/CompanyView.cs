using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.ViewModels
{
    public class CompanyView
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public string? Government { get; set; }
        public string? Address { get; set; }
    }
}
