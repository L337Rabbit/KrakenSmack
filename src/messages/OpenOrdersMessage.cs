using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okitoki.kraken.messages
{
    public class OpenOrdersMessage
    {
        public Dictionary<string, OpenOrderData> orders = new Dictionary<string, OpenOrderData>();

        public string ChannelName { get; set; }

        public int SequenceNumber { get; set; }

        public class OpenOrderData
        {
            public string ReferralTransactionID { get; set; }
            
            public int UserReferenceID { get; set; }
            
            public string Status { get; set; }
            
            public double PlaceTime { get; set; }
            
            public double ScheduledStartTime { get; set; }
            
            public double ExpirationTime { get; set; }

            public OpenOrderDescription Description { get; set; }

            public double Volume { get; set; }

            public double VolumeExecuted { get; set; }

            public double TotalCost { get; set; }

            public double TotalFee { get; set; }

            public double AveragePrice { get; set; }

            public double StopPrice { get; set; }

            public double LimitPrice { get; set; }

            public string Miscellaneous { get; set; }

            public string OptionalFlags { get; set; }

            public string TimeInForce { get; set; }

            public string CancelReason { get; set; }

            public int RateCount { get; set; }
        }

        public class OpenOrderDescription 
        { 
            public string Pair { get; set; }

            public string Position { get; set; }

            public string OrderSide { get; set; }

            public string OrderType { get; set; }

            public double PrimaryPrice { get; set; }

            public double SecondaryPrice { get; set; }

            public double LeverageAmount { get; set; }

            public string OrderDescription { get; set; }

            public string CloseDescription { get; set; }
        }
    }
}
