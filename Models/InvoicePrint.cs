using System.ComponentModel;
namespace GSM.Models{
    public class InvoicePrint{
        public string InvoiceDetailID { get; set; }
        [DisplayName("Tên hàng")]
        public string ProductName { get; set; }
        [DisplayName("Tổng trọng lượng")]
        public double SumWeight { get; set; }
        [DisplayName("% vàng")]
        public string PercentGold { get; set; }
        [DisplayName("Trọng lượng vàng")]
        public double GoldWeight { get; set; }
        [DisplayName("Đơn giá vàng")]
        public decimal GoldUnitPrice { get; set; }
        [DisplayName("Trọng lượng đá")]
        public double GemstoneWeight { get; set; }
        [DisplayName("Đơn giá đá")]
        public decimal GemstoneUnitPrice { get; set; }
        [DisplayName("Công chế tác")]
        public decimal CraftingWages  { get; set; }
        [DisplayName("Thành tiền")]
        public decimal IntoMoney { get; set; }
    }
}