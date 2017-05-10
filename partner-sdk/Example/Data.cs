using System;

namespace Example
{
    public class Data
    {
		public static readonly Object PICKUP_LOCATION = new {
			latitude = 51.902854,
			longitude = -0.211351,
		};

		public static readonly Object PICKUP = new {
			address = "10 Argyle Way, Stevenage SG1 2AD",
			location = PICKUP_LOCATION,
		};

		public static readonly Object DROPOFF_LOCATION = new {
			latitude = 51.763860,
			longitude = -0.235458,
		};

		public static readonly Object DROPOFF = new {
			address = "53 De Havilland Cl, Hatfield AL10 0DS",
			location = DROPOFF_LOCATION,
		};

		public static readonly Object PASSENGER = new {
			user_id = "poiqwe31121",
			email = "tes42342311t@test.com",
			first_name = "John",
			last_name = "Mullock",
			profile_picture = "https://static.splytech.io/partner-logos/splyt.png",
			phone = new {
				country_code = "44",
				number = "749511377715",
			},
			payment = new {
				token = "15666511999abc",
				display_name = "**** 0667",
			},
		};

    }
}
