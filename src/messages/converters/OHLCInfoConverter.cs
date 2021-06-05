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
    public class OHLCInfoConverter : JsonConverter<OHLCInfo>
    {
        public override OHLCInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            OHLCInfo info = new OHLCInfo();

            JsonDocument doc = JsonDocument.ParseValue(ref reader);

            JsonElement.ArrayEnumerator arrayEnum = doc.RootElement.EnumerateArray();

            info.ChannelID = JsonUtils.ReadInt(ref arrayEnum);

            arrayEnum.MoveNext();
            JsonElement.ArrayEnumerator dataArray = arrayEnum.Current.EnumerateArray();

            info.Data.beginTime = JsonUtils.ReadDouble(ref dataArray);
            info.Data.endTime = JsonUtils.ReadDouble(ref dataArray);
            info.Data.openPrice = JsonUtils.ReadDouble(ref dataArray);
            info.Data.highPrice = JsonUtils.ReadDouble(ref dataArray);
            info.Data.lowPrice = JsonUtils.ReadDouble(ref dataArray);
            info.Data.closePrice = JsonUtils.ReadDouble(ref dataArray);
            info.Data.volumeWeightedAveragePrice = JsonUtils.ReadDouble(ref dataArray);
            info.Data.volume = JsonUtils.ReadDouble(ref dataArray);
            info.Data.tradeCount = JsonUtils.ReadInt(ref dataArray);

            info.ChannelName = JsonUtils.ReadString(ref arrayEnum);
            info.Pair = JsonUtils.ReadString(ref arrayEnum);

            return info;
        }

        public override void Write(Utf8JsonWriter writer, OHLCInfo value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }


    }
}
