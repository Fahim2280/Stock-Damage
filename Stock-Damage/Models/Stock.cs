namespace Stock_Damage.Models
{
    public class Stock
    {
        public int StockId { get; set; }
        public string? SubItemCode { get; set; }
        public decimal StockQuantity { get; set; }
        public string? GodownNo { get; set; }
    }
}