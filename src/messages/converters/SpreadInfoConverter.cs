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
    public class SpreadInfoConverter : JsonConverter<SpreadInfo>
    {
        public override SpreadInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            SpreadInfo info = new SpreadInfo();

            JsonDocument doc = JsonDocument.ParseValue(ref reader);
            JsonElement.ArrayEnumerator arrayEnum = doc.RootElement.EnumerateArray();

            info.ChannelID = JsonUtils.ReadInt(ref arrayEnum);

            arrayEnum.MoveNext();
            JsonElement.ArrayEnumerator dataArray = arrayEnum.Current.EnumerateArray();

            info.Data.bidPrice = JsonUtils.ReadDouble(ref dataArray);
            info.Data.askPrice = JsonUtils.ReadDouble(ref dataArray);
            info.Data.timestamp = JsonUtils.ReadDouble(ref dataArray);
            info.Data.bidVolume = JsonUtils.ReadDouble(ref dataArray);
            info.Data.askVolume = JsonUtils.ReadDouble(ref dataArray);

            info.ChannelName = JsonUtils.ReadString(ref arrayEnum);
            info.Pair = JsonUtils.ReadString(ref arrayEnum);

            return info;
        }

        public override void Write(Utf8JsonWriter writer, SpreadInfo value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
