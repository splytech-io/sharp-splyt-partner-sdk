﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Example.Model
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    class Estimate
	{
        public TotalValue Distance { get; set; }
        public TotalValue Duration { get; set; }

        [JsonProperty(PropertyName = "price_range")]
		public PriceRange PriceRange { get; set; }

        public override string ToString() {
            return string.Format("[Estimate: Distance={0}, Duration={1}, PriceRange={2}]", Distance, Duration, PriceRange);
        }
	}

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    class TotalValue 
    {
        public int Total { get; set; }

        public override string ToString() {
            return string.Format("[TotalValue: Total={0}]", Total);
        }
    }
}
