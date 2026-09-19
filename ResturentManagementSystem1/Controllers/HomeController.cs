using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturentManagementSystem1.Data;
using ResturentManagementSystem1.Models;
using System.Diagnostics;

namespace ResturentManagementSystem1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch categories and combos to display on the menu homepage
            var categories = await _context.Categories.ToListAsync();
            var combos = await _context.Combos.ToListAsync();

            ViewBag.Combos = combos;
            return View(categories);
        }

        // ==========================================
        // Customer Live Order Tracking Action (by ID)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> TrackOrder(int id)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found with ID #" + id;
                return RedirectToAction(nameof(Index));
            }

            return View(order); // Looks for Views/Home/TrackOrder.cshtml
        }

        // ==========================================
        // Dynamically Track User's Latest Order
        // ==========================================
        [Authorize]
        public async Task<IActionResult> TrackMyLatestOrder()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Find the latest order placed by this specific user
            var latestOrder = await _context.Orders
                .Where(o => o.CustomerId == currentUser.Id)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();

            if (latestOrder == null)
            {
                TempData["ErrorMessage"] = "You haven't placed any orders yet!";
                return RedirectToAction(nameof(Index));
            }

            // Redirect to the regular TrackOrder action using their actual latest ID
            return RedirectToAction(nameof(TrackOrder), new { id = latestOrder.Id });
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