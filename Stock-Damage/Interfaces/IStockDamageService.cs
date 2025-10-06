using Stock_Damage.DTOs;
using Stock_Damage.Models;

namespace Stock_Damage.Interfaces
{
    public interface IStockDamageService
    {
        Task<List<Godown>> GetGodownsAsync();
        Task<List<SubItem_Code>> GetItemsAsync();
        Task<List<Currency>> GetCurrenciesAsync();
        Task<List<Employee>> GetEmployeesAsync();
        Task<ItemDetailsResponse> GetItemDetailsAsync(string subItemCode);
        Task<SaveResponse> SaveStockDamageAsync(List<StockDamageEntry> entries, string createdBy);
    }

}
