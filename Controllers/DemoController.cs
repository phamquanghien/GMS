using Microsoft.AspNetCore.Mvc;
using GSM.Data;
using OfficeOpenXml;
using System.IO;
using System;
using System.Linq;

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
        public IActionResult PrintInvoice()
        {
            using(ExcelPackage excelPackage = new ExcelPackage())
            {
                //create a WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                //create a new list with books
                var invoiceDetail = _context.InvoiceDetail.ToList();
                worksheet.Cells["A1"].Value = "Thông tin hoá đơn";
                worksheet.Cells["A3"].LoadFromCollection(invoiceDetail);
                var stream = new MemoryStream(excelPackage.GetAsByteArray()); //Get updated stream
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Demo.xlsx");
            }
        }
    }
}