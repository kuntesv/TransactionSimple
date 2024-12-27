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
        public IActionResult AddTransactions([FromBody] AddtransactionsRequest addTransactionRequest)
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
                return StatusCode(500, new ApiResponse(500, "An unexpected error occurred." + ex.Message));
            }

            // Check if any rows were affected
            if (rowsAffected <= 0)
            {
                return BadRequest(new ApiResponse(400, "No records were inserted."));
            }

            return CreatedAtAction(nameof(AddTransactions), new { id = addTransactionRequest.Item }, "Transaction created successfully.");
        }

        [HttpGet("getAllTransactions")]
        public IActionResult GetAllTransactions([FromQuery] String? sortProperty = null)
        {
            List<GetAllTransactionResponse> outputRecords;

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

        private static void ValidateInput(AddtransactionsRequest addtransactionsRequest)
        {
            if (addtransactionsRequest == null)
            {
                throw new ArgumentNullException(nameof(addtransactionsRequest), "The transaction request cannot be null.");
            }

            if (string.IsNullOrEmpty(addtransactionsRequest.Category))
            {
                throw new ArgumentException("Category is required.", nameof(addtransactionsRequest.Category));
            }

            if (string.IsNullOrEmpty(addtransactionsRequest.Item))
            {
                throw new ArgumentException("Item is required.", nameof(addtransactionsRequest.Item));
            }

            if (addtransactionsRequest.Price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(addtransactionsRequest.Price), "Price cannot be negative.");
            }
        }



    }
}
