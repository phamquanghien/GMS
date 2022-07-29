using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System;
namespace GSM.Models{
    [Table("Categories")]
    public class Category{
        [Key]
        public int CategoryID { get; set; }
        [DisplayName("% Vàng")]
        public string CategoryName { get; set; }
        [DisplayName("Tổng trọng lượng")]
        public double TotalWeight { get; set; }
        [DisplayName("Đơn giá")]
        public decimal UnitPrice { get; set; }
        [DisplayName("Thành tiền")]
        public decimal IntoMoney { get; set; }
        
    }
}