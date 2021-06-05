using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class CancelAllOrdersAfterTimeStatusMessage : IdentifiableMessage
    {
        [JsonPropertyName("status")]
        [JsonInclude]
        public string Status { get; set; }

        [JsonPropertyName("currentTime")]
        [JsonInclude]
        public string RequestProcessedTime { get; set; }

        [JsonPropertyName("triggerTime")]
        [JsonInclude]
        public string CancellationTime { get; set; }

        [JsonPropertyName("errorMessage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string ErrorMessage { get; set; }
    }
}
