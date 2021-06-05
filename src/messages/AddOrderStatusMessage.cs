using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class AddOrderStatusMessage : IdentifiableMessage
    {
        public AddOrderStatusMessage() { }

        [JsonPropertyName("status")]
        [JsonInclude]
        public string Status { get; set; }

        [JsonPropertyName("txid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string TransactionID { get; set; }

        [JsonPropertyName("descr")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string Description { get; set; }

        [JsonPropertyName("errorMessage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string ErrorMessage { get; set; }
    }
}
