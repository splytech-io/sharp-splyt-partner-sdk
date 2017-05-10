using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Example.Model
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class PriceRange
    {
        public int Lower { get; set; }
        public int Upper { get; set; }

        public override string ToString() {
            return string.Format("[PriceRange: Lower={0}, Upper={1}]", Lower, Upper);
        }
    }
}
