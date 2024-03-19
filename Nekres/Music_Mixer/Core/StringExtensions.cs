using System;
using System.Text.RegularExpressions;

namespace Nekres.Music_Mixer.Core
{
	public static class StringExtensions
	{
		public static string SplitCamelCase(this string input)
		{
			return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
		}

		public static bool IsWebLink(this string uri)
		{
			if (Uri.TryCreate(uri, UriKind.Absolute, out var uriResult))
			{
				if (!(uriResult.Scheme == Uri.UriSchemeHttp))
				{
					return uriResult.Scheme == Uri.UriSchemeHttps;
				}
				return true;
			}
			return false;
		}
	}
}
