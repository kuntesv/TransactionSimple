namespace TransactionSimple.Model
{
    public class UpdateTransactionRequest
    {
        public int Id { get; set; }

        public int Price { get; set; }
        
        public string Category { get; set; }

        public string Item { get; set; }


    }
}
