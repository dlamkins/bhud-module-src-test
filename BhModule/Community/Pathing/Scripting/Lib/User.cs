using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.Scripting.Lib
{
	public class User
	{
		private readonly PathingGlobal _global;

		public User(PathingGlobal global)
		{
			_global = global;
		}

		public bool SetClipboard(string value)
		{
			return SetClipboard(value, $"'{value}' copied to clipboard.");
		}

		public bool SetClipboard(string value, string message)
		{
			if (_global.ScriptEngine.Module.Settings.PackMarkerConsentToClipboard.get_Value() == MarkerClipboardConsentLevel.Never)
			{
				return false;
			}
			ClipboardUtil.get_WindowsClipboardService().SetTextAsync(value).ContinueWith(delegate(Task<bool> t)
			{
				if (t.IsCompleted && t.Result)
				{
					ScreenNotification.ShowNotification(message, (NotificationType)0, (Texture2D)null, 2);
				}
			});
			return true;
		}
	}
}
