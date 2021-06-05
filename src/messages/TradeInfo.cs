using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class TradeInfo
    {
        private int channelID;
        private List<TradeData> data = new List<TradeData>();
        private string channelName;
        private string pair;

        [JsonPropertyName("channelID")]
        [JsonInclude]
        public int ChannelID
        {
            get { return this.channelID; }
            set { this.channelID = value; }
        }

        public List<TradeData> Data
        {
            get { return this.data; }
            set { this.data = value; }
        }

        [JsonPropertyName("")]
        [JsonInclude]
        public string ChannelName
        {
            get { return this.channelName; }
            set { this.channelName = value; }
        }

        [JsonPropertyName("")]
        [JsonInclude]
        public string Pair
        {
            get { return this.pair; }
            set { this.pair = value; }
        }

        public class TradeData
        {
            public double price;
            public double volume;
            public double time;
            public string side;
            public string orderType;
            public string misc;
        }
    }
}
