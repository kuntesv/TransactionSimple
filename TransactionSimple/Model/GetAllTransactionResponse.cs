using System.Text.Json.Serialization;

namespace TransactionSimple.Model
{
    public class GetAllTransactionResponse
    {
        public int Price { get; set; }
        public string Category { get; set; }
        public string Item { get; set; }

        [JsonIgnore]
        public int IsActive { get; set; }
    }
}
