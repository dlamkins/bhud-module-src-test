using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using LiteDB;

namespace Nekres.Music_Mixer.Core.Services.Data
{
	public class Playlist : DbEntity
	{
		[BsonIgnore]
		public bool IsEmpty
		{
			get
			{
				List<AudioSource> tracks = Tracks;
				if (tracks == null)
				{
					return true;
				}
				return !tracks.Any();
			}
		}

		[BsonField("external_id")]
		public string ExternalId { get; set; }

		[BsonField("enabled")]
		public bool Enabled { get; set; }

		[BsonField("playlist")]
		[BsonRef("audio_sources")]
		public List<AudioSource> Tracks { get; set; }

		public AudioSource GetRandom(int dayCycle)
		{
			return dayCycle switch
			{
				1 => GetRandom(AudioSource.DayCycle.Dawn), 
				2 => GetRandom(AudioSource.DayCycle.Day), 
				3 => GetRandom(AudioSource.DayCycle.Dusk), 
				4 => GetRandom(AudioSource.DayCycle.Night), 
				_ => AudioSource.Empty, 
			};
		}

		private AudioSource GetRandom(AudioSource.DayCycle dayCycle)
		{
			List<AudioSource> tracks = Tracks;
			if (tracks == null || !tracks.Any())
			{
				return AudioSource.Empty;
			}
			List<AudioSource> cycle = Tracks.Where((AudioSource x) => x.HasDayCycle(dayCycle)).ToList();
			if (!cycle.Any())
			{
				return AudioSource.Empty;
			}
			return cycle.ElementAt(RandomUtil.GetRandom(0, cycle.Count - 1));
		}
	}
}
