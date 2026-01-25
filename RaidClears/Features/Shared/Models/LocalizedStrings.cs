using System;
using Newtonsoft.Json;

namespace RaidClears.Features.Shared.Models
{
	[Serializable]
	public class LocalizedStrings
	{
		[JsonProperty("en")]
		public string? En { get; set; }

		[JsonProperty("fr")]
		public string? Fr { get; set; }

		[JsonProperty("de")]
		public string? De { get; set; }

		[JsonProperty("es")]
		public string? Es { get; set; }

		public string? GetValue(string locale)
		{
			if (locale == null)
			{
				return En;
			}
			return locale.ToLowerInvariant() switch
			{
				"fr" => Fr, 
				"de" => De, 
				"es" => Es, 
				"en" => En, 
				_ => En, 
			};
		}
	}
}
