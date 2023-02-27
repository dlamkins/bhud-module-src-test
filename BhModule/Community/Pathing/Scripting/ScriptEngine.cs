using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Scripting.Extensions;
using Blish_HUD;
using Blish_HUD.Debug;
using Humanizer;
using Microsoft.Xna.Framework;
using Neo.IronLua;
using TmfLib;

namespace BhModule.Community.Pathing.Scripting
{
	public class ScriptEngine
	{
		public struct ScriptMessage
		{
			public DateTime Timestamp { get; }

			public string Message { get; }

			public string Source { get; }

			public int LogLevel { get; }

			public ScriptMessage(string message, string source, DateTime timestamp, int logLevel = 0)
			{
				Message = message;
				Source = source;
				Timestamp = timestamp;
				LogLevel = logLevel;
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<ScriptEngine>();

		private Lua _lua;

		private TraceLineDebugger _stackTraceDebugger;

		private readonly RingBuffer<TimeSpan> _frameExecutionTime = new RingBuffer<TimeSpan>(10);

		private TimeSpan _currentFrameDuration = TimeSpan.Zero;

		public readonly SafeList<ScriptMessage> OutputMessages = new SafeList<ScriptMessage>();

		internal PathingModule Module { get; }

		public PathingGlobal Global { get; private set; }

		public SafeList<ScriptState> Scripts { get; } = new SafeList<ScriptState>();


		public TimeSpan FrameExecutionTime
		{
			get
			{
				TimeSpan[] frameTimes = _frameExecutionTime.get_InternalBuffer().ToArray();
				TimeSpan averageFrameTime = TimeSpan.Zero;
				TimeSpan[] array = frameTimes;
				foreach (TimeSpan frameTime in array)
				{
					averageFrameTime += frameTime;
				}
				return TimeSpanExtension.Divide(averageFrameTime, frameTimes.Length);
			}
		}

		public ScriptEngine(PathingModule module)
		{
			Module = module;
			LuaType.RegisterTypeExtension(typeof(StandardMarkerScriptExtensions));
			LuaType.RegisterTypeExtension(typeof(PathingCategoryScriptExtensions));
			LuaType.RegisterTypeExtension(typeof(GuidExtensions));
		}

		private void BuildEnv()
		{
			_lua?.Dispose();
			_lua = new Lua(LuaIntegerType.Int32, LuaFloatType.Float);
			StandardMarkerScriptExtensions.SetPackInitiator(Module.PackInitiator);
			PathingCategoryScriptExtensions.SetPackInitiator(Module.PackInitiator);
			_stackTraceDebugger = new TraceLineDebugger();
			Global = _lua.CreateEnvironment<PathingGlobal>();
			Global.ScriptEngine = this;
			PushMessage("Loaded new environment.", -1);
		}

		public void PushMessage(string message, int logLevel = 0, DateTime? timestamp = null, string source = null)
		{
			DateTime valueOrDefault = timestamp.GetValueOrDefault();
			if (!timestamp.HasValue)
			{
				valueOrDefault = DateTime.UtcNow;
				timestamp = valueOrDefault;
			}
			if (logLevel == -1)
			{
				source = "system";
			}
			OutputMessages.Add(new ScriptMessage(message, source ?? _stackTraceDebugger.LastFrameSource ?? "unknown", timestamp.Value, logLevel));
		}

		private void PublishException(LuaException ex)
		{
			int sourceLine = ex.Line;
			string source = ex.FileName ?? ex.Source;
			string source2 = ex.Source;
			if (source2 == "Anonymously Hosted DynamicMethods Assembly" || source2 == "Neo.Lua")
			{
				source = _stackTraceDebugger.LastFrameSource;
				sourceLine = _stackTraceDebugger.LastFrameLine;
			}
			string message = (string.IsNullOrEmpty(ex.FileName) ? $"{ex.Message.TrimEnd('.')} during execution on or around line {sourceLine} in '{_stackTraceDebugger.LastFrameScope}'." : $"{ex.Message.TrimEnd('.')} on or around line {sourceLine} column {ex.Column}.");
			if (!(ex is LuaParseException))
			{
				if (ex is LuaRuntimeException)
				{
					source2 = source;
					PushMessage(message, 2, null, source2);
				}
			}
			else
			{
				source2 = source;
				PushMessage(message, 2, null, source2);
			}
		}

		public (LuaResult Result, bool Success) WrapScriptCall(Func<LuaResult> scriptCallDelegate)
		{
			Stopwatch runClock = Stopwatch.StartNew();
			LuaResult result = null;
			bool success = true;
			LuaRuntimeException lre = default(LuaRuntimeException);
			try
			{
				result = scriptCallDelegate();
			}
			catch (LuaRuntimeException ex2)
			{
				success = false;
				PublishException(ex2);
			}
			catch (TargetInvocationException ex3) when (((Func<bool>)delegate
			{
				// Could not convert BlockContainer to single expression
				lre = ex3.InnerException as LuaRuntimeException;
				return lre != null;
			}).Invoke())
			{
				success = false;
				PublishException(lre);
			}
			catch (Exception ex)
			{
				success = false;
				PushMessage(ex.Message, 2);
			}
			_currentFrameDuration += runClock.Elapsed;
			return (result, success);
		}

		public LuaResult CallFunction(string funcName, params object[] args)
		{
			return WrapScriptCall(() => Global.CallMemberDirect(funcName, args, ignoreCase: true, rawGet: false, throwExceptions: true, ignoreNilFunction: true)).Result;
		}

		public LuaResult CallFunction(string funcName, IEnumerable<object> args)
		{
			return CallFunction(funcName, args.ToArray());
		}

		public async Task<LuaChunk> LoadScript(string scriptName, IPackResourceManager resourceManager)
		{
			if (!resourceManager.ResourceExists(scriptName))
			{
				if (scriptName != "pack.lua")
				{
					Logger.Warn("Attempted to load script '" + scriptName + "', but it could not be found.");
				}
				return null;
			}
			try
			{
				using StreamReader scriptReader = new StreamReader(await resourceManager.LoadResourceStreamAsync(scriptName));
				string scriptSource = await scriptReader.ReadToEndAsync();
				LuaChunk chunk = _lua.CompileChunk(scriptSource, scriptName, new LuaCompileOptions
				{
					DebugEngine = _stackTraceDebugger
				}, new KeyValuePair<string, Type>("Pack", typeof(PackContext)));
				ScriptState newScript = new ScriptState(chunk);
				newScript.Run(Global, new PackContext(this, resourceManager));
				Scripts.Add(newScript);
				PushMessage(newScript.Name + ".lua loaded in " + newScript.LoadTime.Humanize(2) + ".", (newScript.LoadTime.Milliseconds > 500) ? 1 : 0, null, "system");
				return chunk;
			}
			catch (LuaException ex2)
			{
				PublishException(ex2);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load script '" + scriptName + "'.");
			}
			PushMessage("Failed to load " + scriptName + ".", 2, null, "system");
			return null;
		}

		internal LuaResult EvalScript(string script)
		{
			try
			{
				LuaChunk chunk = _lua.CompileChunk(script, "eval", new LuaCompileOptions
				{
					DebugEngine = _stackTraceDebugger
				});
				(LuaResult, bool) scriptResult = WrapScriptCall(() => chunk.Run(Global));
				if (scriptResult.Item2)
				{
					return scriptResult.Item1;
				}
			}
			catch (LuaException ex2)
			{
				PublishException(ex2);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to eval script.");
			}
			return null;
		}

		public void Update(GameTime gameTime)
		{
			_frameExecutionTime.PushValue(_currentFrameDuration);
			_currentFrameDuration = TimeSpan.Zero;
			Global?.Update(gameTime);
		}

		public void Reset()
		{
			Scripts.Clear();
			BuildEnv();
		}

		public void Unload()
		{
			Scripts.Clear();
			_lua?.Dispose();
			StandardMarkerScriptExtensions.SetPackInitiator(null);
			PathingCategoryScriptExtensions.SetPackInitiator(null);
		}
	}
}
