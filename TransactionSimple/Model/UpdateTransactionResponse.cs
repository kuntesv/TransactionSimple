using System.Text.Json.Serialization;

namespace TransactionSimple.Model
{
    public class UpdateTransactionResponse : UpdateTransactionRequest
    {
        [JsonIgnore]
        public int IsActive { get; set; }
    }
}
