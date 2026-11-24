using Digital_License___Key_Vault_Store.Data;
using Microsoft.AspNetCore.Mvc;

namespace Digital_License___Key_Vault_Store.Controllers
{
    public class ProductsController(KeyVaultDbContext _context) : Controller
    {
        
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            var orders = _context.Orders.ToList();
            var orderitems= _context.OrderItems.ToList();
            var products= _context.Products.ToList();
           

            return View();
        }

        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        


    }
}
