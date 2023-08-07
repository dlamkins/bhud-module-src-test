using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.UI.Configs
{
	public class SmartPingConfig : ConfigBase
	{
		private int _selectedToken;

		public static SmartPingConfig Default => new SmartPingConfig
		{
			_selectedToken = 77302
		};

		[JsonProperty("selected_token")]
		public int SelectedToken
		{
			get
			{
				return _selectedToken;
			}
			set
			{
				_selectedToken = value;
				SaveConfig<SmartPingConfig>(ProofLogix.Instance.SmartPingConfig);
			}
		}
	}
}
