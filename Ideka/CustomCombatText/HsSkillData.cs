using System.Net.Http;
using System.Runtime.CompilerServices;
using HsAPI;

namespace Ideka.CustomCombatText
{
	public class HsSkillData : HsApiCache<int, Skill>
	{
		[CompilerGenerated]
		private HttpClient _003Cclient_003EP;

		protected override string Endpoint => "skills";

		protected override HttpClient Client => _003Cclient_003EP;

		public HsSkillData(HttpClient client)
		{
			_003Cclient_003EP = client;
			base._002Ector();
		}
	}
}
