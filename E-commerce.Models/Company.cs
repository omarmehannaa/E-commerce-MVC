using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public string? Government { get; set; }
        public string? Address { get; set; }
        public virtual ApplicationUser applicationUser {  get; set; }
    }
}
