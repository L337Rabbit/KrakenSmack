using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okitoki.kraken.messages
{
    public enum OrderType
    {
        MARKET,
        LIMIT,
        STOP_LOSS,
        TAKE_PROFIT,
        STOP_LOST_LIMIT,
        TAKE_PROFIT_LIMIT,
        SETTLE_POSITION
    }
}
