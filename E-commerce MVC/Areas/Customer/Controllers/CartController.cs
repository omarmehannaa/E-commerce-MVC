using System.Security.Claims;
using AutoMapper;
using E_commerce.DataAccess.Repositories;
using E_commerce.Models;
using E_commerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace E_commerce_MVC.Areas.Customer.Controllers
{

    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            Claim claimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userId = claimId.Value;
            IEnumerable<CartItem> items = await _unitOfWork.CartItem.GetAllByFilterAsync(i => i.ApplicationUserId == userId, ["Product"]);
             Cart cart = new Cart() { Items = _mapper.Map<List<CartItemView>>(items) };
            cart.Items = _mapper.Map<List<CartItemView>>(items);
            return View(cart);
        }

        public async Task<IActionResult> IncreaseQuantity(int? id)
        {
            if (id == null)
                return BadRequest();
            var item = await _unitOfWork.CartItem.GetByIdAsync(id.Value);
            if (item.Quantity >= 100)
                return RedirectToAction(nameof(Index));
            item.Quantity++;
            _unitOfWork.CartItem.Update(item);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DecreaseQuantity(int? id)
        {
            if (id == null)
                return BadRequest();
            var item = await _unitOfWork.CartItem.GetByIdAsync(id.Value);
            if (item.Quantity == 1)
                _unitOfWork.CartItem.Delete(item.Id);
            else
                item.Quantity--;
            _unitOfWork.CartItem.Update(item);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var item = await _unitOfWork.CartItem.GetByIdAsync(id.Value);
            _unitOfWork.CartItem.Delete(item.Id);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Summary()
        {
            Claim idClaim = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            string userId = idClaim.Value;
            var user = await _userManager.GetUserAsync(User);
            OrderHeaderView orderHeaderView = new OrderHeaderView()
            {
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Country = user.Country,
                Government = user.Government,
                Name = $"{user.FirstName} {user.LastName}"
            };
            List<OrderDetailsView> orderDetails = new List<OrderDetailsView>();
            IEnumerable<CartItem> items = await _unitOfWork.CartItem.GetAllByFilterAsync(i => i.ApplicationUserId == userId, ["Product"]);
            foreach (var item in items)
            {
                OrderDetailsView orderDetail = new OrderDetailsView()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Product = item.Product
                };
                orderDetails.Add(orderDetail);
            }
            orderHeaderView.OrderDetails = orderDetails;
            orderHeaderView.OrderTotal = orderHeaderView.OrderDetails.Sum(i => i.TotalPrice);
            return View(orderHeaderView);
        }

        [HttpPost]
        public async Task<IActionResult> Summary(OrderHeaderView orderHeaderView)
        {
            Claim idClaim = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            string userId = idClaim.Value;
            var user = await _userManager.GetUserAsync(User);
            orderHeaderView.ApplicationUserId = userId;
            List<OrderDetailsView> orderDetails = new List<OrderDetailsView>();
            IEnumerable<CartItem> items = await _unitOfWork.CartItem.GetAllByFilterAsync(i => i.ApplicationUserId == userId, ["Product"]);
            foreach (var item in items)
            {
                OrderDetailsView orderDetail = new OrderDetailsView()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Product = item.Product
                };
                orderDetails.Add(orderDetail);
            }
            orderHeaderView.OrderDetails = orderDetails;
            orderHeaderView.OrderTotal = orderHeaderView.OrderDetails.Sum(i => i.TotalPrice);
            if (ModelState.IsValid)
            {
                OrderHeader orderHeader = _mapper.Map<OrderHeader>(orderHeaderView);
                orderHeader.OrderDate = DateTime.Now;
                if (await _userManager.IsInRoleAsync(user, "Company"))
                {
                    orderHeader.OrderStatus = "Approved";
                    orderHeader.PaymentStatus = "Approved for delay payment";

                    await _unitOfWork.OrderHeader.CreateAsync(orderHeader);
                    await _unitOfWork.Save();
                }
                else
                {
                    orderHeader.OrderStatus = "Pending";
                    orderHeader.PaymentStatus = "Pending";
                    string domain = "https://localhost:7158";
                    var options = new SessionCreateOptions
                    {
                        SuccessUrl = domain + $"/Customer/Cart/OrderConfirmation?id={orderHeader.Id}",
                        CancelUrl = domain + "/Customer/Cart/Index",
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment"
                    };
                    foreach (var item in orderHeader.OrderDetails)
                    {
                        
                        var sessionLineItem = new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)item.UnitPrice * 100,
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Product.Title
                                }
                            },
                            Quantity = item.Quantity,
                        };
                        options.LineItems.Add(sessionLineItem);
                    };
                    var service = new SessionService();
                    Session session = service.Create(options);
                    orderHeaderView.SessionId = session.Id;

                    await _unitOfWork.OrderHeader.CreateAsync(orderHeader);

                    await _unitOfWork.Save();

                    Response.Headers.Add("Location", session.Url);
                    return new StatusCodeResult(303);
                }
                return RedirectToAction(nameof(OrderConfirmation), new { id = orderHeader.Id });
            }
            return View(orderHeaderView);
        }
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            OrderHeader orderHeader = await _unitOfWork.OrderHeader.GetByIdAsync(id, ["ApplicationUser"]);
            if (orderHeader.ApplicationUser.CompanyId == 0)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    orderHeader.PaymentIntentId = session.PaymentIntentId;
                    _unitOfWork.OrderHeader.Update(orderHeader);
                }
            }
            IEnumerable<CartItem> items = await _unitOfWork.CartItem.GetAllByFilterAsync(i => i.ApplicationUserId == orderHeader.ApplicationUserId);
            foreach (var item in items)
            {
                _unitOfWork.CartItem.Delete(item.Id);
            }
            await _unitOfWork.Save();
            return View(id); ;
        }
    }
}
