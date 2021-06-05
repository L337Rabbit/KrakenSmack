using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class CancelAllOrdersAfterTimeMessage : CancelAllOrdersMessage
    {
        public CancelAllOrdersAfterTimeMessage(int delayInSeconds)
        {
            this.Event = "cancelAllOrdersAfter";
            this.Timeout = delayInSeconds;
        }

        [JsonPropertyName("timeout")]
        [JsonInclude]
        public int Timeout { get; set; }
    }
}
