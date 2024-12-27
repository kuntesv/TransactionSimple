using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TransactionSimple.Model;
using TransactionSimple.Service;

namespace TransactionSimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        // String connectionString = "Data Source=DI2INPUN1870WNB;Initial Catalog=transactionDb;Integrated Security=True;MultipleActiveResultSets=True";

        private TransactionService transactionService;

        public TransactionController(IConfiguration configuration)
        {
            transactionService = new TransactionService(configuration);
        }

        [HttpPost("addtransactions")]
        public IActionResult AddTransactions([FromBody] AddTransactionRequest addTransactionRequest)
        {
            // Validate the incoming request
            int rowsAffected = 0;
            try
            {
                ValidateInput(addTransactionRequest);

                rowsAffected = transactionService.AddRecord(addTransactionRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, "An unexpected error occurred."));
            }

            // Check if any rows were affected
            if (rowsAffected <= 0)
            {
                return BadRequest(new ApiResponse(400, "No records were inserted."));
            }

            return CreatedAtAction(nameof(AddTransactions), new { id = addTransactionRequest.Item }, "Transaction created successfully.");
        }

        [HttpGet("getAllTransactions")]
        public IActionResult GetAllTransactions([FromQuery] String sortProperty = null)
        {
            List<TransactionRecord> outputRecords;

            try
            {
                outputRecords = transactionService.GetAllRecords(sortProperty);

                return Ok(outputRecords); // Return 200 OK with the list of records
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error retrieving data from the database: " + ex.Message);
            }
        }

        private static void ValidateInput(AddTransactionRequest addTransactionRequest)
        {
            if (addTransactionRequest == null ||
                string.IsNullOrEmpty(addTransactionRequest.Category) ||
                string.IsNullOrEmpty(addTransactionRequest.Item) ||
                addTransactionRequest.Price < 0) // Assuming Price should not be negative
            {
                throw new Exception($"Invalid input please check constraints.", new ArgumentException());
            }
        }


    }
}
