using System.Text.Unicode;

namespace System.Text.Encodings.Web
{
	internal abstract class UrlEncoder : TextEncoder
	{
		public static UrlEncoder Default => DefaultUrlEncoder.BasicLatinSingleton;

		public static UrlEncoder Create(TextEncoderSettings settings)
		{
			return new DefaultUrlEncoder(settings);
		}

		public static UrlEncoder Create(params UnicodeRange[] allowedRanges)
		{
			return new DefaultUrlEncoder(new TextEncoderSettings(allowedRanges));
		}
	}
}
