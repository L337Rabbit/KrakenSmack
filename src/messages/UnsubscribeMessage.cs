using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class UnsubscribeMessage : IdentifiableMessage
    {
        private string[] pairs;

        private SubscriptionBase subscription;

        public UnsubscribeMessage()
        {
            this.Event = "unsubscribe";
        }

        public UnsubscribeMessage(SubscriptionBase subscription)
        {
            this.Subscription = subscription;
        }

        [JsonPropertyName("pair")]
        [JsonInclude]
        public string[] Pairs
        {
            get { return this.pairs; }
            set { this.pairs = value; }
        }

        [JsonPropertyName("subscription")]
        [JsonInclude]
        public SubscriptionBase Subscription
        {
            get { return this.subscription; }
            set { this.subscription = value; }
        }

        public bool IsAuthenticationRequired()
        {
            switch (subscription.SubscriptionType)
            {
                case SubscriptionType.ALL: return true;
                case SubscriptionType.OPEN_ORDERS: return true;
                case SubscriptionType.OWN_TRADES: return true;
                default: return false;
            }
        }
    }
}
