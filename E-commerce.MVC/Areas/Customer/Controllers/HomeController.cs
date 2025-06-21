using System.Diagnostics;
using AutoMapper;
using E_commerce.Models;
using E_commerce.Models.ViewModels;
using E_commerce.DataAccess.Repositories;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace E_commerce.MVC.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.Product.GetAllAsync();
            return View(_mapper.Map<IEnumerable<ProductView>>(products));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();
            Product product = await _unitOfWork.Product.GetByIdAsync(id.Value, ["Category"]);
            return View(_mapper.Map<ProductView>(product));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(int? id, int quantity)
        {
            Claim claimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userId = claimId.Value;
            CartItem item = await _unitOfWork.CartItem.GetOneByFilterAsync(i => i.ProductId == id && i.ApplicationUserId == userId);
            if (item == null)
            {
                item = new CartItem() 
                {
                    ProductId = id.Value,
                    Quantity = quantity,
                    ApplicationUserId = userId
                };
                await _unitOfWork.CartItem.CreateAsync(item);
            }
            else
            {
                item.Quantity += quantity;
                _unitOfWork.CartItem.Update(item);
            }
            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
