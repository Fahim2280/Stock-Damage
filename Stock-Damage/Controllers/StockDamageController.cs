using Microsoft.AspNetCore.Mvc;
using Stock_Damage.DTOs;
using Stock_Damage.Interfaces;

namespace Stock_Damage.Controllers
{
    public class StockDamageController : Controller
    {
        private readonly IStockDamageService _stockDamageService;

        public StockDamageController(IStockDamageService stockDamageService)
        {
            _stockDamageService = stockDamageService;
        }

        // GET: StockDamage/Index
        public async Task<IActionResult> Index()
        {
            var viewModel = new StockDamageViewModel
            {
                Godowns = await _stockDamageService.GetGodownsAsync(),
                Items = await _stockDamageService.GetItemsAsync(),
                Currencies = await _stockDamageService.GetCurrenciesAsync(),
                Employees = await _stockDamageService.GetEmployeesAsync()
            };

            return View(viewModel);
        }

        // AJAX: Get Item Details
        [HttpGet]
        public async Task<IActionResult> GetItemDetails(string? subItemCode)
        {
            if (string.IsNullOrEmpty(subItemCode))
            {
                return BadRequest(new { success = false, message = "Item code is required" });
            }

            var itemDetails = await _stockDamageService.GetItemDetailsAsync(subItemCode);

            if (itemDetails == null)
            {
                return NotFound(new { success = false, message = "Item not found" });
            }

            return Json(new { success = true, data = itemDetails });
        }

        // AJAX: Save Stock Damage
        [HttpPost]
        public async Task<IActionResult> SaveStockDamage([FromBody] List<StockDamageEntry>? entries)
        {
            if (entries == null || !entries.Any())
            {
                return BadRequest(new { success = false, message = "No entries to save" });
            }

            // Validate all entries
            foreach (var entry in entries)
            {
                if (entry.Quantity <= 0 || entry.Rate <= 0)
                {
                    return BadRequest(new { success = false, message = "Invalid quantity or rate" });
                }
            }

            // You can get the current user from HttpContext if using authentication
            string? currentUser = User?.Identity?.Name ?? "System";

            var result = await _stockDamageService.SaveStockDamageAsync(entries, currentUser ?? "System");

            if (result.Status == "Success")
            {
                return Json(new { success = true, message = result.Message });
            }
            else
            {
                return BadRequest(new { success = false, message = result.Message });
            }
        }
    }
}