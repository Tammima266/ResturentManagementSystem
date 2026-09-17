using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResturentManagementSystem1.Data;
using ResturentManagementSystem1.Helpers;
using ResturentManagementSystem1.Models;

namespace ResturentManagementSystem1.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        public IActionResult AddToCart(int id)
        {
            var item = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.Id == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = 1,
                    ImageUrl = item.ImageUrl ?? string.Empty
                });
            }
            else
            {
                cartItem.Quantity++;
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.Id == id);

            if (cartItem != null)
            {
                if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                }
                else
                {
                    cart.Remove(cartItem);
                }
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.Id == id);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any()) return RedirectToAction("Index");

            return View(cart);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(string paymentMethod, string deliveryAddress)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Calculate total cost
            decimal totalAmount = cart.Sum(item => item.Price * item.Quantity);

            // Create the Order record
            var order = new Order
            {
                CustomerId = currentUser.Id,
                CustomerName = currentUser.Name ?? currentUser.UserName ?? "Valued Customer",
                DeliveryAddress = !string.IsNullOrEmpty(deliveryAddress) ? deliveryAddress : (currentUser.Address ?? "Not Provided"),
                TotalAmount = totalAmount,
                OrderStatus = "Pending", // Default initial status for Admin tracking
                OrderDate = DateTime.Now,
                OrderItems = cart.Select(ci => new OrderItem
                {
                    ProductName = ci.Name,
                    Price = ci.Price,
                    Quantity = ci.Quantity
                }).ToList()
            };

            // Save order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear shopping cart session after order is placed
            HttpContext.Session.Remove("Cart");

            ViewBag.PaymentMethod = paymentMethod;
            ViewBag.OrderId = order.Id;
            return View("OrderSuccess");
        }
    }
}