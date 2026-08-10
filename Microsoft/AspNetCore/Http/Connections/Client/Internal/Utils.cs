using System;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal static class Utils
	{
		public static Uri AppendPath(Uri url, string path)
		{
			UriBuilder uriBuilder = new UriBuilder(url);
			if (!uriBuilder.Path.EndsWith("/", StringComparison.Ordinal))
			{
				uriBuilder.Path += "/";
			}
			uriBuilder.Path += path;
			return uriBuilder.Uri;
		}

		internal static Uri AppendQueryString(Uri url, string qs)
		{
			if (string.IsNullOrEmpty(qs))
			{
				return url;
			}
			UriBuilder uriBuilder = new UriBuilder(url);
			string text = uriBuilder.Query;
			if (!string.IsNullOrEmpty(uriBuilder.Query))
			{
				text += "&";
			}
			text += qs;
			if (text.Length > 0 && text[0] == '?')
			{
				text = text.Substring(1);
			}
			uriBuilder.Query = text;
			return uriBuilder.Uri;
		}
	}
}
