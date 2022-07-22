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
            return View(await _context.Invoice.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Find(DateTime startTime, DateTime finishTime, string key)
        {
            if(!String.IsNullOrEmpty(key)){
                key = key.Trim();
            }
            var model = await _context.Invoice.ToListAsync();
            if(startTime != null){
                model = model.Where(m => m.CreateDate >= startTime).ToList();
            }
            if(finishTime != null){
                model = model.Where(m => m.CreateDate <= finishTime).ToList();
            }
            if(key!=null){
                model = model.Where(m => m.CustomerName.Contains(key) || m.InvoiceNumber.Contains(key) || m.PhoneNumber.Contains(key)).ToList();
            }
            return View(model);
        }

        // GET: Invoice/Create
        public IActionResult Create()
        {
            //tra ve danh sach Invoice trong csdl
            var model = _context.Invoice.ToList();
            //neu chua co du lieu => Ma Invoice = "N001"
            if(model.Count() == 0) ViewBag.InVoiceKey = "N001";
            //neu co du liẹu trong csdl
            else {
                //lay ra ban ghi moi nhat cua Invoice
                var newKey = model.OrderByDescending(m => m.InvoiceNumber).FirstOrDefault().InvoiceNumber;
                //su dung ViewBag de tra ve du lieu ma Invoice tu sinh
                ViewBag.InVoiceKey = strPro.GenerateKey(newKey);
            }
            return View();
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceID,CustomerName,InvoiceNumber,Address,PhoneNumber,CreateDate,InvoiceCode,TotalMoney,IsPaid")] Invoice invoice)
        {
            invoice.CreateDate = DateTime.Now;
            invoice.TotalMoney = 0;
            invoice.IsPaid = false;
            invoice.InvoiceCode = null;
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
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
