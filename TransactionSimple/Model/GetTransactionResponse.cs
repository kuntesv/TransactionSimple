using System.Text.Json.Serialization;

namespace TransactionSimple.Model
{
    public class GetTransactionResponse
    {
        public int Id {  get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string Item { get; set; }

        [JsonIgnore]
        public int IsActive { get; set; }
    }
}
