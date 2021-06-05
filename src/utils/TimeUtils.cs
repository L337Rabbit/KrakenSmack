using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okitoki.kraken.utils
{
    public class TimeUtils
    {
        public static double UnixTimestamp()
        {
            return (System.DateTime.UtcNow - System.DateTime.UnixEpoch).TotalMilliseconds;
        }
    }
}
