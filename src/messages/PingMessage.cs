using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okitoki.kraken.messages
{
    public class PingMessage : IdentifiableMessage
    {
        public PingMessage()
        {
            this.Event = "ping";
        }
    }
}
