using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class Subscription : SubscriptionBase
    {
        private bool rateCounter = default;

        private bool snapshot = default;

        [JsonPropertyName("ratecounter")]
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool RateCounter
        {
            get { return this.rateCounter; }
            set { this.rateCounter = value; }
        }

        [JsonPropertyName("snapshot")]
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Snapshot
        {
            get { return this.snapshot; }
            set { this.snapshot = value; }
        }
    }
}
