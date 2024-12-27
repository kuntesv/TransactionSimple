using System.Data.SqlClient;
using TransactionSimple.Model;

namespace TransactionSimple.Service
{
    public class TransactionService
    {
        public String _connectionString;

        public TransactionService(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("LocalSql");
        }

        public int AddRecord(AddTransactionRequest addTransactionRequest)
        {
            int rowsAffected = 0;
            try
            {
                // Use 'using' statement to ensure proper disposal of resources
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    // Prepare the SQL command with parameters
                    SqlCommand cmd = new SqlCommand("INSERT INTO TRecords (Price, Category, Item, IsActive) VALUES (@Price, @Category, @Item, @IsActive)", con);

                    // Add parameters to the command
                    cmd.Parameters.AddWithValue("@Price", addTransactionRequest.Price);
                    cmd.Parameters.AddWithValue("@Category", addTransactionRequest.Category);
                    cmd.Parameters.AddWithValue("@Item", addTransactionRequest.Item);
                    cmd.Parameters.AddWithValue("@IsActive", 1); // Assuming 1 means active

                    // Open the connection and execute the command
                    con.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                // Handle other exceptions
                throw;
            }

            return rowsAffected;
        }

        public int GetAllRecord(AddTransactionRequest addTransactionRequest)
        {
            int rowsAffected = 0;
            try
            {
                // Use 'using' statement to ensure proper disposal of resources
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    // Prepare the SQL command with parameters
                    SqlCommand cmd = new SqlCommand("INSERT INTO TRecords (Price, Category, Item, IsActive) VALUES (@Price, @Category, @Item, @IsActive)", con);

                    // Add parameters to the command
                    cmd.Parameters.AddWithValue("@Price", addTransactionRequest.Price);
                    cmd.Parameters.AddWithValue("@Category", addTransactionRequest.Category);
                    cmd.Parameters.AddWithValue("@Item", addTransactionRequest.Item);
                    cmd.Parameters.AddWithValue("@IsActive", 1); // Assuming 1 means active

                    // Open the connection and execute the command
                    con.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                // Handle other exceptions
                throw;
            }

            return rowsAffected;
        }

        public List<TransactionRecord> GetAllRecords(string sortProperty)
        {
            List<TransactionRecord> outputRecords = new List<TransactionRecord>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Price, Category, Item, IsActive FROM TRecords", con);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var record = new TransactionRecord
                        {
                            Price = (int)Convert.ToInt64(reader["Price"]),
                            Category = reader["Category"].ToString(),
                            Item = reader["Item"].ToString(),
                            IsActive = (int)reader["IsActive"]
                        };
                        outputRecords.Add(record);
                    }
                }
                con.Close();
            }

            if (sortProperty != null)
            {
                if (sortProperty.ToLower() == "price")
                {
                    outputRecords = outputRecords.OrderBy(record => record.Price).ToList();
                }
                else if (sortProperty.ToLower() == "category")
                {
                    outputRecords = outputRecords.OrderBy(record => record.Category).ToList();
                }
            }

            return outputRecords;
        }
    }
}
