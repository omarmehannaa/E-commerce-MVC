using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace E_commerce.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public string? Country { get; set; }
        public string? Government { get; set; }
        public string? Address { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
