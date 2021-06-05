using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.utils
{
    public class JsonUtils
    {
        public static int ReadInt(ref JsonElement.ArrayEnumerator valueArray)
        {
            valueArray.MoveNext();
            try
            {
                return valueArray.Current.GetInt32();
            }
            catch(Exception e)
            {
                return int.Parse(valueArray.Current.GetString());
            }
        }

        public static double ReadDouble(ref JsonElement.ArrayEnumerator valueArray)
        {
            valueArray.MoveNext();
            try
            {
                return valueArray.Current.GetDouble();
            }
            catch (Exception e)
            {
                return double.Parse(valueArray.Current.GetString());
            }
        }

        public static string ReadString(ref JsonElement.ArrayEnumerator valueArray)
        {
            valueArray.MoveNext();
            return valueArray.Current.GetString();
        }

        public static int ReadInt(ref JsonElement.ObjectEnumerator objectEnum)
        {
            objectEnum.MoveNext();
            try
            {
                return objectEnum.Current.Value.GetInt32();
            }
            catch (Exception e)
            {
                return int.Parse(objectEnum.Current.Value.GetString());
            }
        }

        public static double ReadDouble(ref JsonElement.ObjectEnumerator objectEnum)
        {
            objectEnum.MoveNext();
            try
            {
                return objectEnum.Current.Value.GetDouble();
            }
            catch (Exception e)
            {
                return double.Parse(objectEnum.Current.Value.GetString());
            }
        }

        public static string ReadString(ref JsonElement.ObjectEnumerator objectEnum)
        {
            objectEnum.MoveNext();
            return objectEnum.Current.Value.GetString();
        }

        public static int ReadInt(JsonProperty prop)
        {
            try
            {
                return prop.Value.GetInt32();
            }
            catch (Exception e)
            {
                return int.Parse(prop.Value.GetString());
            }
        }

        public static double ReadDouble(JsonProperty prop)
        {
            try
            {
                return prop.Value.GetDouble();
            }
            catch (Exception e)
            {
                return double.Parse(prop.Value.GetString());
            }
        }

        public static string ReadString(JsonProperty prop)
        {
            return prop.Value.GetString();
        }
    }
}
