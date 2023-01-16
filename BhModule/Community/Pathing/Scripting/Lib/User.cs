using Blish_HUD;

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
			ClipboardUtil.get_WindowsClipboardService().SetTextAsync(value);
			return true;
		}
	}
}
