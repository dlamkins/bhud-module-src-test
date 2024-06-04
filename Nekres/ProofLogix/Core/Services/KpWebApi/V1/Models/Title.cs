using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models
{
	[JsonConverter(typeof(TitleConverter))]
	public sealed class Title
	{
		public enum TitleMode
		{
			Unknown,
			Raid,
			Fractal,
			Strike
		}

		public string Name { get; set; }

		public TitleMode Mode { get; set; }
	}
}
