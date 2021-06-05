using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.WebSockets;
using System.IO;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net.Http.Headers;
using com.okitoki.kraken.events;
using com.okitoki.kraken.messages;
using com.okitoki.kraken.messages.converters;
using com.okitoki.kraken.exception;
using com.okitoki.kraken.utils;

namespace com.okitoki.kraken
{
    public class KrakenClient
    {
        private bool isAuthenticated = false;
        private string privateKey;
        private string apiKey;
        private string authToken;

        private Random random = new Random(158);
        private ClientWebSocket socket;
        private Thread listenThread;

        public event EventHandler<TickerInfoReceivedEventArgs> TickerInfoReceived;
        public event EventHandler<OHLCInfoReceivedEventArgs> OHLCInfoReceived;
        public event EventHandler<TradeInfoReceivedEventArgs> TradeInfoReceived;
        public event EventHandler<SpreadInfoReceivedEventArgs> SpreadInfoReceived;
        public event EventHandler<BookInfoReceivedEventArgs> BookInfoReceived;
        public event EventHandler<HeartbeatReceivedEventArgs> HeartbeatReceived;
        public event EventHandler<SystemStatusReceivedEventArgs> SystemStatusReceived;
        public event EventHandler<PongReceivedEventArgs> PongReceived;
        public event EventHandler<SubscriptionStatusReceivedEventArgs> SubscriptionStatusReceived;
        public event EventHandler<AddOrderStatusReceivedEventArgs> AddOrderStatusReceived;
        public event EventHandler<CancelOrderStatusReceivedEventArgs> CancelOrderStatusReceived;
        public event EventHandler<CancelAllOrdersStatusReceivedEventArgs> CancelAllOrdersStatusReceived;
        public event EventHandler<CancelAllOrdersAfterTimeStatusReceivedEventArgs> CancelAllOrdersAfterTimeStatusReceived;
        public event EventHandler<OwnTradesReceivedEventArgs> OwnTradesDataReceived;
        public event EventHandler<OpenOrdersReceivedEventArgs> OpenOrdersDataReceived;

        public KrakenClient() { }

        public KrakenClient(string privateKey, string apiKey)
        {
            isAuthenticated = true;
            this.privateKey = privateKey;
            this.apiKey = apiKey;

            GetWebSocketSecurityToken();
        }

        public void Connect()
        {
            socket = new ClientWebSocket();
            socket.ConnectAsync(new Uri("wss://ws.kraken.com/"), CancellationToken.None);

            WaitForConnection();
            Listen();
        }

        public void Disconnect()
        {
            listenThread.Interrupt();
            socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Kraken websocket disconnected.", CancellationToken.None);
        }

        private void WaitForConnection()
        {
            while (socket.State != WebSocketState.Open)
            {
                Thread.Sleep(1000);
            }
        }

        private void Listen()
        {
            listenThread = new Thread(() => 
            {
                //byte[] buffer = new byte[256];
                ArraySegment<byte> buffer = WebSocket.CreateClientBuffer(1024, 1024);

                while (Thread.CurrentThread.IsAlive)
                {
                    Task<byte[]> receiveMessageTask = WaitForMessage(buffer);
                    receiveMessageTask.Wait();

                    ProcessReceivedMessage(receiveMessageTask.Result);
                }
            });

            listenThread.Name = "KrakenClient_Listen";
            listenThread.Start();
        }

        private string GetWebSocketSecurityToken()
        {
            long nonce = (long)(TimeUtils.UnixTimestamp() * 100);
            string apiSignature = SecurityUtils.GenerateApiSignature(privateKey, nonce, "/0/private/GetWebSocketsToken", "nonce=" + nonce);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.kraken.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("API-Key", apiKey);
            client.DefaultRequestHeaders.Add("API-Sign", apiSignature);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            Dictionary<string, string> keysAndValues = new Dictionary<string, string>() { { "nonce", ("" + nonce) } };
            FormUrlEncodedContent content = new FormUrlEncodedContent(keysAndValues);

            Task<HttpResponseMessage> responseMessageTask = client.PostAsync("0/private/GetWebSocketsToken", content);
            responseMessageTask.Wait();

            HttpResponseMessage response = responseMessageTask.Result;
            Task<string> readResponseBodyTask = response.Content.ReadAsStringAsync();
            readResponseBodyTask.Wait();

            //Console.WriteLine("Web token response: " + readResponseBodyTask.Result);
            WebSocketSecurityTokenResponse tokenResponse = JsonSerializer.Deserialize<WebSocketSecurityTokenResponse>(readResponseBodyTask.Result);

            return tokenResponse.Result.Token;
        }

        public void Subscribe(SubscribeMessage subscribeMessage)
        {
            if(subscribeMessage.IsAuthenticationRequired())
            {
                //Check for token presence.
                string token = subscribeMessage.Subscription.Token;
                if (token == null || token.Length == 0)
                {
                    if(!isAuthenticated)
                    {
                        throw new AuthenticationException("Authentication is required in order to subscribe to the channels specified. In order to use these channels, pass in credentials to the KrakenClient or specify a valid token value on the SubscribeMessage.");
                    }

                    if(authToken == null)
                    {
                        try
                        {
                            authToken = GetWebSocketSecurityToken();
                        }
                        catch(Exception e)
                        {
                            throw new AuthenticationException("Subscriptions to the " + subscribeMessage.Subscription.Name + " channel require an authentication token to be provided: " + e);
                        }
                    }

                    subscribeMessage.Subscription.Token = authToken;
                }
            }

            try
            {
                string message = JsonSerializer.Serialize(subscribeMessage);
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
                socket.SendAsync(messageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch(Exception e)
            {
                Console.WriteLine("An error occurred when attempting to create a subscription for " + subscribeMessage.Subscription.Name + ": " + e);
            }
        }

        public void Subscribe(SubscriptionType subscriptionType, string[] currencyPairs)
        {
            SubscribeMessage msg = new SubscribeMessage();
            msg.Pairs = currencyPairs;
            msg.RequestId = random.Next(10000000, 99999999);

            Subscription subscription = new Subscription();
            subscription.SubscriptionType = subscriptionType;
            msg.Subscription = subscription;

            Subscribe(msg);
        }

        public void Unsubscribe(UnsubscribeMessage unsubscribeMessage)
        {
            if (unsubscribeMessage.IsAuthenticationRequired())
            {
                //Check for token presence.
                string token = unsubscribeMessage.Subscription.Token;
                if (token == null || token.Length == 0)
                {
                    if (!isAuthenticated)
                    {
                        throw new AuthenticationException("Authentication is required in order to unsubscribe from the channels specified. In order to unsubscribe from these channels, pass in credentials to the KrakenClient or specify a valid token value on the UnsubscribeMessage.");
                    }

                    if (authToken == null)
                    {
                        try
                        {
                            authToken = GetWebSocketSecurityToken();
                        }
                        catch (Exception e)
                        {
                            throw new AuthenticationException("Unsubscribing from the " + unsubscribeMessage.Subscription.Name + " channel requires an authentication token to be provided: " + e);
                        }
                    }

                    unsubscribeMessage.Subscription.Token = authToken;
                }
            }

            try
            {
                string message = JsonSerializer.Serialize(unsubscribeMessage);
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
                socket.SendAsync(messageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred when attempting to unsubscribe from " + unsubscribeMessage.Subscription.Name + ": " + e);
            }
        }

        public void Unsubscribe(SubscriptionType subscriptionType, string[] currencyPairs)
        {
            UnsubscribeMessage msg = new UnsubscribeMessage();
            msg.Pairs = currencyPairs;
            msg.RequestId = random.Next(10000000, 99999999);

            Subscription subscription = new Subscription();
            subscription.SubscriptionType = subscriptionType;
            msg.Subscription = subscription;

            Unsubscribe(msg);
        }

        public void AddOrder(AddOrderMessage addOrderMessage)
        {
            //Check for authentication token.
            if(addOrderMessage.Token == null || addOrderMessage.Token.Length == 0)
            {
                if (!isAuthenticated)
                {
                    throw new AuthenticationException("Authentication is required to create an order. Pass in credentials to the KrakenClient or specify a valid token value on the AddOrderMessage.");
                }

                if (authToken == null)
                {
                    authToken = GetWebSocketSecurityToken();

                    if (authToken == null)
                    {
                        throw new AuthenticationException("Adding an order requires an authentication token to be provided.");
                    }
                }

                addOrderMessage.Token = authToken;
            }

            try
            {
                string message = JsonSerializer.Serialize(addOrderMessage);
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
                socket.SendAsync(messageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred when attempting to add an order: " + e);
            }
        }

        public void CancelOrders(List<string> transactionIDs)
        {
            CancelOrderMessage message = new CancelOrderMessage(transactionIDs.ToArray());
            CancelOrders(message);
        }

        public void CancelOrders(CancelOrderMessage cancelOrdersMessage)
        {
            if (cancelOrdersMessage.Token == null || cancelOrdersMessage.Token.Length == 0)
            {
                if (!isAuthenticated)
                {
                    throw new AuthenticationException("Authentication is required to cancel orders. Pass in credentials to the KrakenClient or specify a valid token value on the CancelOrderMessage.");
                }

                if (authToken == null)
                {
                    authToken = GetWebSocketSecurityToken();

                    if (authToken == null)
                    {
                        throw new AuthenticationException("Cancelling orders requires an authentication token to be provided.");
                    }
                }

                cancelOrdersMessage.Token = authToken;
            }

            try
            {
                string message = JsonSerializer.Serialize(cancelOrdersMessage);
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
                socket.SendAsync(messageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred when attempting to cancel orders: " + e);
            }
        }

        public void CancelAllOrders()
        {
            CancelAllOrdersMessage message = new CancelAllOrdersMessage();
            CancelAllOrders(message);
        }

        public void CancelAllOrders(CancelAllOrdersMessage cancelAllOrdersMessage)
        {
            if (cancelAllOrdersMessage.Token == null || cancelAllOrdersMessage.Token.Length == 0)
            {
                if (!isAuthenticated)
                {
                    throw new AuthenticationException("Authentication is required to cancel all orders. Pass in credentials to the KrakenClient or specify a valid token value on the CancelAllOrderMessage.");
                }

                if (authToken == null)
                {
                    authToken = GetWebSocketSecurityToken();

                    if (authToken == null)
                    {
                        throw new AuthenticationException("Cancelling all orders requires an authentication token to be provided.");
                    }
                }

                cancelAllOrdersMessage.Token = authToken;
            }

            try
            {
                string message = JsonSerializer.Serialize(cancelAllOrdersMessage);
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
                socket.SendAsync(messageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred when attempting to cancel all orders: " + e);
            }
        }

        public void CancelAllOrdersAfterTime(int numSeconds)
        {
            CancelAllOrdersAfterTimeMessage message = new CancelAllOrdersAfterTimeMessage(numSeconds);
            CancelAllOrdersAfterTime(message);
        }

        public void CancelAllOrdersAfterTime(CancelAllOrdersAfterTimeMessage cancelAllOrdersAfterTimeMessage)
        {
            if (cancelAllOrdersAfterTimeMessage.Token == null || cancelAllOrdersAfterTimeMessage.Token.Length == 0)
            {
                if (!isAuthenticated)
                {
                    throw new AuthenticationException("Authentication is required to cancel all orders after a delay. Pass in credentials to the KrakenClient or specify a valid token value on the CancelAllOrderAfterTimeMessage.");
                }

                if (authToken == null)
                {
                    authToken = GetWebSocketSecurityToken();

                    if (authToken == null)
                    {
                        throw new AuthenticationException("Cancelling all orders after some time requires an authentication token to be provided.");
                    }
                }

                cancelAllOrdersAfterTimeMessage.Token = authToken;
            }

            try
            {
                string message = JsonSerializer.Serialize(cancelAllOrdersAfterTimeMessage);
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
                socket.SendAsync(messageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred when attempting to cancel all orders after a delay: " + e);
            }
        }

        private async Task<byte[]> WaitForMessage(ArraySegment<byte> buffer)
        {
            MemoryStream memStream = new MemoryStream();
            WebSocketReceiveResult result = null;

            do
            {
                result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                memStream.Write(buffer.Array, buffer.Offset, result.Count);
            } while (!result.EndOfMessage);

            memStream.Seek(0, SeekOrigin.Begin);
            return memStream.ToArray();
        }

        private void ProcessReceivedMessage(byte[] messageBuffer)
        {
            string data = Encoding.UTF8.GetString(messageBuffer);

            Console.WriteLine("Received Data: " + data);

            if (data.Contains("\"event\":"))
            {
                //Process event.
                ProcessEvent(data);
            }
            else if (data.Contains("ticker"))
            {
                ProcessTickerInfo(data);
            }
            else if(data.Contains("ohlc"))
            {
                ProcessOHLCInfo(data);
            }
            else if(data.Contains("trade"))
            {
                ProcessTradeInfo(data);
            }
            else if(data.Contains("spread"))
            {
                ProcessSpreadInfo(data);
            }
            else if(data.Contains("book"))
            {
                ProcessBookInfo(data);
            }
            else if(data.Contains("ownTrades"))
            {
                ProcessOwnTradesData(data);
            }
            else if(data.Contains("openOrders"))
            {
                ProcessOpenOrdersData(data);
            }
        }

        private void ProcessEvent(string data)
        {
            if (data.Contains("subscriptionStatus"))
            {
                //Received a subscription status message.
                ProcessSubscriptionStatus(data);
            }
            else if (data.Contains("heartbeat"))
            {
                //Received heartbeat signal.
                ProcessHeartbeat(data);
            }
            else if (data.Contains("pong"))
            {
                //Received pong after pinging. Can add keep alive logic here.
                ProcessPong(data);
            }
            else if (data.Contains("systemStatus"))
            {
                //Received system status message.
                ProcessSystemStatus(data);
            }
            else if (data.Contains("addOrderStatus"))
            {
                //Received add order status response message.
                ProcessAddOrderStatus(data);
            }
            else if (data.Contains("cancelOrderStatus"))
            {
                //Received cancel order status response message.
                ProcessCancelOrderStatus(data);
            }
            else if (data.Contains("cancelAllStatus"))
            {
                //Received cancel all orders status message.
                ProcessCancelAllOrdersStatus(data);
            }
            else if (data.Contains("cancelAllOrdersAfterStatus"))
            {
                //Received cancel all orders after time period status message.
                ProcessCancelAllOrdersAfterTimeStatus(data);
            }
        }

        private void ProcessSubscriptionStatus(string data)
        {
            try
            {
                SubscriptionStatusMessage message = JsonSerializer.Deserialize<SubscriptionStatusMessage>(data);

                if (message != null)
                {
                    SubscriptionStatusReceivedEventArgs args = new SubscriptionStatusReceivedEventArgs();
                    args.message = message;
                    OnSubscriptionStatusReceived(args);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Encountered error when attempting to process : \n" + data + "\n" + e);
            }
        }

        private void OnSubscriptionStatusReceived(SubscriptionStatusReceivedEventArgs args)
        {
            if(SubscriptionStatusReceived != null)
            {
                SubscriptionStatusReceived(this, args);
            }
        }

        private void ProcessHeartbeat(string data)
        {
            try
            {
                HeartbeatMessage message = JsonSerializer.Deserialize<HeartbeatMessage>(data);

                if (message != null)
                {
                    HeartbeatReceivedEventArgs args = new HeartbeatReceivedEventArgs();
                    args.message = message;
                    OnHeartbeatReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Encountered error when attempting to process : \n" + data + "\n" + e);
            }
        }

        private void OnHeartbeatReceived(HeartbeatReceivedEventArgs args)
        {
            if(HeartbeatReceived != null)
            {
                HeartbeatReceived(this, args);
            }
        }

        private void ProcessPong(string data)
        {
            try
            {
                PongMessage message = JsonSerializer.Deserialize<PongMessage>(data);

                if (message != null)
                {
                    PongReceivedEventArgs args = new PongReceivedEventArgs();
                    args.message = message;
                    OnPongReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Encountered error when attempting to process : \n" + data + "\n" + e);
            }
        }

        private void OnPongReceived(PongReceivedEventArgs args)
        {
            if(PongReceived != null)
            {
                PongReceived(this, args);
            }
        }

        private void ProcessSystemStatus(string data)
        {
            try
            {
                SystemStatusMessage message = JsonSerializer.Deserialize<SystemStatusMessage>(data);

                if (message != null)
                {
                    SystemStatusReceivedEventArgs args = new SystemStatusReceivedEventArgs();
                    args.message = message;
                    OnSystemStatusReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Encountered error when attempting to process : \n" + data + "\n" + e);
            }
        }

        private void OnSystemStatusReceived(SystemStatusReceivedEventArgs args)
        {
            if(SystemStatusReceived != null)
            {
                SystemStatusReceived(this, args);
            }
        }

        private void ProcessAddOrderStatus(string data)
        {
            try
            {
                AddOrderStatusMessage message = JsonSerializer.Deserialize<AddOrderStatusMessage>(data);

                if (message != null)
                {
                    AddOrderStatusReceivedEventArgs args = new AddOrderStatusReceivedEventArgs();
                    args.message = message;
                    OnAddOrderStatusReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Encountered error when attempting to process : \n" + data + "\n" + e);
            }
        }

        private void OnAddOrderStatusReceived(AddOrderStatusReceivedEventArgs args)
        {
            if(AddOrderStatusReceived != null)
            {
                AddOrderStatusReceived(this, args);
            }
        }

        private void ProcessCancelOrderStatus(string data)
        {
            try
            {
                CancelOrderStatusMessage message = JsonSerializer.Deserialize<CancelOrderStatusMessage>(data);

                if (message != null)
                {
                    CancelOrderStatusReceivedEventArgs args = new CancelOrderStatusReceivedEventArgs();
                    args.message = message;
                    OnCancelOrderStatusReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Encountered error when attempting to process : \n" + data + "\n" + e);
            }
        }

        private void OnCancelOrderStatusReceived(CancelOrderStatusReceivedEventArgs args)
        {
            if(CancelOrderStatusReceived != null)
            {
                CancelOrderStatusReceived(this, args);
            }
        }

        private void ProcessCancelAllOrdersStatus(string data)
        {
            try
            {
                CancelAllOrdersStatusMessage message = JsonSerializer.Deserialize<CancelAllOrdersStatusMessage>(data);

                if (message != null)
                {
                    CancelAllOrdersStatusReceivedEventArgs args = new CancelAllOrdersStatusReceivedEventArgs();
                    args.message = message;
                    OnCancelAllOrdersStatusReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Encountered error when attempting to process : \n" + data + "\n" + e);
            }
        }

        private void OnCancelAllOrdersStatusReceived(CancelAllOrdersStatusReceivedEventArgs args)
        {
            if(CancelAllOrdersStatusReceived != null)
            {
                CancelAllOrdersStatusReceived(this, args);
            }
        }

        private void ProcessCancelAllOrdersAfterTimeStatus(string data)
        {
            try
            {
                CancelAllOrdersAfterTimeStatusMessage message = JsonSerializer.Deserialize<CancelAllOrdersAfterTimeStatusMessage>(data);

                if (message != null)
                {
                    CancelAllOrdersAfterTimeStatusReceivedEventArgs args = new CancelAllOrdersAfterTimeStatusReceivedEventArgs();
                    args.message = message;
                    OnCancelAllOrdersAfterTimeStatusReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Encountered error when attempting to process : \n" + data + "\n" + e);
            }
        }

        private void OnCancelAllOrdersAfterTimeStatusReceived(CancelAllOrdersAfterTimeStatusReceivedEventArgs args)
        {
            if(CancelAllOrdersAfterTimeStatusReceived != null)
            {
                CancelAllOrdersAfterTimeStatusReceived(this, args);
            }
        }

        private void ProcessTickerInfo(string data)
        {
            try
            {
                //Console.WriteLine("Received ticker data: \"" + data + "\"");
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new TickerInfoConverter());
                TickerInfo info = JsonSerializer.Deserialize<TickerInfo>(data, options);

                if(info != null) 
                {
                    TickerInfoReceivedEventArgs args = new TickerInfoReceivedEventArgs();
                    args.tickerInfo = info;
                    OnTickerInfoReceived(args); 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when attempting to deserialize ticker info: " + e);
            }
        }

        private void OnTickerInfoReceived(TickerInfoReceivedEventArgs args)
        {
            if(TickerInfoReceived != null)
            {
                TickerInfoReceived(this, args);
            }
        }

        private void ProcessOHLCInfo(string data)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new OHLCInfoConverter());
                OHLCInfo info = JsonSerializer.Deserialize<OHLCInfo>(data, options);

                if(info != null)
                {
                    OHLCInfoReceivedEventArgs args = new OHLCInfoReceivedEventArgs();
                    args.ohlcInfo = info;
                    OnOHLCInfoReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when attempting to deserialize open-high-low-close info: " + e);
            }
        }

        private void OnOHLCInfoReceived(OHLCInfoReceivedEventArgs args)
        {
            if(OHLCInfoReceived != null)
            {
                OHLCInfoReceived(this, args);
            }
        }

        private void ProcessTradeInfo(string data)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new TradeInfoConverter());
                TradeInfo info = JsonSerializer.Deserialize<TradeInfo>(data, options);

                if(info != null)
                {
                    TradeInfoReceivedEventArgs args = new TradeInfoReceivedEventArgs();
                    args.tradeInfo = info;
                    OnTradeInfoReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when attempting to deserialize trade info: " + e);
            }
        }

        private void OnTradeInfoReceived(TradeInfoReceivedEventArgs args)
        {
            if(TradeInfoReceived != null)
            {
                TradeInfoReceived(this, args);
            }
        }

        private void ProcessSpreadInfo(string data)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new SpreadInfoConverter());
                SpreadInfo info = JsonSerializer.Deserialize<SpreadInfo>(data, options);

                if(info != null)
                {
                    SpreadInfoReceivedEventArgs args = new SpreadInfoReceivedEventArgs();
                    args.spreadInfo = info;
                    OnSpreadInfoReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when attempting to deserialize spread info: " + e);
            }
        }

        private void OnSpreadInfoReceived(SpreadInfoReceivedEventArgs args)
        {
            if(SpreadInfoReceived != null)
            {
                SpreadInfoReceived(this, args);
            }
        }

        private void ProcessBookInfo(string data)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new BookInfoConverter());
                BookInfo info = JsonSerializer.Deserialize<BookInfo>(data, options);

                if(info != null)
                {
                    BookInfoReceivedEventArgs args = new BookInfoReceivedEventArgs();
                    args.bookInfo = info;
                    OnBookInfoReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when attempting to deserialize book info: " + e);
            }
        }

        private void OnBookInfoReceived(BookInfoReceivedEventArgs args)
        {
            if(BookInfoReceived != null)
            {
                BookInfoReceived(this, args);
            }
        }

        private void ProcessOwnTradesData(string data)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new OwnTradesConverter());
                OwnTradesMessage message = JsonSerializer.Deserialize<OwnTradesMessage>(data, options);

                if (message != null)
                {
                    OwnTradesReceivedEventArgs args = new OwnTradesReceivedEventArgs();
                    args.message = message;
                    OnOwnTradesDataReceived(args);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error when attempting to deserialize own trade info: " + e);
            }
        }

        private void OnOwnTradesDataReceived(OwnTradesReceivedEventArgs args)
        {
            if(OwnTradesDataReceived != null)
            {
                OwnTradesDataReceived(this, args);
            }
        }

        private void ProcessOpenOrdersData(string data)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new OpenOrdersConverter());
                OpenOrdersMessage message = JsonSerializer.Deserialize<OpenOrdersMessage>(data, options);

                if (message != null)
                {
                    OpenOrdersReceivedEventArgs args = new OpenOrdersReceivedEventArgs();
                    args.message = message;
                    OnOpenOrdersDataReceived(args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when attempting to deserialize open orders info: " + e);
            }
        }

        public void OnOpenOrdersDataReceived(OpenOrdersReceivedEventArgs args)
        {
            if(OpenOrdersDataReceived != null)
            {
                OpenOrdersDataReceived(this, args);
            }
        }
    }
}
