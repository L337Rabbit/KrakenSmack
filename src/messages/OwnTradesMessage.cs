using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okitoki.kraken.messages
{
    public class OwnTradesMessage
    {
        public Dictionary<string, OwnTradesData> trades = new Dictionary<string, OwnTradesData>();

        public string ChannelName { get; set; }
        public int SequenceNumber { get; set; }

        public class OwnTradesData
        {
            public string OrderTransactionID { get; set; }

            public string PositionTransactionID { get; set; }

            public string Pair { get; set; }

            public double Timestamp { get; set; }

            public string Type { get; set; }

            public string OrderType { get; set; }

            public double Price { get; set; }

            public double Cost { get; set; }

            public double Fee { get; set; }

            public double Volume { get; set; }

            public double Margin { get; set; }

            public int UserReferenceID { get; set; }
        }
    }
}
