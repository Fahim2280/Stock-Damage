using System.ComponentModel.DataAnnotations;

namespace Stock_Damage.DTOs
{
    public class StockDamageEntry
    {
        public string? GodownNo { get; set; }
        public string? GodownName { get; set; }
        public string? SubItemCode { get; set; }
        public string? SubItemName { get; set; }
        public string? Unit { get; set; }
        public decimal Stock { get; set; }
        public string BatchNo { get; set; } = "NA";

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
        public decimal Rate { get; set; }

        [Required]
        public decimal AmountIn { get; set; }

        public string? CurrencyName { get; set; }
        public decimal ConversionRate { get; set; }
        public string DrAcHead { get; set; } = "Stock Damage";
        public string? EmployeeName { get; set; }
        public string? Comments { get; set; }
    }
}