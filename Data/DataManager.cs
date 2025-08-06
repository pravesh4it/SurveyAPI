using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ABC.Data
{
    public class DataManager
    {
        private readonly IConfiguration _configuration;

        public DataManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Get the connection string from appsettings.json
        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("PDRDb");
        }

        // Get DataTable asynchronously from stored procedure
        public async Task<DataTable> GetDataTableAsync(string sp_Name, Dictionary<string, string> objGetDataTable)
        {
            string constr = GetConnectionString();
            DataTable dt = new DataTable();

            // Use async/await for better async programming
            using (SqlConnection sqlCon = new SqlConnection(constr))
            {
                await sqlCon.OpenAsync(); // Ensure connection is opened asynchronously

                using (SqlCommand sqlComd = new SqlCommand(sp_Name, sqlCon))
                {
                    sqlComd.CommandType = CommandType.StoredProcedure;

                    // Add parameters dynamically from the dictionary
                    foreach (KeyValuePair<string, string> kv in objGetDataTable)
                    {
                        // Use Add with parameter type to avoid potential issues
                        sqlComd.Parameters.AddWithValue(kv.Key, kv.Value);
                    }

                    // Using SqlDataAdapter to fill the DataTable asynchronously
                    using (SqlDataAdapter sqlAdpt = new SqlDataAdapter(sqlComd))
                    {
                        await Task.Run(() => sqlAdpt.Fill(dt)); // Asynchronously fill the DataTable
                    }
                }
            }
            return dt;
        }
    }
}
