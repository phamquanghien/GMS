using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GSM.Models;
using GSM.Data;
using Microsoft.EntityFrameworkCore;

namespace GSM.Controllers
{
    public class ImportInvoiceController : Controller
    {
        private readonly GSMDbContext _context;
        private readonly StringProcess strPro = new StringProcess();

        public ImportInvoiceController(GSMDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.ImportInvoice.ToListAsync());
        }
        public IActionResult Create(){
            
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryName");
            var listPro = _context.Product.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(List<ImportInvoiceDetail> listInv){
            //save information of ImportInvoice
            ImportInvoice impInvoice = new ImportInvoice();
            impInvoice.CreateDate = DateTime.Now;
            impInvoice.TotalMoney = 0;
            _context.Add(impInvoice);
            _context.SaveChangesAsync();
            //get ImportInvoiceId
            var impInvID = _context.ImportInvoice.OrderByDescending(m => m.ImportInvoiceID).FirstOrDefault().ImportInvoiceID;
            //save information of invoice detail by InvoiceID
            foreach(ImportInvoiceDetail invD in listInv)
            {
                invD.ImportInvoiceID = impInvID;
                _context.Add(invD);
            }
            _context.SaveChangesAsync();
            //get list ImportInvoiceDetail by ImportInvoiceID
            var impInvDList = _context.ImportInvoiceDetail.Where(m => m.ImportInvoiceID == impInvID).ToList();
            decimal totalMoney = 0;
            for (var i = 0; i < impInvDList.Count; i++)
            {
                var impInvDetail = _context.ImportInvoiceDetail.Find(impInvDList[i].InvoiceDetailID);
                impInvDetail.IntoMoney = Convert.ToDecimal(impInvDetail.SumWeight) * impInvDetail.GoldUnitPrice;
                totalMoney += impInvDetail.IntoMoney;
                _context.Update(impInvDetail);
                _context.SaveChanges();
            }
            //update TotalMoney of ImportInvoice
            var impInv = _context.ImportInvoice.Find(impInvID);
            impInv.TotalMoney = totalMoney;
            _context.Update(impInv);
            _context.SaveChanges();
            //update TotalWeight to Category
            for (var i = 0; i < impInvDList.Count; i++)
            {
                var sumWeight = impInvDList[i].SumWeight;
                var categoryId = impInvDList[i].CategoryID;
                var category = _context.Category.Find(categoryId);
                category.TotalWeight += sumWeight;
                _context.Update(category);
                _context.SaveChanges(); 
            }
            return Json("Tạo mới hoá đơn thành công.");
        }
    }
}