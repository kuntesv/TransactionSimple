namespace TransactionSimple.Model
{
    public class ApiResponse
    {
        public int StatusCode { get; }
        public String StatusMessage { get; }

        public ApiResponse(int code , String message)
        {
            StatusCode = code;
            StatusMessage = message;
        }
    }
}
