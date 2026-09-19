using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResturentManagementSystem1.Data;
using ResturentManagementSystem1.Models;

namespace ResturentManagementSystem1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin Dashboard / Manage Menu Items & Delivered Sales
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            var combos = await _context.Combos.ToListAsync();

            // Calculate total sales ONLY from orders with status "Delivered"
            decimal totalSales = await _context.Orders
                .Where(o => o.OrderStatus == "Delivered")
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            ViewBag.TotalSales = totalSales;
            ViewBag.Combos = combos;

            return View(categories);
        }

        // GET: Create Category Item
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST: Create Category Item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Delete Category Item
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Delete Category Item Confirmed
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // Live Order Tracking & Management Actions
        // ==========================================

        // GET: Admin/Orders
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

        // POST: Admin/UpdateOrderStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                // Updates the status in SQL Server (Pending, Preparing, Ready, Delivered, Cancelled)
                order.OrderStatus = status;
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Orders));
        }
    }
}