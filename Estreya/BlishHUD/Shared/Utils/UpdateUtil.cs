using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Threading;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class UpdateUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(UpdateUtil));

		private static readonly SynchronizedCollection<IntPtr> _asyncStateMonitor = new SynchronizedCollection<IntPtr>();

		public static void Update(Action<GameTime> call, GameTime gameTime, double interval, ref double lastCheck)
		{
			lastCheck += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (lastCheck >= interval)
			{
				call(gameTime);
				lastCheck = 0.0;
			}
		}

		public static void Update(Action call, GameTime gameTime, double interval, ref double lastCheck)
		{
			lastCheck += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (lastCheck >= interval)
			{
				call();
				lastCheck = 0.0;
			}
		}

		public static async Task UpdateAsync(Func<GameTime, Task> call, GameTime gameTime, double interval, AsyncRef<double> lastCheck, bool doLogging = true, TaskCreationOptions taskCreationOptions = TaskCreationOptions.None)
		{
			lastCheck.Value += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (lastCheck.Value < interval || _asyncStateMonitor.Contains(call.Method.MethodHandle.Value))
			{
				return;
			}
			_asyncStateMonitor.Add(call.Method.MethodHandle.Value);
			string methodName = call.Target.GetType().FullName + "." + call.Method.Name + "()";
			if (doLogging)
			{
				Logger.Debug("Start running update function '{0}'.", new object[1] { methodName });
			}
			try
			{
				await Task.Factory.StartNew(() => call(gameTime), taskCreationOptions).Unwrap();
				lastCheck.Value = 0.0;
			}
			finally
			{
				_asyncStateMonitor.Remove(call.Method.MethodHandle.Value);
			}
			if (doLogging)
			{
				Logger.Debug("Update function '{0}' finished running.", new object[1] { methodName });
			}
		}

		public static async Task UpdateAsync(Func<Task> call, GameTime gameTime, double interval, AsyncRef<double> lastCheck, bool doLogging = true, TaskCreationOptions taskCreationOptions = TaskCreationOptions.None)
		{
			lastCheck.Value += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (!(lastCheck.Value < interval) && !_asyncStateMonitor.Contains(call.Method.MethodHandle.Value))
			{
				_asyncStateMonitor.Add(call.Method.MethodHandle.Value);
				string methodName = call.Target.GetType().FullName + "." + call.Method.Name + "()";
				if (doLogging)
				{
					Logger.Debug("Start running update function '{0}'.", new object[1] { methodName });
				}
				try
				{
					await Task.Factory.StartNew(call, taskCreationOptions).Unwrap();
					lastCheck.Value = 0.0;
				}
				finally
				{
					_asyncStateMonitor.Remove(call.Method.MethodHandle.Value);
				}
				if (doLogging)
				{
					Logger.Debug("Update function '{0}' finished running.", new object[1] { methodName });
				}
			}
		}
	}
}
