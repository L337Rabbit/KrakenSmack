using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.okitoki.kraken.messages;

namespace com.okitoki.kraken.extensions
{
    public static class OrderSideExtensions
    {
        public static string StringValue(this OrderSide orderSide)
        {
            switch(orderSide)
            {
                case OrderSide.BUY: return "buy";
                case OrderSide.SELL: return "sell";
            }

            return null;
        }

        public static OrderSide GetOrderSide(this string stringValue)
        {
            switch(stringValue)
            {
                case "buy": return OrderSide.BUY;
                case "sell": return OrderSide.SELL;
            }

            return OrderSide.BUY;
        }
    }
}
