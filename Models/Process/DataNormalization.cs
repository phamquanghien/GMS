using System.Linq;
using GSM.Data;

namespace GSM.Models
{
    public class DataNormalization
    {
        private GSMDbContext _context;
        public DataNormalization()
        {
        }
        public DataNormalization(GSMDbContext context)
        {
            _context = context;
        }
        public void CategoryDataNormalization()
        {
            //get list of ImportInvoiceDetail
            var impInvDetail = _context.ImportInvoiceDetail.ToList();
            //get list of InvoiceDetail
            var invDetail = _context.InvoiceDetail.ToList();
            //get distinct all CategoryID in ImportInvoiceDetail
            var impInvDetailID = _context.ImportInvoiceDetail.Select(m => m.CategoryID).Distinct().ToList();
            
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
        public void CategoryInputDataNormalization(int importInvoiceID)
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
        }
        public void CategoryOutputDataNormalization(int invoiceID)
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
        }
    }   
}