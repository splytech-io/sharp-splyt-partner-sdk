using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Example.Model
{
	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Driver
    {
		[JsonProperty(PropertyName = "_id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "first_name")]
		public string FirstName { get; set; }

		[JsonProperty(PropertyName = "last_name")]
		public string LastName { get; set; }

		//    {
		//  "_id": "5645bac81439193b6899c0a6",
		//  "last_name": "15",
		//  "first_name": "Driver",
		//  "geo": {
		//    "bearing": 0,
		//    "speed": 0,
		//    "location": {
		//      "latitude": 51.90376,
		//      "longitude": -0.1966117
		//    },
		//    "updated": "2017-05-10T16:22:34.693Z",
		//    "polyline": {
		//      "updated": "2016-11-29T16:56:25.594Z",
		//      "data": "upr{Hfyc@??"
		//    },
		//    "accuracy": 3.9
		//  },
		//  "car": {
		//    "manufacturer": "Nissan",
		//    "model": "Panda",
		//    "plate_number": "HUY069",
		//    "color": "Red",
		//    "picture": "",
		//    "type": "standard"
		//  },
		//  "profile_picture": "og/gg/gi/photo-on-26-01-2017-at-11_38.jpg",
		//  "metadata": {},
		//  "rating": 0.94082840236686394
		//}

        public override string ToString() {
            return string.Format("[Driver: Id={0}, FirstName={1}, LastName={2}]", Id, FirstName, LastName);
        }
	}
}
