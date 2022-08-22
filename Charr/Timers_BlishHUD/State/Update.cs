using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.State
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class Update
	{
		[JsonProperty("name")]
		public string name { get; set; } = "Hero Markers";


		[JsonProperty("createdAt")]
		public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.MinValue;


		[JsonProperty("url")]
		public Uri URL { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("supportedModuleVersion")]
		public string SupportedModuleVersion { get; set; }

		public void Initialize()
		{
			if (CollectionUtilities.IsNullOrEmpty<char>((IEnumerable<char>)Version))
			{
				throw new ArgumentNullException();
			}
		}
	}
}
