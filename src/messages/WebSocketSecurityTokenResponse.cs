using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class WebSocketSecurityTokenResponse
    {
        [JsonPropertyName("error")]
        [JsonInclude]
        public string[] Error { get; set; }

        [JsonPropertyName("result")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public WebSocketToken Result { get; set; }
    }
}
