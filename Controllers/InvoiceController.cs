using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GSM.Models;
using GSM.Data;

namespace GSM.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly GSMDbContext _context;
        private readonly StringProcess strPro = new StringProcess();

        public InvoiceController(GSMDbContext context)
        {
            _context = context;
        }

        // GET: Invoice
        public async Task<IActionResult> Index()
        {
            return View(await _context.Invoice.OrderByDescending(m => m.CreateDate).ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(DateTime startTime, DateTime finishTime, string key)
        {
            if(!String.IsNullOrEmpty(key)){
                key = key.Trim();
            }
            var model = await _context.Invoice.ToListAsync();
            if(startTime != null && startTime.ToShortDateString() != "1/1/0001"){
                model = model.Where(m => m.CreateDate >= startTime).ToList();
            }
            if(finishTime != null && finishTime.ToShortDateString() != "1/1/0001"){
                model = model.Where(m => m.CreateDate <= finishTime).ToList();
            }
            if(key!=null){
                model = model.Where(m => m.CustomerName.Contains(key) || m.InvoiceNumber.Contains(key) || m.PhoneNumber.Contains(key)).ToList();
            }
            return View(model);
        }

        // GET: Invoice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceID,CustomerName,InvoiceNumber,Address,PhoneNumber,CreateDate,InvoiceCode,TotalMoney,IsPaid")] Invoice invoice)
        {
            if (id != invoice.InvoiceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .FirstOrDefaultAsync(m => m.InvoiceID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            var listInvD = await _context.InvoiceDetail.Where(m => m.InvoiceID == id).ToListAsync();
            foreach(InvoiceDetail invD in listInvD){
                _context.InvoiceDetail.Remove(invD);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id){
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            ViewBag.informationInvoice = "Hoá đơn: " + invoice.InvoiceNumber + "-" +invoice.CustomerName + "-" + invoice.PhoneNumber + "-" + invoice.TotalMoney.ToString("#") + "VNĐ";
            return View(await _context.InvoiceDetail.Where(m => m.InvoiceID == id).ToListAsync());
        }
        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.InvoiceID == id);
        }
    }
}
