﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Example.Model
{
	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
	class Partner
	{
		[JsonProperty(PropertyName = "_id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "display_name")]
		public string DisplayName { get; set; }

        public override string ToString() {
            return string.Format("[Partner: Id={0}, DisplayName={1}]", Id, DisplayName);
        }
	}
}
