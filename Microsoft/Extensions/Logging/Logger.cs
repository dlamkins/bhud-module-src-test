using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.Logging
{
	[DebuggerDisplay("{DebuggerToString(),nq}")]
	[DebuggerTypeProxy(typeof(LoggerDebugView))]
	internal sealed class Logger : ILogger
	{
		private sealed class LoggerDebugView
		{
			[CompilerGenerated]
			private Logger _003Clogger_003EP;

			public string Name => _003Clogger_003EP._categoryName;

			public List<LoggerProviderDebugView> Providers
			{
				get
				{
					List<LoggerProviderDebugView> list = new List<LoggerProviderDebugView>();
					for (int i = 0; i < _003Clogger_003EP.Loggers.Length; i++)
					{
						LoggerInformation loggerInfo = _003Clogger_003EP.Loggers[i];
						string providerName = ProviderAliasUtilities.GetAlias(loggerInfo.ProviderType) ?? loggerInfo.ProviderType.Name;
						MessageLogger? messageLogger2 = _003Clogger_003EP.MessageLoggers?.FirstOrDefault((MessageLogger messageLogger) => messageLogger.Logger == loggerInfo.Logger);
						list.Add(new LoggerProviderDebugView(providerName, messageLogger2));
					}
					return list;
				}
			}

			public List<object> Scopes
			{
				get
				{
					IExternalScopeProvider externalScopeProvider = _003Clogger_003EP.ScopeLoggers?.FirstOrDefault().ExternalScopeProvider;
					if (externalScopeProvider == null)
					{
						return null;
					}
					List<object> list = new List<object>();
					externalScopeProvider.ForEachScope(delegate(object scope, List<object> scopes)
					{
						scopes.Add(scope);
					}, list);
					return list;
				}
			}

			public LogLevel? MinLevel => _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EDebuggerDisplayFormatting.CalculateEnabledLogLevel(_003Clogger_003EP);

			public bool Enabled => _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EDebuggerDisplayFormatting.CalculateEnabledLogLevel(_003Clogger_003EP).HasValue;

			public LoggerDebugView(Logger logger)
			{
				_003Clogger_003EP = logger;
				base._002Ector();
			}
		}

		[DebuggerDisplay("{DebuggerToString(),nq}")]
		private sealed class LoggerProviderDebugView
		{
			[CompilerGenerated]
			private string _003CproviderName_003EP;

			[CompilerGenerated]
			private MessageLogger? _003CmessageLogger_003EP;

			public string Name => _003CproviderName_003EP;

			public LogLevel LogLevel => CalculateEnabledLogLevel(_003CmessageLogger_003EP) ?? LogLevel.None;

			public LoggerProviderDebugView(string providerName, MessageLogger? messageLogger)
			{
				_003CproviderName_003EP = providerName;
				_003CmessageLogger_003EP = messageLogger;
				base._002Ector();
			}

			private static LogLevel? CalculateEnabledLogLevel(MessageLogger? logger)
			{
				if (!logger.HasValue)
				{
					return null;
				}
				Span<LogLevel> span = stackalloc LogLevel[6]
				{
					LogLevel.Critical,
					LogLevel.Error,
					LogLevel.Warning,
					LogLevel.Information,
					LogLevel.Debug,
					LogLevel.Trace
				};
				ReadOnlySpan<LogLevel> readOnlySpan = span;
				LogLevel? result = null;
				ReadOnlySpan<LogLevel> readOnlySpan2 = readOnlySpan;
				for (int i = 0; i < readOnlySpan2.Length; i++)
				{
					LogLevel logLevel = readOnlySpan2[i];
					if (!logger.Value.IsEnabled(logLevel))
					{
						break;
					}
					result = logLevel;
				}
				return result;
			}

			private string DebuggerToString()
			{
				return $"Name = \"{_003CproviderName_003EP}\", LogLevel = {LogLevel}";
			}
		}

		private sealed class Scope : IDisposable
		{
			private bool _isDisposed;

			private IDisposable _disposable0;

			private IDisposable _disposable1;

			private readonly IDisposable[] _disposable;

			public Scope(int count)
			{
				if (count > 2)
				{
					_disposable = new IDisposable[count - 2];
				}
			}

			public void SetDisposable(int index, IDisposable disposable)
			{
				switch (index)
				{
				case 0:
					_disposable0 = disposable;
					break;
				case 1:
					_disposable1 = disposable;
					break;
				default:
					_disposable[index - 2] = disposable;
					break;
				}
			}

			public void Dispose()
			{
				if (_isDisposed)
				{
					return;
				}
				_disposable0?.Dispose();
				_disposable1?.Dispose();
				if (_disposable != null)
				{
					int num = _disposable.Length;
					for (int i = 0; i != num; i++)
					{
						_disposable[i]?.Dispose();
					}
				}
				_isDisposed = true;
			}
		}

		private readonly string _categoryName;

		public LoggerInformation[] Loggers { get; set; }

		public MessageLogger[] MessageLoggers { get; set; }

		public ScopeLogger[] ScopeLoggers { get; set; }

		public Logger(string categoryName, LoggerInformation[] loggers)
		{
			_categoryName = categoryName;
			Loggers = loggers;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			MessageLogger[] messageLoggers = MessageLoggers;
			if (messageLoggers == null)
			{
				return;
			}
			List<Exception> exceptions2 = null;
			for (int i = 0; i < messageLoggers.Length; i++)
			{
				ref MessageLogger reference = ref messageLoggers[i];
				if (reference.IsEnabled(logLevel))
				{
					LoggerLog(logLevel, eventId, reference.Logger, exception, formatter, ref exceptions2, in state);
				}
			}
			if (exceptions2 != null && exceptions2.Count > 0)
			{
				ThrowLoggingError(exceptions2);
			}
			static void LoggerLog(LogLevel logLevel, EventId eventId, ILogger logger, Exception exception, Func<TState, Exception, string> formatter, ref List<Exception> exceptions, in TState state)
			{
				try
				{
					logger.Log(logLevel, eventId, state, exception, formatter);
				}
				catch (Exception item)
				{
					if (exceptions == null)
					{
						exceptions = new List<Exception>();
					}
					exceptions.Add(item);
				}
			}
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			MessageLogger[] messageLoggers = MessageLoggers;
			if (messageLoggers == null)
			{
				return false;
			}
			List<Exception> exceptions2 = null;
			int i;
			for (i = 0; i < messageLoggers.Length; i++)
			{
				ref MessageLogger reference = ref messageLoggers[i];
				if (reference.IsEnabled(logLevel) && LoggerIsEnabled(logLevel, reference.Logger, ref exceptions2))
				{
					break;
				}
			}
			if (exceptions2 != null && exceptions2.Count > 0)
			{
				ThrowLoggingError(exceptions2);
			}
			return i < messageLoggers.Length;
			static bool LoggerIsEnabled(LogLevel logLevel, ILogger logger, ref List<Exception> exceptions)
			{
				try
				{
					if (logger.IsEnabled(logLevel))
					{
						return true;
					}
				}
				catch (Exception item)
				{
					if (exceptions == null)
					{
						exceptions = new List<Exception>();
					}
					exceptions.Add(item);
				}
				return false;
			}
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			ScopeLogger[] scopeLoggers = ScopeLoggers;
			if (scopeLoggers == null)
			{
				return _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ENullScope.Instance;
			}
			if (scopeLoggers.Length == 1)
			{
				return scopeLoggers[0].CreateScope(state);
			}
			Scope scope = new Scope(scopeLoggers.Length);
			List<Exception> list = null;
			for (int i = 0; i < scopeLoggers.Length; i++)
			{
				ref ScopeLogger reference = ref scopeLoggers[i];
				try
				{
					scope.SetDisposable(i, reference.CreateScope(state));
				}
				catch (Exception item)
				{
					if (list == null)
					{
						list = new List<Exception>();
					}
					list.Add(item);
				}
			}
			if (list != null && list.Count > 0)
			{
				ThrowLoggingError(list);
			}
			return scope;
		}

		private static void ThrowLoggingError(List<Exception> exceptions)
		{
			throw new AggregateException("An error occurred while writing to logger(s).", exceptions);
		}

		internal string DebuggerToString()
		{
			return _003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EDebuggerDisplayFormatting.DebuggerToString(_categoryName, this);
		}
	}
	[DebuggerDisplay("{DebuggerToString(),nq}")]
	internal class Logger<T> : ILogger<T>, ILogger
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private readonly ILogger _logger;

		public Logger(ILoggerFactory factory)
		{
			_003Cd25529be_002D23db_002D4821_002Dbf3b_002D764b02b1b0ed_003EThrowHelper.ThrowIfNull(factory, "factory");
			_logger = factory.CreateLogger(GetCategoryName());
		}

		IDisposable ILogger.BeginScope<TState>(TState state)
		{
			return _logger.BeginScope(state);
		}

		bool ILogger.IsEnabled(LogLevel logLevel)
		{
			return _logger.IsEnabled(logLevel);
		}

		void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			_logger.Log(logLevel, eventId, state, exception, formatter);
		}

		private static string GetCategoryName()
		{
			return TypeNameHelper.GetTypeDisplayName(typeof(T), fullName: true, includeGenericParameterNames: false, includeGenericParameters: false, '.');
		}

		internal string DebuggerToString()
		{
			return DebuggerDisplayFormatting.DebuggerToString(GetCategoryName(), this);
		}
	}
}
