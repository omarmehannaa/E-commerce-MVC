using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_commerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_commerce.Business.Admin.CompanyServices
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyView>> GetCompaniesViewAsync(string[] includes = null);
        Task<CompanyView> GetCompanyViewByIdAsync(int id, string[] includes = null);
        Task<CompanyView> CreateCompanyAsync(CompanyView companyView);
        Task<CompanyView> UpdateCompanyAsync(CompanyView companyView);
        Task<int> DeleteCompanyAsync(int id);
    }
}
