using Nekres.Music_Mixer.Core.Services;

namespace Nekres.Music_Mixer
{
	internal static class Gw2StateExtensions
	{
		public static bool IsIntermediate(this Gw2StateService.State state)
		{
			if ((uint)state <= 2u || state == Gw2StateService.State.Defeated)
			{
				return true;
			}
			return false;
		}
	}
}
