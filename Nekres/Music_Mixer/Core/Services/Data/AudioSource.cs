using System;
using Blish_HUD;
using Blish_HUD.Content;
using LiteDB;

namespace Nekres.Music_Mixer.Core.Services.Data
{
	public class AudioSource : DbEntity
	{
		[Flags]
		public enum DayCycle
		{
			None = 0x0,
			Dawn = 0x1,
			Day = 0x3,
			Dusk = 0x4,
			Night = 0xC
		}

		public static AudioSource Empty = new AudioSource
		{
			Title = string.Empty,
			Uploader = string.Empty,
			PageUrl = string.Empty,
			AudioUrl = string.Empty,
			IsEmpty = true
		};

		private float _volume;

		private AsyncTexture2D _thumbnail;

		[BsonIgnore]
		public bool IsEmpty { get; private init; }

		[BsonIgnore]
		public Gw2StateService.State State { get; set; }

		[BsonField("external_id")]
		public string ExternalId { get; set; }

		[BsonField("page_url")]
		public string PageUrl { get; set; }

		[BsonField("title")]
		public string Title { get; set; }

		[BsonField("uploader")]
		public string Uploader { get; set; }

		[BsonField("duration")]
		public TimeSpan Duration { get; set; }

		[BsonField("volume")]
		public float Volume
		{
			get
			{
				return _volume;
			}
			set
			{
				_volume = value;
				this.VolumeChanged?.Invoke(this, new ValueEventArgs<float>(value));
			}
		}

		[BsonField("day_cycles")]
		public DayCycle DayCycles { get; set; }

		[BsonIgnore]
		public AsyncTexture2D Thumbnail
		{
			get
			{
				if (_thumbnail != null)
				{
					return _thumbnail;
				}
				_thumbnail = MusicMixer.Instance.Data.GetThumbnail(this);
				return _thumbnail;
			}
		}

		[BsonField("audio_url")]
		public string AudioUrl { get; set; }

		public event EventHandler<ValueEventArgs<float>> VolumeChanged;

		public bool HasDayCycle(DayCycle cycle)
		{
			return (DayCycles & cycle) == cycle;
		}
	}
}
