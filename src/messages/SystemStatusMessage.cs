using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class SystemStatusMessage : EventMessage
    {
        private long connectionID;
        private string status;
        private string version;

        public SystemStatusMessage()
        {
            this.Event = "systemStatus";
        }

        [JsonPropertyName("connectionID")]
        [JsonInclude]
        public long ConnectionID
        {
            get { return this.connectionID; }
            set { this.connectionID = value; }
        }

        [JsonPropertyName("status")]
        [JsonIgnore]
        public string Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        [JsonPropertyName("version")]
        [JsonIgnore]
        public string Version
        {
            get { return this.version; }
            set { this.version = value; }
        }
    }
}
