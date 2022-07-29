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
    public class ImpInvoiceDetailController : Controller
    {
        private readonly GSMDbContext _context;
        private readonly StringProcess strPro = new StringProcess();

        public ImpInvoiceDetailController(GSMDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.ImportInvoiceDetail.ToListAsync());
        }
        
    }
}