using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GSM.Models;
using GSM.Data;
using OfficeOpenXml;
using System.IO;
using System.Collections.Generic;
using OfficeOpenXml.Style;

namespace GSM.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly GSMDbContext _context;
        private StringProcess strPro = new StringProcess();

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
                    //invoiceDetail[i].InvoiceID = null;
                    invoiceDetail[i].InvoiceDetailID = i+1;
                }
                
                //create InvoicePrint list
                List<InvoicePrint> list = new List<InvoicePrint>();
                // Add value to InvoicePrint list
                for (var i = 0; i < invoiceDetail.Count; i++)
                {
                    InvoicePrint invPrint = new InvoicePrint();
                    invPrint.InvoiceDetailID = "     " + (i+1);
                    invPrint.ProductName = invoiceDetail[i].ProductName;
                    invPrint.SumWeight = invoiceDetail[i].SumWeight;
                    invPrint.PercentGold = invoiceDetail[i].PercentGold;
                    invPrint.GoldWeight = invoiceDetail[i].GoldWeight;
                    invPrint.GoldUnitPrice = invoiceDetail[i].GoldUnitPrice;
                    invPrint.GemstoneWeight = invoiceDetail[i].GemstoneWeight;
                    invPrint.GemstoneUnitPrice = invoiceDetail[i].GemstoneUnitPrice;
                    invPrint.CraftingWages = invoiceDetail[i].CraftingWages;
                    invPrint.IntoMoney = invoiceDetail[i].IntoMoney;
                    list.Add(invPrint);
                }
                //customer name
                worksheet.Cells["D3"].Value = invoice.CustomerName;
                worksheet.Cells["D3:H3"].Merge = true;
                //Invoice number
                worksheet.Cells["J3"].Value = invoice.InvoiceNumber;
                //address
                worksheet.Cells["B4"].Value = "                  " + invoice.Address; 
                worksheet.Cells["B4:G4"].Merge = true;
                //phone number
                worksheet.Cells["J4"].Value = invoice.PhoneNumber;
                //Create date
                worksheet.Cells["B5"].Value = invoice.CreateDate.Day + "              ";
                worksheet.Cells["C5"].Value = invoice.CreateDate.Month;
                worksheet.Cells["E5"].Value = invoice.CreateDate.Year;
                worksheet.Cells["E5:F5"].Merge = true;
                worksheet.Cells["H5"].Value = invoice.CreateDate.Hour + ":" + invoice.CreateDate.Minute;
                //Invoice detail
                //worksheet.Cells["A9"].LoadFromCollection(invoiceDetail);
                worksheet.Cells["A9"].LoadFromCollection(list);
                //set Width of column
                worksheet.Column(1).Width = 5;
                worksheet.Column(2).Width = 22;
                worksheet.Column(3).Width = 6;
                worksheet.Column(4).Width = 6;
                worksheet.Column(5).Width = 7;
                worksheet.Column(7).Width = 7;
                worksheet.Column(9).Width = 7;
                worksheet.Column(10).Width = 12;
                //set Height of row
                worksheet.Row(2).Height = 20;
                worksheet.Row(4).Height = 21;
                worksheet.Row(5).Height = 21;
                worksheet.Row(9).Height = 20;
                worksheet.Row(10).Height = 20;
                worksheet.Row(11).Height = 21;
                worksheet.Row(12).Height = 21;
                //set Horizontal Alignment
                worksheet.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Row(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Row(10).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Row(11).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Row(12).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["D9:D12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["I9:I12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["B5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["C5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["E5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["H5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //set Style text
                worksheet.Cells["C9:C12"].Style.Font.Bold = true;
                worksheet.Cells["E9:E12"].Style.Font.Bold = true;
                worksheet.Cells["J9:J12"].Style.Font.Bold = true;
                worksheet.Cells["F9:F12"].Style.Font.Size = 8;
                worksheet.Cells["H9:H12"].Style.Font.Size = 8;
                worksheet.Cells["I9:I12"].Style.Font.Size = 8;
                //set WrapText
                worksheet.Column(2).Style.WrapText = true;
                //format number
                worksheet.Cells["F9:F14"].Style.Numberformat.Format = "#,##0";
                worksheet.Cells["H9:J14"].Style.Numberformat.Format = "#,##0";
                worksheet.Cells["B13"].Style.Numberformat.Format = "#,##0";
                //set page layout
                worksheet.PrinterSettings.RightMargin = 0M;
                //get total money
                worksheet.Cells["B13:C13"].Merge = true;
                worksheet.Cells["B13"].Value = invoice.TotalMoney;
                worksheet.Cells["B13"].Style.Font.Bold = true;
                worksheet.Row(13).Height = 25;
                //get total money text
                worksheet.Cells["C14:J14"].Merge = true;
                string textMoney = strPro.NumberToText(Convert.ToDouble(invoice.TotalMoney));
                textMoney = strPro.CapitalizeFirstLetter(textMoney);
                worksheet.Cells["C14"].Value = textMoney;
                worksheet.Cells["C14"].Style.Font.Bold = true;
                worksheet.Row(14).Height = 25;
                //set name of store owner
                worksheet.Cells["H20"].Value = "Nguyễn Văn Ngọc    ";
                worksheet.Cells["H20:J20"].Merge = true;
                worksheet.Cells["H20"].Style.Font.Bold = true;
                worksheet.Cells["H20"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["H20"].Style.Font.Name = "Apple Color Emoji";

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
