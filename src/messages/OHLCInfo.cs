using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class OHLCInfo
    {
        private int channelID;
        private OHLCData data = new OHLCData();
        private string channelName;
        private string pair;

        [JsonPropertyName("channelID")]
        [JsonInclude]
        public int ChannelID
        {
            get { return this.channelID; }
            set { this.channelID = value; }
        }

        public OHLCData Data
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

        public class OHLCData
        {
            public double beginTime;
            public double endTime;
            public double openPrice;
            public double highPrice;
            public double lowPrice;
            public double closePrice;
            public double volumeWeightedAveragePrice;
            public double volume;
            public double tradeCount;
        }
    }
}
