using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public abstract class IdentifiableMessage : EventMessage
    {
        private int requestId = default;

        [JsonPropertyName("reqid")]
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int RequestId
        {
            get { return this.requestId; }
            set { this.requestId = value; }
        }
    }
}
