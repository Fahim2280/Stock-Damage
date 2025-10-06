using Stock_Damage.Models;

namespace Stock_Damage.DTOs
{
    public class StockDamageViewModel
    {
        public List<Godown> Godowns { get; set; } = new List<Godown>();
        public List<SubItem_Code> Items { get; set; } = new List<SubItem_Code>();
        public List<Currency> Currencies { get; set; } = new List<Currency>();
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public StockDamageEntry CurrentEntry { get; set; } = new StockDamageEntry();
    }
}
