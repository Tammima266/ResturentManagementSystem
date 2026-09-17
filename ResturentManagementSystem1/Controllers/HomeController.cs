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

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch categories and combos to display on the menu homepage
            var categories = await _context.Categories.ToListAsync();
            var combos = await _context.Combos.ToListAsync();

            ViewBag.Combos = combos;
            return View(categories);
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