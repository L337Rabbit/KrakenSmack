using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class BookInfo
    {
        private int channelID;
        private BookData data = new BookData();
        private string channelName;
        private string pair;

        [JsonPropertyName("channelID")]
        [JsonInclude]
        public int ChannelID
        {
            get { return this.channelID; }
            set { this.channelID = value; }
        }

        public BookData Data
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

        public class BookData
        {
            public List<PriceLevel> askPriceLevels = new List<PriceLevel>();
            public List<PriceLevel> bidPriceLevels = new List<PriceLevel>();
        }

        public class PriceLevel
        {
            public double price;
            public double volume;
            public double timestamp;
        }
    }
}
