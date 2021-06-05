using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public abstract class EventMessage
    {
        private string eventType = "subscribe";

        [JsonPropertyName("event")]
        [JsonInclude]
        public string Event
        {
            get { return this.eventType; }
            set { this.eventType = value; }
        }
    }
}
