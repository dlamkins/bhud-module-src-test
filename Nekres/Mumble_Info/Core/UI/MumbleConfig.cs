using Newtonsoft.Json;

namespace Nekres.Mumble_Info.Core.UI
{
	internal class MumbleConfig : ConfigBase
	{
		private bool _swapYZ;

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
	}
}
