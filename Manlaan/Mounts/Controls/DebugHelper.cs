using Blish_HUD;

namespace Manlaan.Mounts.Controls
{
	public static class DebugHelper
	{
		public static bool IsDebugEnabled()
		{
			if (0 == 0)
			{
				return GameService.Debug.get_EnableAdditionalDebugDisplay().get_Value();
			}
			return true;
		}
	}
}
