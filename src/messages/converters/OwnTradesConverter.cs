using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.okitoki.kraken.utils;
using com.okitoki.kraken.messages;

namespace com.okitoki.kraken.messages.converters
{
    public class OwnTradesConverter : JsonConverter<OwnTradesMessage>
    {
        public override OwnTradesMessage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            OwnTradesMessage message = new OwnTradesMessage();

            JsonDocument doc = JsonDocument.ParseValue(ref reader);
            JsonElement.ArrayEnumerator arrayEnum = doc.RootElement.EnumerateArray();

            arrayEnum.MoveNext();
            JsonElement.ArrayEnumerator tradesEnum = arrayEnum.Current.EnumerateArray();

            while(tradesEnum.MoveNext())
            {
                JsonElement.ObjectEnumerator tradeObj = tradesEnum.Current.EnumerateObject();
                tradeObj.MoveNext();

                OwnTradesMessage.OwnTradesData tradeData = ReadTradeData(tradeObj.Current.Value.EnumerateObject());
                message.trades.Add(tradeObj.Current.Name, tradeData);
            }

            message.ChannelName = JsonUtils.ReadString(ref arrayEnum);

            arrayEnum.MoveNext();
            JsonElement.ObjectEnumerator sequenceEnum = arrayEnum.Current.EnumerateObject();
            message.SequenceNumber = JsonUtils.ReadInt(ref sequenceEnum);

            return message;
        }

        private OwnTradesMessage.OwnTradesData ReadTradeData(JsonElement.ObjectEnumerator tradeDataEnum)
        {
            OwnTradesMessage.OwnTradesData tradeData = new OwnTradesMessage.OwnTradesData();

            while (tradeDataEnum.MoveNext())
            {
                switch(tradeDataEnum.Current.Name)
                {
                    case "ordertxid": 
                        tradeData.OrderTransactionID = JsonUtils.ReadString(tradeDataEnum.Current); 
                        break;
                    case "postxid":
                        tradeData.PositionTransactionID = JsonUtils.ReadString(tradeDataEnum.Current);
                        break;
                    case "pair":
                        tradeData.Pair = JsonUtils.ReadString(tradeDataEnum.Current);
                        break;
                    case "time":
                        tradeData.Timestamp = JsonUtils.ReadDouble(tradeDataEnum.Current);
                        break;
                    case "type":
                        tradeData.Type = JsonUtils.ReadString(tradeDataEnum.Current);
                        break;
                    case "ordertype":
                        tradeData.OrderType = JsonUtils.ReadString(tradeDataEnum.Current);
                        break;
                    case "price":
                        tradeData.Price = JsonUtils.ReadDouble(tradeDataEnum.Current);
                        break;
                    case "cost":
                        tradeData.Cost = JsonUtils.ReadDouble(tradeDataEnum.Current);
                        break;
                    case "fee":
                        tradeData.Fee = JsonUtils.ReadDouble(tradeDataEnum.Current);
                        break;
                    case "vol":
                        tradeData.Volume = JsonUtils.ReadDouble(tradeDataEnum.Current);
                        break;
                    case "margin":
                        tradeData.Margin = JsonUtils.ReadDouble(tradeDataEnum.Current);
                        break;
                    case "userref":
                        tradeData.UserReferenceID = JsonUtils.ReadInt(tradeDataEnum.Current);
                        break;
                }
            }

            return tradeData;
        }

        public override void Write(Utf8JsonWriter writer, OwnTradesMessage value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
