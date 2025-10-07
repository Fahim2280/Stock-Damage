namespace Stock_Damage.DTOs
{
    public class ItemDetailsResponse
    {
        public string? SubItemCode { get; set; }
        public string? SubItemName { get; set; }
        public string? Unit { get; set; }
        public decimal? Weight { get; set; }
        public decimal StockQuantity { get; set; }
    }
}