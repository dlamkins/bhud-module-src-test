using Blish_HUD.Content;

namespace CinemaModule.Models
{
	public interface IStreamData
	{
		string Id { get; }

		string Name { get; }

		string TypeString { get; }

		string Url { get; }

		string InfoUrl { get; }

		string StaticImage { get; }

		StreamType Type { get; }

		bool IsRadio { get; }

		AsyncTexture2D StaticImageTexture { get; set; }
	}
}
