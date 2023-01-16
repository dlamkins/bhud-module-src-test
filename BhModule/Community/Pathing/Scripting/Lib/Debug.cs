using System.Collections.Generic;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.Scripting.Lib
{
	public class Debug
	{
		private readonly PathingGlobal _global;

		internal Dictionary<string, object> WatchValues { get; } = new Dictionary<string, object>();


		internal Debug(PathingGlobal global)
		{
			_global = global;
		}

		public void ShowMessage(string message)
		{
			ScreenNotification.ShowNotification(message, (NotificationType)0, (Texture2D)null, 4);
		}

		public void Print(string message)
		{
			_global.ScriptEngine.PushMessage(message);
		}

		public void Info(string message)
		{
			Print(message);
		}

		public void Warn(string message)
		{
			_global.ScriptEngine.PushMessage(message, 1);
		}

		public void Error(string message)
		{
			_global.ScriptEngine.PushMessage(message, 2);
		}

		public void Watch(string key, object value)
		{
			WatchValues[key] = value;
		}

		internal void ClearWatch()
		{
			WatchValues.Clear();
		}

		public void ClearWatch(string key)
		{
			WatchValues.Remove(key);
		}
	}
}
