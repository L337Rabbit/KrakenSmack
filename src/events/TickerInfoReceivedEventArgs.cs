using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.okitoki.kraken.messages;

namespace com.okitoki.kraken.events
{
    public class TickerInfoReceivedEventArgs : EventArgs
    {
        public TickerInfo tickerInfo;
    }
}
