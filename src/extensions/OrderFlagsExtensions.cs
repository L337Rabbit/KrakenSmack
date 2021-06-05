using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.okitoki.kraken.messages;

namespace com.okitoki.kraken.extensions
{
    public static class OrderFlagsExtensions
    {
        public static string StringValue(this OrderFlags flags)
        {
            switch(flags)
            {
                case OrderFlags.VOLUME_IN_QUOTE_CURRENCY: return "viqc";
                case OrderFlags.PREFER_FEE_IN_BASE_CURRENCY: return "fcib";
                case OrderFlags.PREFER_FEE_IN_QUOTE_CURRENCY: return "fciq";
                case OrderFlags.NO_MARKET_PRICE_PROTECTION: return "nompp";
                case OrderFlags.POST_ONLY_ORDER: return "post";
            }

            return null;
        }

        public static OrderFlags GetOrderFlag(this string stringValue)
        {
            switch(stringValue)
            {
                case "viqc": return OrderFlags.VOLUME_IN_QUOTE_CURRENCY;
                case "fcib": return OrderFlags.PREFER_FEE_IN_BASE_CURRENCY;
                case "fciq": return OrderFlags.PREFER_FEE_IN_QUOTE_CURRENCY;
                case "nompp": return OrderFlags.NO_MARKET_PRICE_PROTECTION;
                case "post": return OrderFlags.POST_ONLY_ORDER;
            }

            return OrderFlags.INVALID;
        }

        public static List<OrderFlags> GetOrderFlags(this string stringValue)
        {
            List<OrderFlags> flags = new List<OrderFlags>();

            //Parse the string for flag values.
            string[] flagValues = stringValue.Split(',', ' ');
            for(int i = 0; i < flagValues.Length; i++)
            {
                OrderFlags flag = flagValues[i].GetOrderFlag();
                if(flag != OrderFlags.INVALID && !flags.Contains(flag))
                {
                    flags.Add(flag);
                }
            }

            return flags;
        }

        public static string GetFlagString(this List<OrderFlags> flags)
        {
            string stringValue = "";
            for(int i = 0; i < flags.Count; i++)
            {
                stringValue += flags[i].StringValue();

                if(i < flags.Count - 1)
                {
                    stringValue += ",";
                }
            }

            return stringValue;
        }
    }
}
