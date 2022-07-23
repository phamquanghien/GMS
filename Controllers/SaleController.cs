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
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create(Invoice invoice, List<InvoiceDetail> listInv){
        //     //save information of invoice
        //     _context.Add(invoice);
        //     await _context.SaveChangesAsync();
        //     //get InvoiceId
        //     var invID = _context.Invoice.OrderByDescending(m => m.InvoiceID).FirstOrDefault().InvoiceID;
        //     //save information of invoice detail by InvoiceID
        //     foreach(InvoiceDetail invD in listInv)
        //     {
        //         invD.InvoiceID = invID;
        //         _context.Add(invD);
        //     }
        //     await _context.SaveChangesAsync();
        //     //update IsPaid of Invoice = True
        //     var inv = await _context.Invoice.FindAsync(invID);
        //     if(inv!=null){
        //         inv.IsPaid = true;
        //         _context.Update(inv);
        //         await _context.SaveChangesAsync();
        //     }
        //     return RedirectToAction("Index","Invoice");
        // }
        [HttpPost]
        public IActionResult Create(Invoice invoice, List<InvoiceDetail> listInv){
            //save information of invoice
            invoice.CreateDate = DateTime.Now;
            _context.Add(invoice);
            _context.SaveChangesAsync();
            //get InvoiceId
            var invID = _context.Invoice.OrderByDescending(m => m.InvoiceID).FirstOrDefault().InvoiceID;
            //save information of invoice detail by InvoiceID
            foreach(InvoiceDetail invD in listInv)
            {
                invD.InvoiceID = invID;
                _context.Add(invD);
            }
            _context.SaveChangesAsync();
            //update IsPaid of Invoice = True
            var inv = _context.Invoice.Find(invID);
            if(inv!=null){
                inv.IsPaid = true;
                _context.Update(inv);
                _context.SaveChangesAsync();
            }
            //return RedirectToAction("Index","Invoice");
            return Json("Tạo mới hoá đơn thành công.");
        }

        // [HttpPost]
        // public JsonResult Create(Invoice invoice, List<InvoiceDetail> listInv){
        //     var mess = "";
        //     mess+= invoice.CustomerName + "-" + invoice.InvoiceNumber + "-" + invoice.Address + "-" + invoice.PhoneNumber + "-" + invoice.CreateDate + "-" + invoice.InvoiceCode + "-" + invoice.TotalMoney + "-" + invoice.IsPaid + ";\n";
        //     foreach(InvoiceDetail invD in listInv)
        //     {
        //         mess+= invD.ProductName + "-" + invD.SumWeight + "-" + invD.PercentGold + "-" + invD.GoldWeight + "-" + invD.GoldUnitPrice + "-" + invD.GemstoneWeight + "-" + invD.GemstoneUnitPrice + "-" + invD.CraftingWages + "-" + invD.IntoMoney;
        //     }
        //     return Json(mess);
        // }
    }
}