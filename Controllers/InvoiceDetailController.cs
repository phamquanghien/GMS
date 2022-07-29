using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GSM.Models;
using GSM.Data;

namespace GSM.Controllers
{
    public class InvoiceDetailController : Controller
    {
        private readonly GSMDbContext _context;

        public InvoiceDetailController(GSMDbContext context)
        {
            _context = context;
        }

        // GET: InvoiceDetail/Create
        public IActionResult Create()
        {
            ViewData["InvoiceID"] = new SelectList(_context.Invoice, "InvoiceID", "InvoiceID");
            return View();
        }

        // POST: InvoiceDetail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceDetailID,InvoiceID,ProductName,SumWeight,PercentGold,GoldWeight,GoldUnitPrice,GemstoneWeight,GemstoneUnitPrice,CraftingWages,IntoMoney")] InvoiceDetail invoiceDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Invoice");
            }
            ViewData["InvoiceID"] = new SelectList(_context.Invoice, "InvoiceID", "InvoiceID", invoiceDetail.InvoiceID);
            return View(invoiceDetail);
        }

        // GET: InvoiceDetail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetail.FindAsync(id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }
            ViewBag.invNumber = _context.Invoice.Where(m => m.InvoiceID == invoiceDetail.InvoiceID).First().InvoiceNumber;
            return View(invoiceDetail);
        }

        // POST: InvoiceDetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceDetailID,InvoiceID,ProductName,SumWeight,PercentGold,GoldWeight,GoldUnitPrice,GemstoneWeight,GemstoneUnitPrice,CraftingWages,IntoMoney")] InvoiceDetail invoiceDetail)
        {
            if (id != invoiceDetail.InvoiceDetailID)
            {
                return NotFound();
            }
            var invID = _context.InvoiceDetail.Find(id).InvoiceID;
            if(invID!=invoiceDetail.InvoiceDetailID)
            {
                return NotFound();
            }
            try{
                // var IntoMoney = GoldWeight * GoldUnitPrice + GemstoneWeight * GemstoneUnitPrice + CraftingWages;
                invoiceDetail.IntoMoney = invoiceDetail.CraftingWages + Convert.ToDecimal(invoiceDetail.GoldWeight * Convert.ToDouble(invoiceDetail.GoldUnitPrice) + invoiceDetail.GemstoneWeight * Convert.ToDouble(invoiceDetail.GemstoneUnitPrice));
            }
            catch{
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceDetailExists(invoiceDetail.InvoiceDetailID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Detail","Invoice", invoiceDetail.InvoiceID);
            }
            return View(invoiceDetail);
        }

        // GET: InvoiceDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetail
                .Include(i => i.Invoice)
                .FirstOrDefaultAsync(m => m.InvoiceDetailID == id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // POST: InvoiceDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceDetail = await _context.InvoiceDetail.FindAsync(id);
            _context.InvoiceDetail.Remove(invoiceDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Invoice");
        }

        private bool InvoiceDetailExists(int id)
        {
            return _context.InvoiceDetail.Any(e => e.InvoiceDetailID == id);
        }
    }
}
