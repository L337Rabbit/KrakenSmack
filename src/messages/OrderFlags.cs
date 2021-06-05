using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okitoki.kraken.messages
{
    public enum  OrderFlags
    {
        INVALID,
        VOLUME_IN_QUOTE_CURRENCY,
        PREFER_FEE_IN_BASE_CURRENCY,
        PREFER_FEE_IN_QUOTE_CURRENCY,
        NO_MARKET_PRICE_PROTECTION,
        POST_ONLY_ORDER
    }
}
