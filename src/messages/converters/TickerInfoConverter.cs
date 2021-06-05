using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using com.okitoki.kraken.utils;

namespace com.okitoki.kraken.messages.converters
{
    public class TickerInfoConverter : JsonConverter<TickerInfo>
    {
        public override TickerInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TickerInfo ti = new TickerInfo();

            JsonDocument doc = JsonDocument.ParseValue(ref reader);
            JsonElement.ArrayEnumerator arrayEnum = doc.RootElement.EnumerateArray();
            ti.ChannelID = JsonUtils.ReadInt(ref arrayEnum);

            arrayEnum.MoveNext();
            JsonElement.ObjectEnumerator objEnum = arrayEnum.Current.EnumerateObject();

            //Read data fields...
            objEnum.MoveNext();
            ti.Data.Ask = ReadAsk(objEnum.Current.Value);

            objEnum.MoveNext();
            ti.Data.Bid = ReadBid(objEnum.Current.Value);

            objEnum.MoveNext();
            ti.Data.Close = ReadClose(objEnum.Current.Value);

            objEnum.MoveNext();
            ti.Data.Volume = ReadVolume(objEnum.Current.Value);

            objEnum.MoveNext();
            ti.Data.WeightedVolume = ReadWeightedVolume(objEnum.Current.Value);

            objEnum.MoveNext();
            ti.Data.Trades = ReadTrades(objEnum.Current.Value);

            objEnum.MoveNext();
            ti.Data.LowPrice = ReadLowPrice(objEnum.Current.Value);

            objEnum.MoveNext();
            ti.Data.HighPrice = ReadHighPrice(objEnum.Current.Value);

            objEnum.MoveNext();
            ti.Data.OpenPrice = ReadOpenPrice(objEnum.Current.Value);
            ti.ChannelName = JsonUtils.ReadString(ref arrayEnum);
            ti.Pair = JsonUtils.ReadString(ref arrayEnum);

            return ti;
        }

        private TickerInfo.Ask ReadAsk(JsonElement element)
        {
            JsonElement.ArrayEnumerator valArrayEnum = element.EnumerateArray();

            TickerInfo.Ask ask = new TickerInfo.Ask();
            ask.price = JsonUtils.ReadDouble(ref valArrayEnum);
            ask.wholeLotVolume = JsonUtils.ReadInt(ref valArrayEnum);
            ask.lotVolume = JsonUtils.ReadDouble(ref valArrayEnum);
            return ask;
        }

        private TickerInfo.Bid ReadBid(JsonElement element)
        {
            JsonElement.ArrayEnumerator valArrayEnum = element.EnumerateArray();

            TickerInfo.Bid bid = new TickerInfo.Bid();
            bid.price = JsonUtils.ReadDouble(ref valArrayEnum);
            bid.wholeLotVolume = JsonUtils.ReadInt(ref valArrayEnum);
            bid.lotVolume = JsonUtils.ReadDouble(ref valArrayEnum);
            return bid;
        }

        private TickerInfo.Close ReadClose(JsonElement element)
        {
            JsonElement.ArrayEnumerator valArrayEnum = element.EnumerateArray();

            TickerInfo.Close close = new TickerInfo.Close();
            close.price = JsonUtils.ReadDouble(ref valArrayEnum);
            close.lotVolume = JsonUtils.ReadDouble(ref valArrayEnum);
            return close;
        }

        private TickerInfo.Volume ReadVolume(JsonElement element)
        {
            JsonElement.ArrayEnumerator valArrayEnum = element.EnumerateArray();

            TickerInfo.Volume volume = new TickerInfo.Volume();
            volume.volumeToday = JsonUtils.ReadDouble(ref valArrayEnum);
            volume.volumeLast24Hours = JsonUtils.ReadDouble(ref valArrayEnum);
            return volume;
        }

        private TickerInfo.WeightedVolume ReadWeightedVolume(JsonElement element)
        {
            JsonElement.ArrayEnumerator valArrayEnum = element.EnumerateArray();

            TickerInfo.WeightedVolume weightedVolume = new TickerInfo.WeightedVolume();
            weightedVolume.volumeToday = JsonUtils.ReadDouble(ref valArrayEnum);
            weightedVolume.volumeLast24Hours = JsonUtils.ReadDouble(ref valArrayEnum);
            return weightedVolume;
        }

        private TickerInfo.Trades ReadTrades(JsonElement element)
        {
            JsonElement.ArrayEnumerator valArrayEnum = element.EnumerateArray();

            TickerInfo.Trades trades = new TickerInfo.Trades();
            trades.numTradesToday = JsonUtils.ReadInt(ref valArrayEnum);
            trades.numTradesLast24Hours = JsonUtils.ReadInt(ref valArrayEnum);
            return trades;
        }

        private TickerInfo.LowPrice ReadLowPrice(JsonElement element)
        {
            JsonElement.ArrayEnumerator valArrayEnum = element.EnumerateArray();

            TickerInfo.LowPrice lowPrice = new TickerInfo.LowPrice();
            lowPrice.lowToday = JsonUtils.ReadDouble(ref valArrayEnum);
            lowPrice.lowLast24Hours = JsonUtils.ReadDouble(ref valArrayEnum);
            return lowPrice;
        }

        private TickerInfo.HighPrice ReadHighPrice(JsonElement element)
        {
            JsonElement.ArrayEnumerator valArrayEnum = element.EnumerateArray();

            TickerInfo.HighPrice highPrice = new TickerInfo.HighPrice();
            highPrice.highToday = JsonUtils.ReadDouble(ref valArrayEnum);
            highPrice.highLast24Hours = JsonUtils.ReadDouble(ref valArrayEnum);
            return highPrice;
        }

        private TickerInfo.OpenPrice ReadOpenPrice(JsonElement element)
        {
            JsonElement.ArrayEnumerator valArrayEnum = element.EnumerateArray();

            TickerInfo.OpenPrice openPrice = new TickerInfo.OpenPrice();
            openPrice.openPriceToday = JsonUtils.ReadDouble(ref valArrayEnum);
            openPrice.openPriceLast24Hours = JsonUtils.ReadDouble(ref valArrayEnum);
            return openPrice;
        }

        public override void Write(Utf8JsonWriter writer, TickerInfo value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
