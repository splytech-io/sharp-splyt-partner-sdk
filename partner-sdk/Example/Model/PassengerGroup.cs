using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Example.Model
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class PassengerGroup
    {
		[JsonProperty(PropertyName = "_id")]
		public string Id { get; set; }
        public string State { get; set; }

     //   {
     //   "state": "waiting",
     //   "_id": "59133e53e1f27d000a1e25c0",
     //   "created": "2017-05-10T16:22:45.799Z",
     //   "passengers": [
     //     {
     //       "passenger": {
     //         "_id": "59132972cf284c1657ff0415",
     //         "first_name": "John",
     //         "stats": {
     //           "number_of_pool_journeys": 0,
     //           "number_of_journeys": 3
	 //         },
      //        "flags": {
      //          "is_facebook_linked": false
      //        },
      //        "rating": 1
      //      },
      //      "payment_id": "59132972d65092000aefd264",
      //      "phone_number": "+44749511377715",
      //      "_id": "59133e53e1f27d000a1e25c1",
      //      "mutual_friends": {
      //        "items": [],
      //        "total_number": 0
      //      },
      //      "invitation_status": "initiator"
      //    }
      //  ],
      //  "estimated": {
      //    "classic": {
      //      "pickup_eta": 180,
      //      "price_range": {
      //        "lower": 2596,
      //        "upper": 3115
      //      },
      //      "duration": {
      //        "total": 1014
      //      },
      //      "distance": {
      //        "total": 17690.1
      //      }
      //    }
      //  },
      //  "dropoff": {
      //    "address": "53 De Havilland Cl, Hatfield AL10 0DS",
      //    "location": {
      //      "latitude": 51.76386,
      //      "longitude": -0.235458
      //    }
      //  },
      //  "pickup": {
      //    "address": "10 Argyle Way, Stevenage SG1 2AD",
      //    "location": {
      //      "latitude": 51.902854,
      //      "longitude": -0.211351
      //    }
      //  },
      //  "configuration": {
      //    "booking_type": "now",
      //    "car_type": "standard",
      //    "pricing_type": "e-taximeter",
      //    "payment_type": "online"
      //  }
      //}

        public override string ToString() {
            return string.Format("[PassengerGroup: Id={0}, State={1}]", Id, State);
        }

    }

}
