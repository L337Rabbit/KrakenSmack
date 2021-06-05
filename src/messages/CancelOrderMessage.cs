using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class CancelOrderMessage : AuthenticatedMessage
    {
        public CancelOrderMessage(string[] transactionIds)
        {
            this.Event = "cancelOrder";

            if(transactionIds == null || transactionIds.Length == 0)
            {
                throw new Exception("At least one transaction ID must be supplied in order to cancel an order.");
            }

            this.TransactionIDs = transactionIds;
        }

        [JsonPropertyName("txid")]
        [JsonInclude]
        public string[] TransactionIDs { get; set; }
    }
}
