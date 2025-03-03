using System.Net.Http;
using System.Runtime.CompilerServices;
using HsAPI;

namespace Ideka.CustomCombatText
{
	public class HsPaletteData : HsApiCache<int, Palette>
	{
		[CompilerGenerated]
		private HttpClient _003Cclient_003EP;

		protected override string Endpoint => "traits";

		protected override HttpClient Client => _003Cclient_003EP;

		public HsPaletteData(HttpClient client)
		{
			_003Cclient_003EP = client;
			base._002Ector();
		}
	}
}
