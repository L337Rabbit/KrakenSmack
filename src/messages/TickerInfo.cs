using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class TickerInfo
    {
        private int channelID;
        private TickerData data = new TickerData();
        private string channelName;
        private string pair;

        [JsonPropertyName("channelID")]
        [JsonInclude]
        public int ChannelID
        {
            get { return this.channelID; }
            set { this.channelID = value; }
        }

        public TickerData Data
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

        public class TickerData
        {
            private Ask askInfo;
            private Bid bidInfo;
            private Close closeInfo;
            private Volume volumeInfo;
            private WeightedVolume volumeWightedPriceInfo;
            private Trades numTradesInfo;
            private LowPrice lowPriceInfo;
            private HighPrice hightPriceInfo;
            private OpenPrice openPriceInfo;

            [JsonPropertyName("")]
            [JsonInclude]
            public Ask Ask
            {
                get { return this.askInfo; }
                set { this.askInfo = value; }
            }

            [JsonPropertyName("")]
            [JsonInclude]
            public Bid Bid
            {
                get { return this.bidInfo; }
                set { this.bidInfo = value; }
            }

            [JsonPropertyName("")]
            [JsonInclude]
            public Close Close
            {
                get { return this.closeInfo; }
                set { this.closeInfo = value; }
            }

            [JsonPropertyName("")]
            [JsonInclude]
            public Volume Volume
            {
                get { return this.volumeInfo; }
                set { this.volumeInfo = value; }
            }

            [JsonPropertyName("")]
            [JsonInclude]
            public WeightedVolume WeightedVolume
            {
                get { return this.volumeWightedPriceInfo; }
                set { this.volumeWightedPriceInfo = value; }
            }

            [JsonPropertyName("")]
            [JsonInclude]
            public Trades Trades
            {
                get { return this.numTradesInfo; }
                set { this.numTradesInfo = value; }
            }

            [JsonPropertyName("")]
            [JsonInclude]
            public LowPrice LowPrice
            {
                get { return this.lowPriceInfo; }
                set { this.lowPriceInfo = value; }
            }

            [JsonPropertyName("")]
            [JsonInclude]
            public HighPrice HighPrice
            {
                get { return this.hightPriceInfo; }
                set { this.hightPriceInfo = value; }
            }

            [JsonPropertyName("")]
            [JsonInclude]
            public OpenPrice OpenPrice
            {
                get { return this.openPriceInfo; }
                set { this.openPriceInfo = value; }
            }
        }

        public class Ask
        {
            public double price;
            public int wholeLotVolume;
            public double lotVolume;
        }

        public class Bid
        {
            public double price;
            public int wholeLotVolume;
            public double lotVolume;
        }

        public class Close
        {
            public double price;
            public double lotVolume;
        }

        public class Volume
        {
            public double volumeToday;
            public double volumeLast24Hours;
        }

        public class WeightedVolume
        {
            public double volumeToday;
            public double volumeLast24Hours;
        }

        public class Trades
        {
            public int numTradesToday;
            public int numTradesLast24Hours;
        }

        public class LowPrice
        {
            public double lowToday;
            public double lowLast24Hours;
        }

        public class HighPrice
        {
            public double highToday;
            public double highLast24Hours;
        }

        public class OpenPrice
        {
            public double openPriceToday;
            public double openPriceLast24Hours;
        }
    }
}
