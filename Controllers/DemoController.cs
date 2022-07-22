using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GSM.Models;
using GSM.Data;
using System.Collections.Generic;

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
        public JsonResult Index(Product prod)
        {
            string fname = "ProductID: " + prod.ProductID + ", ProductName: " + prod.ProductName;
            return Json(fname);
        }
        [HttpPost]
        public JsonResult ListSubmit(List<Product> list)
        {
            string fname = "";
            if(list.Count==0){
                fname = "Khong nhan duoc du lieu gui len!";
            }
            else{
                foreach( Product item in list)
                {
                    fname += "ProductID: " + item.ProductID + ", ProductName: " + item.ProductName + ";";
                }
            }
            
           return Json(fname);
        }
    }
}