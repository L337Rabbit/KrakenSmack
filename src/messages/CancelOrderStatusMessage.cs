using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class CancelOrderStatusMessage : IdentifiableMessage
    {
        public CancelOrderStatusMessage() { }

        [JsonPropertyName("status")]
        [JsonInclude]
        public string Status { get; set; }

        [JsonPropertyName("errorMessage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string ErrorMessage { get; set; }
    }
}
