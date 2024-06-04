using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class Resources
	{
		public const int UNSTABLE_COSMIC_ESSENCE = 81743;

		public const int LEGENDARY_DIVINATION = 88485;

		public const int BONESKINNER_RITUAL_VIAL = 93781;

		public const int LEGENDARY_INSIGHT = 77302;

		public const int BANANAS_IN_BULK = 12773;

		public const int BANANAS = 12251;

		public static Resources Empty = new Resources
		{
			IsEmpty = true
		};

		[JsonIgnore]
		public bool IsEmpty { get; private init; }

		[JsonProperty("general_tokens")]
		public List<Resource> GeneralTokens { get; set; }

		[JsonProperty("fractals")]
		public List<Resource> Fractals { get; set; }

		[JsonProperty("raids")]
		public List<Raid> Raids { get; set; }

		[JsonProperty("coffers")]
		public List<Resource> Coffers { get; set; }

		[JsonIgnore]
		public List<Resource> Strikes { get; set; }

		public IEnumerable<Raid.Wing> Wings => Raids.SelectMany((Raid raid) => raid.Wings);

		public IEnumerable<Resource> Items => from resource in Raids.SelectMany((Raid raid) => raid.Wings).SelectMany((Raid.Wing wing) => wing.Events).SelectMany((Raid.Wing.Event ev) => ev.GetTokens())
				.Concat(Fractals)
				.Concat(GeneralTokens)
				.Concat(Coffers)
				.Concat(Strikes)
			group resource by resource.Id into @group
			select @group.First();

		public Resources()
		{
			Coffers = new List<Resource>();
			Raids = new List<Raid>();
			Fractals = new List<Resource>();
			GeneralTokens = new List<Resource>();
			Strikes = new List<Resource>();
			LoadDefaults();
		}

		private void LoadDefaults()
		{
			GeneralTokens.AddRange(new Resource[2]
			{
				new Resource
				{
					Id = 12251
				},
				new Resource
				{
					Id = 12773
				}
			});
		}
	}
}
