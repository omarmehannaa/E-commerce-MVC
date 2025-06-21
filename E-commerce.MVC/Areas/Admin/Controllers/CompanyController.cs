using AutoMapper;
using E_commerce.Business.Admin.CompanyServices;
using E_commerce.DataAccess.Repositories;
using E_commerce.Models;
using E_commerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<CompanyView> companiesView = await _companyService.GetCompaniesViewAsync();
            return View(companiesView);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            CompanyView companyView = new CompanyView();
            if (id != null)
            {
                companyView = await _companyService.GetCompanyViewByIdAsync(id.Value);
                if (companyView == null)
                    return NotFound();
            }
            return View(companyView);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(CompanyView companyView)
        {
            if (ModelState.IsValid)
            {
                if (companyView.Id != null || companyView.Id != 0)
                {
                    await _companyService.UpdateCompanyAsync(companyView);
                }
                else
                {
                    await _companyService.CreateCompanyAsync(companyView);
                }
                return RedirectToAction("Index");
            }
            return View(companyView);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            CompanyView companyView = await _companyService.GetCompanyViewByIdAsync(id.Value);
            if (companyView == null)
                return NotFound();
            return View(companyView);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return BadRequest();
            await _companyService.DeleteCompanyAsync(id.Value);
            return RedirectToAction("Index");
        }
    }
}
