using System;
using System.Reflection;
using Gw2Sharp;
using Gw2Sharp.WebApi;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class Gw2SharpHelper
	{
		public static RenderUrl CreateRenderUrl(IConnection connection, string url)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrWhiteSpace(url))
			{
				return default(RenderUrl);
			}
			return (RenderUrl)typeof(RenderUrl).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[3]
			{
				typeof(IGw2Client),
				typeof(string),
				typeof(string)
			}, null).Invoke(new object[3]
			{
				(object)new Gw2Client(connection),
				url,
				connection.get_RenderBaseUrl()
			});
		}
	}
}
