using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System;
namespace GSM.Models{
    [Table("Products")]
    public class Product{
        [Key]
        public int ProductID { get; set; }
        [DisplayName("Tên hàng")]
        public string ProductName { get; set; }
    }
}