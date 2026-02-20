using Blish_HUD.Content;
using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public abstract class StreamDataBase : IStreamData
	{
		public abstract string Id { get; set; }

		public abstract string Name { get; }

		public abstract string TypeString { get; set; }

		public abstract string Url { get; set; }

		public abstract string InfoUrl { get; set; }

		public abstract string StaticImage { get; set; }

		[JsonIgnore]
		public StreamType Type => StreamTypeExtensions.ParseFromString(TypeString);

		[JsonIgnore]
		public bool IsRadio => Type == StreamType.Radio;

		[JsonIgnore]
		public AsyncTexture2D StaticImageTexture { get; set; }
	}
}
