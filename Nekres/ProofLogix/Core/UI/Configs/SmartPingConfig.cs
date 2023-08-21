using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.UI.Configs
{
	public class SmartPingConfig : ConfigBase
	{
		private int _selectedToken;

		private bool _sendProfileId;

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

		[JsonProperty("send_profile_id")]
		public bool SendProfileId
		{
			get
			{
				return _sendProfileId;
			}
			set
			{
				_sendProfileId = value;
				SaveConfig<SmartPingConfig>(ProofLogix.Instance.SmartPingConfig);
			}
		}
	}
}
