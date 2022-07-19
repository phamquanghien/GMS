using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GSM.Models;
using GSM.Data;
namespace GSM.Controllers
{
    public class SaleController : Controller
    {
        private readonly GSMDbContext _context;
        private readonly StringProcess strPro = new StringProcess();

        public SaleController(GSMDbContext context)
        {
            _context = context;
        }
        public IActionResult Create(){
            var model = _context.Invoice.ToList();
            if(model.Count() == 0) ViewBag.InVoiceKey = "N001";
            else {
                var latestKey = model.OrderByDescending(m => m.InvoiceNumber).FirstOrDefault().InvoiceNumber;
                ViewBag.InVoiceKey = strPro.GenerateKey(latestKey);
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductName");

            var listProduct = _context.Product.Select(m => m.ProductName).ToArray();
            ViewData["ListProduct"] = listProduct;

            var listPro = _context.Product.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Invoice inv){
            ///// gui du lieu san pham
            var model = _context.Invoice.ToList();
            if(model.Count() == 0) ViewBag.InVoiceKey = "N001";
            else {
                var latestKey = model.OrderByDescending(m => m.InvoiceNumber).FirstOrDefault().InvoiceNumber;
                ViewBag.InVoiceKey = strPro.GenerateKey(latestKey);
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductName");

            var listProduct = _context.Product.Select(m => m.ProductName).ToArray();
            ViewData["ListProduct"] = listProduct;

            var listPro = _context.Product.ToList();
            /////

            inv.CreateDate = DateTime.Now;
            ViewBag.mess = inv.CustomerName + "-" + inv.InvoiceNumber + "-" + inv.Address + "-" + inv.PhoneNumber + "-" + inv.TotalMoney;
            return View();
        }
        public IActionResult Add()
        {
            var model = _context.Invoice.ToList();
            if(model.Count() == 0) ViewBag.InVoiceKey = "N001";
            else {
                var latestKey = model.OrderByDescending(m => m.InvoiceNumber).FirstOrDefault().InvoiceNumber;
                ViewBag.InVoiceKey = strPro.GenerateKey(latestKey);
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductName");

            var listProduct = _context.Product.Select(m => m.ProductName).ToArray();
            ViewData["ListProduct"] = listProduct;

            var listPro = _context.Product.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Invoice inv, List<InvoiceDetail> list){
            /////
            var model = _context.Invoice.ToList();
            if(model.Count() == 0) ViewBag.InVoiceKey = "N001";
            else {
                var latestKey = model.OrderByDescending(m => m.InvoiceNumber).FirstOrDefault().InvoiceNumber;
                ViewBag.InVoiceKey = strPro.GenerateKey(latestKey);
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductName");

            var listProduct = _context.Product.Select(m => m.ProductName).ToArray();
            ViewData["ListProduct"] = listProduct;

            var listPro = _context.Product.ToList();
            /////
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddList(List<InvoiceDetail> listInvD){
            return Json(new {Success = true});
        }
    }
}