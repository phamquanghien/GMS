using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using System;
namespace GSM.Models{
    [Table("Customers")]
    public class Customer{
        [Key]
        public int CustomerID { get; set; }
        [DisplayName("Họ tên khách hàng (Customer's Name)")]
        public string CustomerName { get; set; }
        [DisplayName("Địa chỉ (Address)")]
        public string Address { get; set; }
        [DisplayName("Điện thoại")]
        public string PhoneNumber { get; set; }
    }
}