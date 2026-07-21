using Microsoft.Xna.Framework;
using rp.spark.Models;

namespace rp.spark.UI.Views
{
	internal static class ProfileStatusColors
	{
		public static Color Get(RPStatus status)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(status switch
			{
				RPStatus.Looking => new Color(150, 255, 80), 
				RPStatus.Busy => new Color(255, 150, 40), 
				RPStatus.Offline => new Color(160, 160, 160), 
				RPStatus.Invisible => new Color(226, 226, 226), 
				_ => new Color(120, 210, 255), 
			});
		}
	}
}
