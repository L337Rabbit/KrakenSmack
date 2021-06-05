using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.okitoki.kraken.messages;

namespace com.okitoki.kraken.extensions
{
    public static class TimeInForceExtensions
    {
        public static string StringValue(this TimeInForce timeInForce)
        {
            switch(timeInForce)
            {
                case TimeInForce.GOOD_TIL_CANCELLED: return "GTC";
                case TimeInForce.IMMEDIATE_OR_CANCEL: return "IOC";
                case TimeInForce.GOOD_TIL_DATE: return "GTD";
            }

            return null;
        }

        public static TimeInForce GetTimeInForce(this string stringValue)
        {
            switch(stringValue)
            {
                case "GTC": return TimeInForce.GOOD_TIL_CANCELLED;
                case "IOC": return TimeInForce.IMMEDIATE_OR_CANCEL;
                case "GTD": return TimeInForce.GOOD_TIL_DATE;
            }

            return TimeInForce.INVALID;
        }
    }
}
