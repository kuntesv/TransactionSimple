namespace TransactionSimple.Model
{
    public class AddTransactionRequest
    {
        public int Price { get; set; }

        public string Category { get; set; }

        public string Item { get; set; }
    }
}
