using Blish_HUD.Input;
using Newtonsoft.Json;

namespace Nekres.Mumble_Info.Core.UI
{
	internal class MumbleConfig : ConfigBase
	{
		private bool _swapYZ;

		private KeyBinding _shortcut;

		[JsonProperty("swap_yz_axis")]
		public bool SwapYZ
		{
			get
			{
				return _swapYZ;
			}
			set
			{
				if (SetProperty(ref _swapYZ, value))
				{
					SaveConfig<MumbleConfig>(MumbleInfoModule.Instance.MumbleConfig);
				}
			}
		}

		[JsonProperty("shortcut")]
		public KeyBinding Shortcut
		{
			get
			{
				return _shortcut;
			}
			set
			{
				if (SetProperty(ref _shortcut, value))
				{
					SaveConfig<MumbleConfig>(MumbleInfoModule.Instance.MumbleConfig);
				}
			}
		}

		protected override void BindingChanged()
		{
			SaveConfig<MumbleConfig>(MumbleInfoModule.Instance.MumbleConfig);
		}
	}
}
