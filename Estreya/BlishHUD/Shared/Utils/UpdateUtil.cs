using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.Shared.Threading;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class UpdateUtil
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(UpdateUtil));

		private static readonly ConcurrentDictionary<IntPtr, Task> _asyncStateMonitor = new ConcurrentDictionary<IntPtr, Task>();

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

		public static async Task<Task> UpdateAsync(Func<GameTime, Task> call, GameTime gameTime, double interval, AsyncRef<double> lastCheck, bool doLogging = true, TaskCreationOptions taskCreationOptions = TaskCreationOptions.None)
		{
			lastCheck.Value += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (lastCheck.Value < interval)
			{
				return Task.CompletedTask;
			}
			if (_asyncStateMonitor.TryGetValue(call.Method.MethodHandle.Value, out var savedTask))
			{
				return savedTask;
			}
			string methodName = call.Target.GetType().FullName + "." + call.Method.Name + "()";
			if (doLogging)
			{
				Logger.Debug("Start running update function '{0}'.", new object[1] { methodName });
			}
			Task task = Task.Factory.StartNew(() => call(gameTime), taskCreationOptions).Unwrap();
			_asyncStateMonitor.AddOrUpdate(call.Method.MethodHandle.Value, task, (IntPtr _, Task _) => task);
			try
			{
				await task;
				lastCheck.Value = 0.0;
			}
			finally
			{
				_asyncStateMonitor.TryRemove(call.Method.MethodHandle.Value, out var _);
			}
			if (doLogging)
			{
				Logger.Debug("Update function '{0}' finished running.", new object[1] { methodName });
			}
			return task;
		}

		public static async Task<Task> UpdateAsync(Func<Task> call, GameTime gameTime, double interval, AsyncRef<double> lastCheck, bool doLogging = true, TaskCreationOptions taskCreationOptions = TaskCreationOptions.None)
		{
			lastCheck.Value += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (lastCheck.Value < interval)
			{
				return Task.CompletedTask;
			}
			if (_asyncStateMonitor.TryGetValue(call.Method.MethodHandle.Value, out var savedTask))
			{
				return savedTask;
			}
			string methodName = call.Target.GetType().FullName + "." + call.Method.Name + "()";
			if (doLogging)
			{
				Logger.Debug("Start running update function '{0}'.", new object[1] { methodName });
			}
			Task task = Task.Factory.StartNew(call, taskCreationOptions).Unwrap();
			_asyncStateMonitor.AddOrUpdate(call.Method.MethodHandle.Value, task, (IntPtr _, Task _) => task);
			try
			{
				await task;
				lastCheck.Value = 0.0;
			}
			finally
			{
				_asyncStateMonitor.TryRemove(call.Method.MethodHandle.Value, out var _);
			}
			if (doLogging)
			{
				Logger.Debug("Update function '{0}' finished running.", new object[1] { methodName });
			}
			return task;
		}
	}
}
