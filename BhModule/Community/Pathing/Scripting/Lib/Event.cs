using System;
using Microsoft.Xna.Framework;
using Neo.IronLua;

namespace BhModule.Community.Pathing.Scripting.Lib
{
	public class Event
	{
		private const int TICK_FREQUENCY = 0;

		private readonly SafeList<Func<GameTime, LuaResult>> _tickListeners = new SafeList<Func<GameTime, LuaResult>>();

		private double _nextTick;

		private readonly PathingGlobal _global;

		internal Event(PathingGlobal global)
		{
			_global = global;
		}

		public void OnTick(Func<GameTime, LuaResult> callback)
		{
			if (callback != null)
			{
				_tickListeners.Add(callback);
			}
		}

		internal void Update(GameTime gameTime)
		{
			if (!(gameTime.get_TotalGameTime().TotalMilliseconds > _nextTick))
			{
				return;
			}
			_nextTick = gameTime.get_TotalGameTime().TotalMilliseconds + 0.0;
			Func<GameTime, LuaResult>[] array = _tickListeners.ToArray();
			foreach (Func<GameTime, LuaResult> listener in array)
			{
				if (!_global.ScriptEngine.WrapScriptCall(() => listener(gameTime)).Success)
				{
					_global.ScriptEngine.PushMessage("Tick callback `" + listener.Method.Name + "` was unregistered because it threw an exception.", ScriptMessageLogLevel.System);
					_tickListeners.Remove(listener);
				}
			}
		}
	}
}
