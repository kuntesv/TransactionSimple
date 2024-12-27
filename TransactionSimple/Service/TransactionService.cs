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

        public int AddTransaction(AddtransactionsRequest addTransactionRequest)
        { 
          return AddRecord(addTransactionRequest);
        }

        public List<GetTransactionResponse> GetAllTransactionRecords(string sortProperty)
        {
            return GetAllRecords(sortProperty);
        }

        public UpdateTransactionResponse UpdateTransaction(UpdateTransactionRequest updateTransactionRequest)
        {

            GetTransactionResponse getTransactionsResponse = GetSingleRecordById(updateTransactionRequest.Id);

            if (getTransactionsResponse == null)
            {
                throw new ArgumentNullException(nameof(UpdateTransaction), $"The record {updateTransactionRequest.Id} not found in database.");
            }

            return UpdateSingleRecordById(updateTransactionRequest);
        }

        public bool DeleteTransaction(int id)
        {
            return DeleteRecordById(id);
        }

        private int AddRecord(AddtransactionsRequest addTransactionRequest)
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

        private List<GetTransactionResponse> GetAllRecords(string sortProperty)
        {
            List<GetTransactionResponse> outputRecords = new List<GetTransactionResponse>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TRecords", con);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var record = new GetTransactionResponse
                        {
                            Id = (int)Convert.ToInt64(reader["ID"]),
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

        private GetTransactionResponse GetSingleRecordById(int id)
        {
            const string fetchByIdStatement = "SELECT * FROM TRecords WHERE ID = @Id";

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(fetchByIdStatement, con))
            {
                // Use parameterized queries to prevent SQL injection
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new GetTransactionResponse
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ID")),
                            Price = reader.GetInt32(reader.GetOrdinal("Price")),
                            Category = reader.GetString(reader.GetOrdinal("Category")),
                            Item = reader.GetString(reader.GetOrdinal("Item")),
                            IsActive = reader.GetInt32(reader.GetOrdinal("IsActive"))
                        };
                    }
                }
            }

            // Return null or a default instance if no record is found
            return null;
        }


        private UpdateTransactionResponse UpdateSingleRecordById(UpdateTransactionRequest updateTransactionRequest)
        {
            const string updateStatement = "UPDATE TRecords SET Price = @Price, Category = @Category, Item = @Item WHERE ID = @Id";
            int rowsAffected = 0;
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(updateStatement, con))
            {
                // Use parameterized queries to prevent SQL injection
                cmd.Parameters.AddWithValue("@ID", updateTransactionRequest.Id);
                cmd.Parameters.AddWithValue("@Price", updateTransactionRequest.Price);
                cmd.Parameters.AddWithValue("@Category", updateTransactionRequest.Category);
                cmd.Parameters.AddWithValue("@Item", updateTransactionRequest.Item);

                con.Open();

                // Execute the command and return true if at least one row was affected
                rowsAffected = cmd.ExecuteNonQuery();
                con.Close();
            }

            if (rowsAffected > 0)
            {
                GetTransactionResponse getTransactionResponse = GetSingleRecordById(updateTransactionRequest.Id);
                return new UpdateTransactionResponse
                {
                    Id = getTransactionResponse.Id,
                    Price = getTransactionResponse.Price,
                    Category = getTransactionResponse.Category,
                    Item = getTransactionResponse.Item,
                    IsActive = getTransactionResponse.IsActive
                };
            }
            return null;
        }

        private bool DeleteRecordById(int transactionId)
        {
            const string deleteStatement = "DELETE FROM TRecords WHERE ID = @Id";

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(deleteStatement, con))
            {
                // Use parameterized queries to prevent SQL injection
                cmd.Parameters.AddWithValue("@Id", transactionId);

                con.Open();

                // Execute the delete command and return true if at least one row was affected
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        private bool PurgeRecordById(int id)
        {
            const string deleteStatement = "DELETE FROM TRecords WHERE ID = @Id";

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(deleteStatement, con))
            {
                // Use parameterized queries to prevent SQL injection
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();

                // Execute the delete command and return true if at least one row was affected
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

    }
}
