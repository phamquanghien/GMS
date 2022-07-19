using GSM.Models;
using Microsoft.EntityFrameworkCore;
namespace GSM.Data{
    public class GSMDbContext : DbContext
    {
        public GSMDbContext (DbContextOptions<GSMDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }

        public DbSet<Invoice> Invoice { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetail { get; set; }
    }
}