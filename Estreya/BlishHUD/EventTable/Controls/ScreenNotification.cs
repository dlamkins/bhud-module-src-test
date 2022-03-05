using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.EventTable.Controls
{
	public static class ScreenNotification
	{
		public static void ShowNotification(string message, NotificationType type = 0, Texture2D icon = null, int duration = 4)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			ShowNotification(new string[1] { message }, type, icon, duration);
		}

		public static void ShowNotification(string[] messages, NotificationType type = 0, Texture2D icon = null, int duration = 4)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			for (int i = messages.Length - 1; i >= 0; i--)
			{
				ScreenNotification.ShowNotification(messages[i], type, icon, duration);
			}
		}
	}
}
