using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Example.Model
{
	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
	public class Booking
    {
		[JsonProperty(PropertyName = "booking_id")]
		public string Id { get; set; }
        public string Status { get; set; }

        public override string ToString() {
            return string.Format("[Booking: Id={0}, Status={1}]", Id, Status);
        }
    }
}
