using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.okitoki.kraken.extensions;

namespace com.okitoki.kraken.messages
{
    public class AddOrderMessage : AuthenticatedMessage
    {
        public AddOrderMessage(OrderType orderType, OrderSide orderSide, string pair, double volume)
        {
            this.Event = "addOrder";
            this.OrderType = orderType;
            this.OrderSide = orderSide;
            this.Pair = pair;
            this.Volume = volume;
        }

        [JsonIgnore]
        public OrderType OrderType { get; set; }

        [JsonPropertyName("ordertype")]
        [JsonInclude]
        public string OrderTypeString 
        { 
            get { return this.OrderType.StringValue(); }
            set { this.OrderType = value.GetOrderType();  }
        }

        [JsonIgnore]
        public OrderSide OrderSide { get; set; }

        [JsonPropertyName("type")]
        [JsonInclude]
        public string OrderSideString 
        {
            get { return this.OrderSide.StringValue(); }
            set { this.OrderSide = value.GetOrderSide(); } 
        }

        [JsonPropertyName("pair")]
        [JsonInclude]
        public string Pair { get; set; }

        [JsonPropertyName("price")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public double PrimaryPrice { get; set; }

        [JsonPropertyName("price2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public double SecondaryPrice { get; set; }

        [JsonPropertyName("volume")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public double Volume { get; set; }

        [JsonPropertyName("leverage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public double DesiredLeverage { get; set; }

        [JsonIgnore]
        public List<OrderFlags> OrderFlags { get; set; }

        [JsonPropertyName("oflags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string OrderFlagsString 
        {
            get { return OrderFlags.GetFlagString(); }
            set { this.OrderFlags = value.GetOrderFlags(); } 
        }

        [JsonIgnore]
        public FutureTime ScheduledStartTime { get; set; }

        [JsonPropertyName("starttm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string ScheduledStartTimeString 
        {
            get { return this.ScheduledStartTime.ToString(); }
            set { this.ScheduledStartTime = FutureTime.FromString(value); }
        }

        [JsonIgnore]
        public FutureTime ScheduledExpirationTime { get; set; }

        [JsonPropertyName("expiretm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string ScheduledExpirationTimeString 
        { 
            get { return this.ScheduledExpirationTime.ToString(); }
            set { this.ScheduledExpirationTime = FutureTime.FromString(value); }
        }

        [JsonPropertyName("deadline")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string Deadline { get; set; }

        [JsonPropertyName("userref")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string UserReferenceID { get; set; }

        [JsonPropertyName("validate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string ValidateOnly { get; set; }

        [JsonPropertyName("close[ordertype]")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string CloseOrderType { get; set; }

        [JsonPropertyName("close[price]")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public double PrimaryClosePrice { get; set; }

        [JsonPropertyName("close[price2]")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public double SecondaryClosePrice { get; set; }

        [JsonIgnore]
        public TimeInForce TimeInForce { get; set; }

        [JsonPropertyName("timeinforce")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string TimeInForceString
        {
            get { return TimeInForce.StringValue(); }
            set { this.TimeInForce = value.GetTimeInForce(); }
        }

        [JsonPropertyName("trading_agreement")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonInclude]
        public string TradingAgreement { get; set; }
    }
}
