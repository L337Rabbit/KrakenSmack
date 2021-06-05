using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class SubscriptionStatusMessage : IdentifiableMessage
    {
        
        private string channelName;
        private string pair;
        private string status;

        private Subscription subscribeInfo;
        private string errorMessage;
        private int channelID;

        public SubscriptionStatusMessage()
        {
            this.Event = "subscriptionStatus";
        }

        [JsonPropertyName("channelName")]
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ChannelName
        {
            get { return this.channelName; }
            set { this.channelName = value; }
        }

        [JsonPropertyName("pair")]
        [JsonInclude]
        public string Pair
        {
            get { return this.pair; }
            set { this.pair = value; }
        }

        [JsonPropertyName("status")]
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        [JsonPropertyName("subscription")]
        [JsonInclude]
        public Subscription SubscribeInfo
        {
            get { return this.subscribeInfo; }
            set { this.subscribeInfo = value; }
        }

        [JsonPropertyName("errorMessage")]
        [JsonInclude]
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = value; }
        }

        [JsonPropertyName("channelID")]
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ChannelID
        {
            get { return this.channelID; }
            set { this.channelID = value; }
        }
    }
}
