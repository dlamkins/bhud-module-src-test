using System.Net;
using System.Net.Http;
using Flurl.Http.Configuration;

namespace Estreya.BlishHUD.Shared.Net
{
	public class FlurlHttpClientFactory : DefaultHttpClientFactory
	{
		public override HttpMessageHandler CreateMessageHandler()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			HttpClientHandler val = new HttpClientHandler();
			val.set_AutomaticDecompression(DecompressionMethods.GZip | DecompressionMethods.Deflate);
			return (HttpMessageHandler)val;
		}
	}
}
