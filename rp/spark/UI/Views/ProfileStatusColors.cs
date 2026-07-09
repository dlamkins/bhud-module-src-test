using Microsoft.Xna.Framework;
using rp.spark.Models;

namespace rp.spark.UI.Views
{
	internal static class ProfileStatusColors
	{
		public static Color Get(RPStatus status)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(status switch
			{
				RPStatus.Looking => new Color(150, 255, 80), 
				RPStatus.Busy => new Color(255, 150, 40), 
				RPStatus.Offline => new Color(160, 160, 160), 
				_ => new Color(120, 210, 255), 
			});
		}
	}
}
