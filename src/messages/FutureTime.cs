using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okitoki.kraken.messages
{
    public class FutureTime
    {
        public FutureTimeType timeType = FutureTimeType.NOW;
        public long timeValue = 0;

        public FutureTime() { }

        public FutureTime(int numSeconds)
        {
            this.timeType = FutureTimeType.SECONDS_FROM_NOW;
            this.timeValue = numSeconds;
        }

        public FutureTime(long timestamp)
        {
            this.timeType = FutureTimeType.TIMESTAMP;
            this.timeValue = timestamp;
        }

        public FutureTime(FutureTimeType timeType, long timeValue)
        {
            this.timeType = timeType;
            this.timeValue = timeValue;
        }

        public override string ToString()
        {
            switch(timeType)
            {
                case FutureTimeType.NOW: return "0";
                case FutureTimeType.SECONDS_FROM_NOW: return "+" + timeValue;
                case FutureTimeType.TIMESTAMP: return "" + timeValue;
            }

            return null;
        }

        public static FutureTime FromString(string value)
        {
            FutureTime futureTime = new FutureTime();
            double timeValue = double.Parse(value.Substring(1));

            if(timeValue <= 0.01)
            {
                futureTime.timeType = FutureTimeType.NOW;
                futureTime.timeValue = 0L;
            }
            else if (value.Contains("+"))
            {
                futureTime.timeType = FutureTimeType.SECONDS_FROM_NOW;
                futureTime.timeValue = (long)timeValue;
            }
            else
            {
                futureTime.timeType = FutureTimeType.TIMESTAMP;
                futureTime.timeValue = (long)timeValue;
            }

            return futureTime;
        }

        public enum FutureTimeType
        {
            NOW,
            SECONDS_FROM_NOW,
            TIMESTAMP
        }
    }
}
