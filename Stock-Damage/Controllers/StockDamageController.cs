using Microsoft.AspNetCore.Mvc;

namespace Stock_Damage.Controllers
{
    public class StockDamageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
