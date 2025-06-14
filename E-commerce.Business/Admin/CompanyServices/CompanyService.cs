using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_commerce.DataAccess.Repositories;
using E_commerce.Models;
using E_commerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_commerce.Business.Admin.CompanyServices
{
    public class CompanyService : ICompanyService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CompanyView>> GetCompaniesViewAsync(string[] includes)
        {
            IEnumerable<Company> companies = await _unitOfWork.Company.GetAllAsync(includes);

            IEnumerable<CompanyView> companiesView = _mapper.Map<IEnumerable<CompanyView>>(companies);
            return companiesView;
        }

        public async Task<CompanyView> CreateCompanyAsync(CompanyView companyView)
        {
            if (companyView == null) return null;
            Company company = _mapper.Map<Company>(companyView);
            company = await _unitOfWork.Company.CreateAsync(company);
            await _unitOfWork.Save();
            companyView = _mapper.Map<CompanyView>(company);
            return companyView;
        }

        public async Task<int> DeleteCompanyAsync(int id)
        {
            Company company = await _unitOfWork.Company.GetByIdAsync(id);
            if (company == null) return 0;
            _unitOfWork.Company.Delete(id);
            await _unitOfWork.Save();
            return 1;
        }

        public async Task<CompanyView> GetCompanyViewByIdAsync(int id, string[] includes)
        {
            Company company = await _unitOfWork.Company.GetByIdAsync(id, includes);
            CompanyView companyView = _mapper.Map<CompanyView>(company);
            return companyView;
        }

        public async Task<CompanyView> UpdateCompanyAsync(CompanyView companyView)
        {
            if (companyView == null) return null;
            Company company = _mapper.Map<Company>(companyView);
            company = _unitOfWork.Company.Update(company);
            await _unitOfWork.Save();
            companyView = _mapper.Map<CompanyView>(company);
            return companyView;
        }

    }
}
