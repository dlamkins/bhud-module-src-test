using System;

namespace Neokain.GW2.AllianceManager.Services.Connection
{
	public static class EndpointResolver
	{
		public const string LiveHub = "am.neokain.de/api/websockets";

		public const string TestHub = "am-test.neokain.de/api/websockets";

		public const string LiveFrontend = "https://am.neokain.de";

		public const string TestFrontend = "https://am-test.neokain.de";

		public static string ResolveHub(EndpointSelection sel, string envOverride)
		{
			if (!string.IsNullOrWhiteSpace(envOverride))
			{
				return envOverride.Trim();
			}
			if (sel != EndpointSelection.Test)
			{
				return "am.neokain.de/api/websockets";
			}
			return "am-test.neokain.de/api/websockets";
		}

		public static string ResolveFrontend(EndpointSelection sel)
		{
			if (sel != EndpointSelection.Test)
			{
				return "https://am.neokain.de";
			}
			return "https://am-test.neokain.de";
		}

		public static EndpointSelection MigrateLegacyEndpoint(string legacyApiEndpoint)
		{
			if (string.IsNullOrWhiteSpace(legacyApiEndpoint))
			{
				return EndpointSelection.Live;
			}
			string v = legacyApiEndpoint.Trim();
			if (v.StartsWith("am-test.neokain.de/api/websockets", StringComparison.OrdinalIgnoreCase))
			{
				return EndpointSelection.Test;
			}
			if (v.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || v.StartsWith("ws://", StringComparison.OrdinalIgnoreCase))
			{
				return EndpointSelection.Test;
			}
			return EndpointSelection.Live;
		}
	}
}
