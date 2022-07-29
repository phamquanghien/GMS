using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using System;
namespace GSM.Models{
    [Table("ImportInvoice")]
    public class ImportInvoice{
        [Key]
        [DisplayName("STT")]
        public int ImportInvoiceID { get; set; }
        [DisplayName("Ngày")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CreateDate { get; set; }
        [DisplayName("Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0}")]
        public decimal TotalMoney { get; set; }
        public ICollection<ImportInvoiceDetail> ImportInvoiceDetails { get; set; }
    }
}