using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class SpreadInfo
    {
        private int channelID;
        private SpreadData data = new SpreadData();
        private string channelName;
        private string pair;

        [JsonPropertyName("channelID")]
        [JsonInclude]
        public int ChannelID
        {
            get { return this.channelID; }
            set { this.channelID = value; }
        }

        public SpreadData Data
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

        public class SpreadData
        {
            public double bidPrice;
            public double askPrice;
            public double timestamp;
            public double bidVolume;
            public double askVolume;
        }
    }
}
