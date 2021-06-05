using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class SubscribeMessage : IdentifiableMessage
    {
        private string[] pairs;

        private Subscription subscription;

        public SubscribeMessage()
        {
            this.Event = "subscribe";
        }

        public SubscribeMessage(Subscription subscription)
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
        public Subscription Subscription
        {
            get { return this.subscription; }
            set { this.subscription = value; }
        }

        public bool IsAuthenticationRequired()
        {
            switch(subscription.SubscriptionType)
            {
                case SubscriptionType.ALL: return true;
                case SubscriptionType.OPEN_ORDERS: return true;
                case SubscriptionType.OWN_TRADES: return true;
                default: return false;
            }
        }
    }
    
}
