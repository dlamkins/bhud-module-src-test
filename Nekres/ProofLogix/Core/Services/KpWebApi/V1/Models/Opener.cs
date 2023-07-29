using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models
{
	public sealed class Opener : BaseResponse
	{
		public enum ServerRegion
		{
			EU,
			NA
		}

		public static Opener Empty = new Opener
		{
			IsEmpty = true
		};

		public bool IsEmpty { get; private init; }

		[JsonProperty("encounter")]
		public string Encounter { get; set; }

		[JsonProperty("region")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ServerRegion Region { get; set; }

		[JsonProperty("opener")]
		public List<Volunteer> Volunteers { get; set; }
	}
}
