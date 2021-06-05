using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.okitoki.kraken.messages;

namespace com.okitoki.kraken.extensions
{
    public static class OrderTypeExtensions
    {
        public static string StringValue(this OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.MARKET: return "market";
                case OrderType.LIMIT: return "limit";
                case OrderType.STOP_LOSS: return "stop-loss";
                case OrderType.TAKE_PROFIT: return "take-profit";
                case OrderType.STOP_LOST_LIMIT: return "stop-lost-limit";
                case OrderType.TAKE_PROFIT_LIMIT: return "take-profit-limit";
                case OrderType.SETTLE_POSITION: return "settle-position";
            }

            return null;
        }

        public static OrderType GetOrderType(this string stringValue)
        {
            switch (stringValue)
            {
                case "market": return OrderType.MARKET;
                case "limit": return OrderType.LIMIT;
                case "stop-loss": return OrderType.STOP_LOSS;
                case "take-profit": return OrderType.TAKE_PROFIT;
                case "stop-lost-limit": return OrderType.STOP_LOST_LIMIT;
                case "take-profit-limit": return OrderType.TAKE_PROFIT_LIMIT;
                case "settle-position": return OrderType.SETTLE_POSITION;
            }

            return OrderType.MARKET;
        }
    }
}
