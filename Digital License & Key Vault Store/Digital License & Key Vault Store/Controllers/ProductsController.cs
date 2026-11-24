using Digital_License___Key_Vault_Store.Data;
using Digital_License___Key_Vault_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Digital_License___Key_Vault_Store.Controllers
{
    public class ProductsController(KeyVaultDbContext _context) : Controller
    {

        public async Task<IActionResult> Index(string? category, string? search)
        {
            var query = _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category.Slug == category);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            var products = await query
                .OrderBy(p => p.Name)
                .Select(p => new ProductsModel
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Slug = p.Slug,
                    CategoryName = p.Category.Name,
                    Price = p.Price,
                    Description = p.Description ?? string.Empty,
                    AvailableKeys = p.LicenseKeys.Count(k => k.Status == "Available")
                })
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
            ViewBag.ActiveCategory = category;
            ViewBag.SearchTerm = search;

            return View(products);
        }

        [Route("store/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive && p.Slug == slug)
                .Select(p => new ProductsModel
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Slug = p.Slug,
                    CategoryName = p.Category.Name,
                    Price = p.Price,
                    Description = p.Description ?? string.Empty,
                    AvailableKeys = p.LicenseKeys.Count(k => k.Status == "Available")
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
