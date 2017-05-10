﻿using System;
using System.Threading.Tasks;
using SplytPartnerSDK;
using Example.Model;

namespace Example
{

    /// <summary>
    /// Example usage of SplytPartnerSdk
    /// </summary>
    class MainClass
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            new MainClass().Run();
			while (true) System.Console.ReadKey();
        }

        /// <summary>
        /// Run this instance.
        /// </summary>
        public async Task Run() {
			try {
                await RunExample();
			} catch (Exception ex) {
                Console.WriteLine($"--> Exception during example (See ResponseErrorException), best to handle per request [{ex.Message}]");
			}
        }

        /// <summary>
        /// Run this instance.
        /// </summary>
        /// <returns>The run.</returns>
		public async Task RunExample()
		{
            Partner partner;
            Estimate estimate;
            Booking booking;
            Journey journey;

            string login = Environment.GetEnvironmentVariable("partner_login");
            string password = Environment.GetEnvironmentVariable("partner_password");

            // - Create client object (handles re/connection)
            Client client = new SplytPartnerSDK.Client("wss://wsapi.dev.splytech.io", login, password) {
                //DEBUG = true,
            };

            Console.WriteLine("Ready.");

            client.On((obj) => Console.WriteLine($"Unhandled push: [{obj}]"));

            // 1. Get Operator for pickup location
            var partnerResponse = await client.Request("partner.roaming.operator.get", new {
                location = Data.PICKUP_LOCATION,
            });
            partner = partnerResponse.GetValue("partner").ToObject<Partner>();
            Console.WriteLine($"Partner: [{partner}]");

            // 2. Get estimate for the journey
            var estimateResponse = await client.Request("partner.roaming.operator.estimate", new {
                pickup = Data.PICKUP,
                dropoff = Data.DROPOFF,
            });
            estimate = estimateResponse.GetValue("estimate").ToObject<Estimate>();
            Console.WriteLine($"Estimate: [{estimate}]");

			// 3. Create booking
			var createBookingResponse = await client.Request("partner.roaming.booking.create", new {
				pickup = Data.PICKUP,
				dropoff = Data.DROPOFF,
                passenger = Data.PASSENGER,
			});
            booking = createBookingResponse.ToObject<Booking>();
			Console.WriteLine($"Booking: [{booking}]");

            // 4. Handle journey created
            client.On("partner.roaming.journey.created", (SplytPartnerSDK.Model.OnPushArgs obj) => {
                journey = obj.Data.GetValue("journey").ToObject<Journey>();
                Console.WriteLine($"Journey created: [{journey}]");
            });

            // 5. Handle journey updated
            client.On("partner.roaming.journey.updated", (SplytPartnerSDK.Model.OnPushArgs obj) => {
                journey = obj.Data.GetValue("journey").ToObject<Journey>();
                var group = journey.PassengerGroups[0];
                Console.WriteLine($"Journey updated, state: [{group.State}]");
            });

        }

    }

}
