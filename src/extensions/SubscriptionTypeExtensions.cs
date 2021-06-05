using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.okitoki.kraken.messages;

namespace com.okitoki.kraken.extensions
{
    public static class SubscriptionTypeExtensions
    {
        public static string StringValue(this SubscriptionType subType)
        {
            switch(subType)
            {
                case SubscriptionType.ALL: return "*";
                case SubscriptionType.BOOK: return "book";
                case SubscriptionType.OHLC: return "ohlc";
                case SubscriptionType.OPEN_ORDERS: return "openOrders";
                case SubscriptionType.OWN_TRADES: return "ownTrades";
                case SubscriptionType.SPREAD: return "spread";
                case SubscriptionType.TICKER: return "ticker";
                case SubscriptionType.TRADE: return "trade";
            }

            return "*";
        }

        public static SubscriptionType GetSubscriptionType(this string stringValue)
        {
            switch(stringValue)
            {
                case "*": return SubscriptionType.ALL;
                case "book": return SubscriptionType.BOOK;
                case "ohlc": return SubscriptionType.OHLC;
                case "openOrders": return SubscriptionType.OPEN_ORDERS;
                case "ownTrades": return SubscriptionType.OWN_TRADES;
                case "spread": return SubscriptionType.SPREAD;
                case "ticker": return SubscriptionType.TICKER;
                case "trade": return SubscriptionType.TRADE;
            }

            return SubscriptionType.ALL;
        }
    }
}
