using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.okitoki.kraken.utils;

namespace com.okitoki.kraken.messages.converters
{
    public class TradeInfoConverter : JsonConverter<TradeInfo>
    {
        public override TradeInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TradeInfo info = new TradeInfo();

            JsonDocument doc = JsonDocument.ParseValue(ref reader);

            JsonElement.ArrayEnumerator arrayEnum = doc.RootElement.EnumerateArray();

            info.ChannelID = JsonUtils.ReadInt(ref arrayEnum);

            arrayEnum.MoveNext();
            JsonElement.ArrayEnumerator tradeDataArray = arrayEnum.Current.EnumerateArray();

            while(tradeDataArray.MoveNext())
            {
                TradeInfo.TradeData tradeData = new TradeInfo.TradeData();
                JsonElement.ArrayEnumerator dataValuesArray = tradeDataArray.Current.EnumerateArray();

                tradeData.price = JsonUtils.ReadDouble(ref dataValuesArray);
                tradeData.volume = JsonUtils.ReadDouble(ref dataValuesArray);
                tradeData.time = JsonUtils.ReadDouble(ref dataValuesArray);
                tradeData.side = JsonUtils.ReadString(ref dataValuesArray);
                tradeData.orderType = JsonUtils.ReadString(ref dataValuesArray);
                tradeData.misc = JsonUtils.ReadString(ref dataValuesArray);

                info.Data.Add(tradeData);
            }

            info.ChannelName = JsonUtils.ReadString(ref arrayEnum);
            info.Pair = JsonUtils.ReadString(ref arrayEnum);

            return info;
        }

        public override void Write(Utf8JsonWriter writer, TradeInfo value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
