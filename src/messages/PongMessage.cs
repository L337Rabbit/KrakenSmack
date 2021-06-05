using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okitoki.kraken.messages
{
    public class PongMessage : IdentifiableMessage
    {
        public PongMessage()
        {
            this.Event = "pong";
        }
    }
}
