using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Example.Model
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Journey
    {
		[JsonProperty(PropertyName = "_id")]
		public string Id { get; set; }

        public string State { get; set; }
        public string Type { get; set; }
        public Driver driver { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        [JsonProperty(PropertyName = "passenger_groups")]
        public List<PassengerGroup> PassengerGroups { get; set; }

        [JsonProperty(PropertyName = "actions_sequence")]
        public object ActionSequence { get; set; }

        public override string ToString() {
            return string.Format("[Journey: Id={0}, State={1}, Type={2}, driver={3}, created={4}, updated={5}, PassengerGroups={6}]", Id, State, Type, driver, created, updated, PassengerGroups);
        }
    }
}
