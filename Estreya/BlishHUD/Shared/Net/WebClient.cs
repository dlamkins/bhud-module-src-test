using System;
using System.Net;

namespace Estreya.BlishHUD.Shared.Net
{
	public class WebClient : System.Net.WebClient
	{
		protected override WebRequest GetWebRequest(Uri address)
		{
			string userAgent = base.Headers.Get("User-Agent");
			WebRequest webRequest = base.GetWebRequest(address);
			base.Headers.Set(HttpRequestHeader.UserAgent, userAgent);
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest != null)
			{
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.UserAgent = userAgent;
			}
			return webRequest;
		}
	}
}
