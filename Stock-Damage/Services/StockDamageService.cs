using Newtonsoft.Json;
using Stock_Damage.DTOs;
using Stock_Damage.Interfaces;
using Stock_Damage.Models;
using System.Data;
using System.Data.SqlClient;

namespace Stock_Damage.Services
{
    public class StockDamageService : IStockDamageService
    {
        private readonly string _connectionString;

        public StockDamageService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Godown>> GetGodownsAsync()
        {
            var godowns = new List<Godown>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SP_GetGodowns", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            godowns.Add(new Godown
                            {
                                AutoSlNo = reader.GetInt32(0),
                                GodownNo = reader.GetString(1),
                                GodownName = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return godowns;
        }

        public async Task<List<SubItem_Code>> GetItemsAsync()
        {
            var items = new List<SubItem_Code>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SP_GetItems", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            items.Add(new SubItem_Code
                            {
                                AutoSlNo = reader.GetInt32(0),
                                SubItemCode = reader.GetString(1),
                                SubItemName = reader.GetString(2),
                                Unit = reader.GetString(3),
                                Weight = reader.IsDBNull(4) ? null : reader.GetDecimal(4)
                            });
                        }
                    }
                }
            }

            return items;
        }

        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            var currencies = new List<Currency>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SP_GetCurrencies", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            currencies.Add(new Currency
                            {
                                CurrencyId = reader.GetInt32(0),
                                CurrencyName = reader.GetString(1),
                                ConversionRate = reader.GetDecimal(2)
                            });
                        }
                    }
                }
            }

            return currencies;
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            var employees = new List<Employee>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SP_GetEmployees", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeId = reader.GetInt32(0),
                                EmployeeCode = reader.GetString(1),
                                EmployeeName = reader.GetString(2),
                                Department = reader.IsDBNull(3) ? null : reader.GetString(3)
                            });
                        }
                    }
                }
            }

            return employees;
        }

        public async Task<ItemDetailsResponse> GetItemDetailsAsync(string subItemCode)
        {
            ItemDetailsResponse itemDetails = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SP_GetItemDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SubItemCode", subItemCode);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            itemDetails = new ItemDetailsResponse
                            {
                                SubItemCode = reader.GetString(0),
                                SubItemName = reader.GetString(1),
                                Unit = reader.GetString(2),
                                Weight = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                                StockQuantity = reader.GetDecimal(4)
                            };
                        }
                    }
                }
            }

            return itemDetails;
        }

        public async Task<SaveResponse> SaveStockDamageAsync(List<StockDamageEntry> entries, string createdBy)
        {
            var response = new SaveResponse();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("SP_StockDamage_Save", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Convert entries to JSON
                    string jsonData = JsonConvert.SerializeObject(entries);
                    command.Parameters.AddWithValue("@StockDamageData", jsonData);
                    command.Parameters.AddWithValue("@CreatedBy", createdBy ?? "System");

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            response.Status = reader.GetString(0);
                            response.Message = reader.GetString(1);
                        }
                    }
                }
            }

            return response;
        }
    }
}
