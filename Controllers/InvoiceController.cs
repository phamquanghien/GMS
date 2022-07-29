using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GSM.Models;
using GSM.Data;
using OfficeOpenXml;
using System.IO;

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
            ViewBag.countInvD = _context.InvoiceDetail.Count();
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
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id){
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            ViewBag.informationInvoice = "Hoá đơn: " + invoice.InvoiceNumber + "-" +invoice.CustomerName + "-" + invoice.PhoneNumber + "-" + string.Format("{0:0,0}", invoice.TotalMoney) + " VNĐ";
            return View(await _context.InvoiceDetail.Where(m => m.InvoiceID == id).ToListAsync());
        }
        public IActionResult PrintInvoice(int invoiceID)
        {
            if(!InvoiceExists(invoiceID)){
                return NotFound();
            }
            //get InvoiceNumber
            var invoice = _context.Invoice.Find(invoiceID);
            //set name of file return
            var fileName = invoice.InvoiceNumber + ".xlsx";
            using(ExcelPackage excelPackage = new ExcelPackage())
            {
                //create a WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                //create a new list with books
                var invoiceDetail = _context.InvoiceDetail.Where(m => m.InvoiceID == invoiceID).ToList();
                for (var i = 0; i < invoiceDetail.Count; i++)
                {
                    invoiceDetail[i].Invoice = null;
                    invoiceDetail[i].InvoiceID = null;
                    invoiceDetail[i].InvoiceDetailID = i+1;
                }
                //customer name
                worksheet.Cells["A1"].Value = invoice.CustomerName;
                worksheet.Cells["A1:E1"].Merge = true;
                //Invoice number
                worksheet.Cells["G1"].Value = invoice.InvoiceNumber;
                //address
                worksheet.Cells["A2"].Value = invoice.Address; 
                worksheet.Cells["A2:E2"].Merge = true;
                //phone number
                worksheet.Cells["G2"].Value = invoice.PhoneNumber;
                //Create date
                worksheet.Cells["A3"].Value = invoice.CreateDate.Day;
                worksheet.Cells["C3"].Value = invoice.CreateDate.Month;
                worksheet.Cells["E3"].Value = invoice.CreateDate.Year;
                //Invoice detail
                worksheet.Cells["A5"].LoadFromCollection(invoiceDetail);
                //set Width of column
                worksheet.Column(3).Width = 25;
                worksheet.Column(11).Width = 25;
                //set Height of row
                worksheet.Row(3).Height = 50;
                //format number
                worksheet.Cells["D5:K6"].Style.Numberformat.Format = "#,##0";
                var stream = new MemoryStream(excelPackage.GetAsByteArray()); //Get updated stream
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.InvoiceID == id);
        }
    }
}
