using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.UI.Configs
{
	public class LfoConfig : ConfigBase
	{
		private Opener.ServerRegion _region;

		public static LfoConfig Default => new LfoConfig
		{
			_region = Opener.ServerRegion.EU
		};

		[JsonProperty("region")]
		public Opener.ServerRegion Region
		{
			get
			{
				return _region;
			}
			set
			{
				_region = value;
				SaveConfig<LfoConfig>(ProofLogix.Instance.LfoConfig);
			}
		}
	}
}
