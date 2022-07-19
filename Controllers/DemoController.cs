using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GSM.Models;
using GSM.Data;

namespace GSM.Controllers
{
    public class DemoController : Controller
    {
        private readonly GSMDbContext _context;
        public DemoController(GSMDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        // [HttpPost]
        // public async Task<IActionResult> Index(Invoice data)
        // {
        //     _context.Add(data);
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }
        [HttpPost]
        public JsonResult Index(string firstName)
        {
            string name = string.Format("Name: {0} ", firstName); ;
            return Json(new { Status = "success", Name = name });
        }
    }
}