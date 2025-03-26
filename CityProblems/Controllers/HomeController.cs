using CityProblems.DataAccess;
using CityProblems.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CityProblems.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CityProblemsDbContext _context;

        public HomeController(ILogger<HomeController> logger, CityProblemsDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var issues = await _context.Issues
                .ToListAsync();

            return View(issues);
        }

        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .ToListAsync();

            return Json(categories);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}