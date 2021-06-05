﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace com.okitoki.kraken.messages
{
    public class WebSocketToken
    {
        [JsonPropertyName("token")]
        [JsonInclude]
        public string Token { get; set; }

        
        [JsonPropertyName("expires")]
        [JsonInclude]
        public int Expires { get; set; }
    }
}
