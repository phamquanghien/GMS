using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
namespace GSM.Models{
    [Table("ImportInvoiceDetails")]
    public class ImportInvoiceDetail{
        [Key]
        public int InvoiceDetailID { get; set; }
        public int? ImportInvoiceID { get; set; }
        public int CategoryID { get; set; }
        [DisplayName("% Vàng")]
        public string CategoryName { get; set; }
        [DisplayName("Tổng trọng lượng")]
        public double SumWeight { get; set; }
        
        [DisplayName("Chú thích")]
        public string Note { get; set; }
        [DisplayName("Đơn giá vàng")]
        public decimal GoldUnitPrice { get; set; }
        [DisplayName("Thành tiền")]
        public decimal IntoMoney { get; set; }
        public ImportInvoice ImportInvoice { get; set; }
    }
}