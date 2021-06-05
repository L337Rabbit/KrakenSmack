using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okitoki.kraken.messages
{
    public enum TimeInForce
    {
        INVALID,
        GOOD_TIL_CANCELLED,
        IMMEDIATE_OR_CANCEL,
        GOOD_TIL_DATE
    }
}
