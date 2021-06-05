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
    public class OpenOrdersConverter : JsonConverter<OpenOrdersMessage>
    {
        public override OpenOrdersMessage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            OpenOrdersMessage message = new OpenOrdersMessage();

            JsonDocument doc = JsonDocument.ParseValue(ref reader);
            JsonElement.ArrayEnumerator arrayEnum = doc.RootElement.EnumerateArray();

            arrayEnum.MoveNext();
            JsonElement.ArrayEnumerator ordersEnum = arrayEnum.Current.EnumerateArray();

            while (ordersEnum.MoveNext())
            {
                JsonElement.ObjectEnumerator orderObj = ordersEnum.Current.EnumerateObject();
                orderObj.MoveNext();

                OpenOrdersMessage.OpenOrderData orderData = ReadOrderData(orderObj.Current.Value.EnumerateObject());
                message.orders.Add(orderObj.Current.Name, orderData);
            }

            message.ChannelName = JsonUtils.ReadString(ref arrayEnum);

            arrayEnum.MoveNext();
            JsonElement.ObjectEnumerator sequenceEnum = arrayEnum.Current.EnumerateObject();
            message.SequenceNumber = JsonUtils.ReadInt(ref sequenceEnum);

            return message;
        }

        private OpenOrdersMessage.OpenOrderData ReadOrderData(JsonElement.ObjectEnumerator orderDataEnum)
        {
            OpenOrdersMessage.OpenOrderData orderData = new OpenOrdersMessage.OpenOrderData();

            while (orderDataEnum.MoveNext())
            {
                switch (orderDataEnum.Current.Name)
                {
                    case "refid":
                        orderData.ReferralTransactionID = JsonUtils.ReadString(orderDataEnum.Current);
                        break;
                    case "userref":
                        orderData.UserReferenceID = JsonUtils.ReadInt(orderDataEnum.Current);
                        break;
                    case "status":
                        orderData.Status = JsonUtils.ReadString(orderDataEnum.Current);
                        break;
                    case "opentm":
                        orderData.PlaceTime = JsonUtils.ReadDouble(orderDataEnum.Current);
                        break;
                    case "starttm":
                        orderData.ScheduledStartTime = JsonUtils.ReadDouble(orderDataEnum.Current);
                        break;
                    case "expiretm":
                        orderData.ExpirationTime = JsonUtils.ReadDouble(orderDataEnum.Current);
                        break;
                    case "desc":
                        orderData.Description = ReadOrderDescription(orderDataEnum.Current.Value.EnumerateObject());
                        break;
                    case "vol":
                        orderData.Volume = JsonUtils.ReadDouble(orderDataEnum.Current);
                        break;
                    case "vol_exec":
                        orderData.VolumeExecuted = JsonUtils.ReadDouble(orderDataEnum.Current);
                        break;
                    case "cost":
                        orderData.TotalCost = JsonUtils.ReadDouble(orderDataEnum.Current);
                        break;
                    case "fee":
                        orderData.TotalFee = JsonUtils.ReadDouble(orderDataEnum.Current);
                        break;
                    case "avg_price":
                        orderData.AveragePrice = JsonUtils.ReadDouble(orderDataEnum.Current);
                        break;
                    case "stopprice":
                        orderData.StopPrice = JsonUtils.ReadDouble(orderDataEnum.Current);
                        break;
                    case "limitprice":
                        orderData.LimitPrice = JsonUtils.ReadDouble(orderDataEnum.Current);
                        break;
                    case "misc":
                        orderData.Miscellaneous = JsonUtils.ReadString(orderDataEnum.Current);
                        break;
                    case "oflags":
                        orderData.OptionalFlags = JsonUtils.ReadString(orderDataEnum.Current);
                        break;
                    case "timeinforce":
                        orderData.TimeInForce = JsonUtils.ReadString(orderDataEnum.Current);
                        break;
                    case "cancel_reason":
                        orderData.CancelReason = JsonUtils.ReadString(orderDataEnum.Current);
                        break;
                    case "ratecount":
                        orderData.RateCount = JsonUtils.ReadInt(orderDataEnum.Current);
                        break;

                }
            }

            return orderData;
        }

        private OpenOrdersMessage.OpenOrderDescription ReadOrderDescription(JsonElement.ObjectEnumerator descriptionEnum)
        {
            OpenOrdersMessage.OpenOrderDescription desc = new OpenOrdersMessage.OpenOrderDescription();

            while (descriptionEnum.MoveNext())
            {
                switch (descriptionEnum.Current.Name)
                {
                    case "pair":
                        desc.Pair = JsonUtils.ReadString(descriptionEnum.Current);
                        break;
                    case "position":
                        desc.Position = JsonUtils.ReadString(descriptionEnum.Current);
                        break;
                    case "type":
                        desc.OrderSide = JsonUtils.ReadString(descriptionEnum.Current);
                        break;
                    case "ordertype":
                        desc.OrderType = JsonUtils.ReadString(descriptionEnum.Current);
                        break;
                    case "price":
                        desc.PrimaryPrice = JsonUtils.ReadDouble(descriptionEnum.Current);
                        break;
                    case "price2":
                        desc.SecondaryPrice = JsonUtils.ReadDouble(descriptionEnum.Current);
                        break;
                    case "leverage":
                        desc.LeverageAmount = JsonUtils.ReadDouble(descriptionEnum.Current);
                        break;
                    case "order":
                        desc.OrderDescription = JsonUtils.ReadString(descriptionEnum.Current);
                        break;
                    case "close":
                        desc.CloseDescription = JsonUtils.ReadString(descriptionEnum.Current);
                        break;
                }
            }

            return desc;
        }

        public override void Write(Utf8JsonWriter writer, OpenOrdersMessage value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
