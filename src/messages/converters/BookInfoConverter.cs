using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.okitoki.kraken.utils;

namespace com.okitoki.kraken.messages.converters
{
    public class BookInfoConverter : JsonConverter<BookInfo>
    {
        public override BookInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                BookInfo info = new BookInfo();

                JsonDocument doc = JsonDocument.ParseValue(ref reader);

                JsonElement.ArrayEnumerator arrayEnum = doc.RootElement.EnumerateArray();

                info.ChannelID = JsonUtils.ReadInt(ref arrayEnum);

                arrayEnum.MoveNext();
                JsonElement.ObjectEnumerator dataObj = arrayEnum.Current.EnumerateObject();

                while (dataObj.MoveNext())
                {
                    //Read ask price levels
                    if (dataObj.Current.Name.Equals("as"))
                    {
                        ReadAskPrices(info, dataObj.Current);
                    }
                    //Read bid price levels
                    else if (dataObj.Current.Name.Equals("bs"))
                    {
                        ReadBidPrices(info, dataObj.Current);
                    }
                }

                info.ChannelName = JsonUtils.ReadString(ref arrayEnum);
                info.Pair = JsonUtils.ReadString(ref arrayEnum);

                return info;
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to read book: " + e);
                return null;
            }
        }

        private void ReadAskPrices(BookInfo info, JsonProperty dataObj)
        {
            JsonElement.ArrayEnumerator askLevelsArray = dataObj.Value.EnumerateArray();
            while (askLevelsArray.MoveNext())
            {
                BookInfo.PriceLevel priceLevel = new BookInfo.PriceLevel();
                JsonElement.ArrayEnumerator priceLevelValues = askLevelsArray.Current.EnumerateArray();

                priceLevel.price = JsonUtils.ReadDouble(ref priceLevelValues);
                priceLevel.volume = JsonUtils.ReadDouble(ref priceLevelValues);
                priceLevel.timestamp = JsonUtils.ReadDouble(ref priceLevelValues);

                info.Data.askPriceLevels.Add(priceLevel);
            }
        }

        private void ReadBidPrices(BookInfo info, JsonProperty dataObj)
        {
            try
            {
                JsonElement.ArrayEnumerator bidLevelsArray = dataObj.Value.EnumerateArray();
                while (bidLevelsArray.MoveNext())
                {
                    BookInfo.PriceLevel priceLevel = new BookInfo.PriceLevel();
                    JsonElement.ArrayEnumerator priceLevelValues = bidLevelsArray.Current.EnumerateArray();

                    priceLevel.price = JsonUtils.ReadDouble(ref priceLevelValues);
                    priceLevel.volume = JsonUtils.ReadDouble(ref priceLevelValues);
                    priceLevel.timestamp = JsonUtils.ReadDouble(ref priceLevelValues);

                    info.Data.bidPriceLevels.Add(priceLevel);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to read bid prices for book info: " + e);
            }
        }

        public override void Write(Utf8JsonWriter writer, BookInfo value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
