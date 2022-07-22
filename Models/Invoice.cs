using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using System;
namespace GSM.Models{
    [Table("Invoice")]
    public class Invoice{
        [Key]
        [DisplayName("STT")]
        public int InvoiceID { get; set; }
        [DisplayName("Họ tên khách hàng")]
        public string CustomerName { get; set; }
        [DisplayName("Số hoá đơn")]
        public string InvoiceNumber { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        [DisplayName("Điện thoại")]
        public string PhoneNumber { get; set; }
        [DisplayName("Ngày")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CreateDate { get; set; }
        [DisplayName("Mã số")]
        public string InvoiceCode { get; set; }
        [DisplayName("Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0}")]
        public decimal TotalMoney { get; set; }
        public bool IsPaid { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}