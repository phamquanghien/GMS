using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
namespace GSM.Models
{
    public class Sale
    {
        [DisplayName("Họ tên khách hàng")]
        public string CustomerName { get; set; }
        [DisplayName("Số")]
        public string InvoiceNumber { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        [DisplayName("Điện thoại")]
        public string PhoneNumber { get; set; }
        [DisplayName("Ngày")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CreateDate { get; set; }
    }
}