using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.UI.Configs
{
	internal class ProfileConfig : ConfigBase
	{
		private int _selectedTab;

		public static ProfileConfig Default => new ProfileConfig
		{
			_selectedTab = 1
		};

		[JsonProperty("selected_tab")]
		public int SelectedTab
		{
			get
			{
				return _selectedTab;
			}
			set
			{
				_selectedTab = value;
				SaveConfig<ProfileConfig>(ProofLogix.Instance.ProfileConfig);
			}
		}
	}
}
