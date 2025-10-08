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
        private readonly string? _connectionString;

        public StockDamageService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Godown>> GetGodownsAsync()
        {
            var godowns = new List<Godown>();

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }

            try
            {
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
                                    GodownNo = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    GodownName = reader.IsDBNull(2) ? null : reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error while fetching godowns: " + ex.Message, ex);
            }

            return godowns;
        }

        public async Task<List<SubItem_Code>> GetItemsAsync()
        {
            var items = new List<SubItem_Code>();

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }

            try
            {
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
                                    SubItemCode = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    SubItemName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    Unit = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Weight = reader.IsDBNull(4) ? null : reader.GetDecimal(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error while fetching items: " + ex.Message, ex);
            }

            return items;
        }

        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            var currencies = new List<Currency>();

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }

            try
            {
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
                                    CurrencyName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    ConversionRate = reader.GetDecimal(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception (in a real application, use a logging framework)
                throw new Exception("Database error while fetching currencies: " + ex.Message, ex);
            }

            return currencies;
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            var employees = new List<Employee>();

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }

            try
            {
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
                                    EmployeeCode = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    EmployeeName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    Department = reader.IsDBNull(3) ? null : reader.GetString(3)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception (in a real application, use a logging framework)
                throw new Exception("Database error while fetching employees: " + ex.Message, ex);
            }

            return employees;
        }

        public async Task<ItemDetailsResponse?> GetItemDetailsAsync(string subItemCode)
        {
            ItemDetailsResponse? itemDetails = null;

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }

            if (string.IsNullOrEmpty(subItemCode))
            {
                return null;
            }

            try
            {
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
                                    SubItemCode = reader.IsDBNull(0) ? null : reader.GetString(0),
                                    SubItemName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    Unit = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    Weight = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                                    StockQuantity = reader.GetDecimal(4)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception (in a real application, use a logging framework)
                throw new Exception("Database error while fetching item details: " + ex.Message, ex);
            }

            return itemDetails;
        }

        public async Task<SaveResponse> SaveStockDamageAsync(List<StockDamageEntry> entries, string createdBy)
        {
            var response = new SaveResponse();

            if (string.IsNullOrEmpty(_connectionString))
            {
                response.Status = "Error";
                response.Message = "Connection string is not configured.";
                return response;
            }

            if (entries == null || !entries.Any())
            {
                response.Status = "Error";
                response.Message = "No entries to save.";
                return response;
            }

            try
            {
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
                                response.Status = reader.IsDBNull(0) ? null : reader.GetString(0);
                                response.Message = reader.IsDBNull(1) ? null : reader.GetString(1);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception (in a real application, use a logging framework)
                response.Status = "Error";
                response.Message = "Database error while saving stock damage: " + ex.Message;
            }
            catch (Exception ex)
            {
                // Log the exception (in a real application, use a logging framework)
                response.Status = "Error";
                response.Message = "Error while saving stock damage: " + ex.Message;
            }

            return response;
        }
    }
}