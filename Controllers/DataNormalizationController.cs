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
    public class DataNormalizationController : Controller
    {
        private readonly GSMDbContext _context;
        public DataNormalizationController(GSMDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> CategoryDataNormalization()
        {
            //get list of ImportInvoiceDetail
            var impInvDetail = _context.ImportInvoiceDetail.ToList();
            //get list of InvoiceDetail
            var invDetail = _context.InvoiceDetail.ToList();
            //get distinct all CategoryID in ImportInvoiceDetail
            var impInvDetailID = _context.ImportInvoiceDetail.Select(m => m.CategoryID).Distinct().ToList();
            if(impInvDetail.Count==0 & impInvDetailID.Count==0){
                var category = _context.Category.ToList();
                foreach (var item in category)
                {
                    item.TotalWeight = 0;
                    _context.UpdateRange(category);
                    _context.SaveChanges();
                }
            }
            else{
                for (var i = 0; i < impInvDetailID.Count; i++)
                {
                    var sumIn = impInvDetail.Where(m => m.CategoryID == impInvDetailID[i]).Sum(m => m.SumWeight);
                    var sumOut = invDetail.Where(m => m.CategoryID == impInvDetailID[i]).Sum(m => m.GoldWeight);
                    var category = _context.Category.Find(impInvDetailID[i]);
                    category.TotalWeight = sumIn - sumOut;
                    _context.Update(category);
                    
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index","Category");
        }
        public async Task<IActionResult> CategoryInputDataNormalization(int importInvoiceID)
        {
            //get list of ImportInvoiceDetail
            var impInvDetail = _context.ImportInvoiceDetail.Where(m => m.ImportInvoiceID == importInvoiceID).ToList();
            //get list of Category
            var categoryWeight = _context.Category.ToList();
            //get distinct all CategoryID in ImportInvoiceDetail
            var impInvDetailID = _context.ImportInvoiceDetail.Select(m => m.CategoryID).Distinct().ToList();
            
            for (var i = 0; i < impInvDetailID.Count; i++)
            {
                var sumIn = impInvDetail.Where(m => m.CategoryID == impInvDetailID[i]).Sum(m => m.SumWeight);
                var sumNow = categoryWeight.Where(m => m.CategoryID == impInvDetailID[i]).Sum(m => m.TotalWeight);
                var category = _context.Category.Find(impInvDetailID[i]);
                category.TotalWeight = sumIn + sumNow;
                _context.Update(category);
                _context.SaveChanges();
            }
            return RedirectToAction("Index","Category");
        }
        public async Task<IActionResult> CategoryOutputDataNormalization(int invoiceID)
        {
            //get list of ImportInvoiceDetail
            var invDetail = _context.InvoiceDetail.Where(m => m.InvoiceID == invoiceID).ToList();
            //get list of Category
            var categoryWeight = _context.Category.ToList();
            //get distinct all CategoryID in ImportInvoiceDetail
            var impInvDetailID = _context.ImportInvoiceDetail.Select(m => m.CategoryID).Distinct().ToList();
            
            for (var i = 0; i < impInvDetailID.Count; i++)
            {
                var sumOut = invDetail.Where(m => m.CategoryID == impInvDetailID[i]).Sum(m => m.GoldWeight);
                var sumNow = categoryWeight.Where(m => m.CategoryID == impInvDetailID[i]).Sum(m => m.TotalWeight);
                var category = _context.Category.Find(impInvDetailID[i]);
                category.TotalWeight = sumNow - sumOut;
                _context.Update(category);
                _context.SaveChanges();
            }
            return RedirectToAction("Index","Category");
        }
    }
}