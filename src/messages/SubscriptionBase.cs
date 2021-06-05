using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.okitoki.kraken.extensions;

namespace com.okitoki.kraken.messages
{
    public class SubscriptionBase
    {
        private int depth = default;

        private int interval = default;

        private string name;

        private string token = default;

        public SubscriptionBase() { }

        public SubscriptionBase(SubscriptionType subscriptionType)
        {
            this.SubscriptionType = subscriptionType;
        }

        public string GetNameForSubscriptionType(SubscriptionType subscriptionType)
        {
            switch (subscriptionType)
            {
                case SubscriptionType.BOOK:
                    return "book";
                case SubscriptionType.OHLC:
                    return "ohlc";
                case SubscriptionType.OPEN_ORDERS:
                    return "openOrders";
                case SubscriptionType.OWN_TRADES:
                    return "ownTrades";
                case SubscriptionType.SPREAD:
                    return "spread";
                case SubscriptionType.TICKER:
                    return "ticker";
                case SubscriptionType.TRADE:
                    return "trade";
            }

            return null;
        }

        [JsonPropertyName("depth")]
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Depth
        {
            get { return this.depth; }
            set { this.depth = value; }
        }

        [JsonPropertyName("interval")]
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Interval
        {
            get { return this.interval; }
            set { this.interval = value; }
        }

        [JsonIgnore]
        public SubscriptionType SubscriptionType { get; set; }

        [JsonPropertyName("name")]
        [JsonInclude]
        public string Name
        {
            get { return this.SubscriptionType.StringValue(); }
            set { this.SubscriptionType = value.GetSubscriptionType(); }
        }

        [JsonPropertyName("token")]
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Token
        {
            get { return this.token; }
            set { this.token = value; }
        }
    }
}
