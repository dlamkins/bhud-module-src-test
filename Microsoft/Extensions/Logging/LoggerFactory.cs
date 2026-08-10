using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging
{
	[DebuggerDisplay("{DebuggerToString(),nq}")]
	[DebuggerTypeProxy(typeof(LoggerFactoryDebugView))]
	internal class LoggerFactory : ILoggerFactory, IDisposable
	{
		private struct ProviderRegistration
		{
			public ILoggerProvider Provider;

			public bool ShouldDispose;
		}

		private sealed class DisposingLoggerFactory : ILoggerFactory, IDisposable
		{
			private readonly ILoggerFactory _loggerFactory;

			private readonly ServiceProvider _serviceProvider;

			public DisposingLoggerFactory(ILoggerFactory loggerFactory, ServiceProvider serviceProvider)
			{
				_loggerFactory = loggerFactory;
				_serviceProvider = serviceProvider;
			}

			public void Dispose()
			{
				_serviceProvider.Dispose();
			}

			public ILogger CreateLogger(string categoryName)
			{
				return _loggerFactory.CreateLogger(categoryName);
			}

			public void AddProvider(ILoggerProvider provider)
			{
				_loggerFactory.AddProvider(provider);
			}
		}

		private sealed class LoggerFactoryDebugView
		{
			[CompilerGenerated]
			private LoggerFactory _003CloggerFactory_003EP;

			public List<ILoggerProvider> Providers => _003CloggerFactory_003EP._providerRegistrations.Select((ProviderRegistration r) => r.Provider).ToList();

			public bool Disposed => _003CloggerFactory_003EP._disposed;

			public LoggerFilterOptions FilterOptions => _003CloggerFactory_003EP._filterOptions;

			public LoggerFactoryDebugView(LoggerFactory loggerFactory)
			{
				_003CloggerFactory_003EP = loggerFactory;
				base._002Ector();
			}
		}

		private readonly ConcurrentDictionary<string, Logger> _loggers = new ConcurrentDictionary<string, Logger>(StringComparer.Ordinal);

		private readonly List<ProviderRegistration> _providerRegistrations = new List<ProviderRegistration>();

		private readonly object _sync = new object();

		private volatile bool _disposed;

		private readonly IDisposable _changeTokenRegistration;

		private LoggerFilterOptions _filterOptions;

		private IExternalScopeProvider _scopeProvider;

		private readonly LoggerFactoryOptions _factoryOptions;

		public LoggerFactory()
			: this(Array.Empty<ILoggerProvider>())
		{
		}

		public LoggerFactory(IEnumerable<ILoggerProvider> providers)
			: this(providers, new StaticFilterOptionsMonitor(new LoggerFilterOptions()))
		{
		}

		public LoggerFactory(IEnumerable<ILoggerProvider> providers, LoggerFilterOptions filterOptions)
			: this(providers, new StaticFilterOptionsMonitor(filterOptions))
		{
		}

		public LoggerFactory(IEnumerable<ILoggerProvider> providers, IOptionsMonitor<LoggerFilterOptions> filterOption)
			: this(providers, filterOption, null)
		{
		}

		public LoggerFactory(IEnumerable<ILoggerProvider> providers, IOptionsMonitor<LoggerFilterOptions> filterOption, IOptions<LoggerFactoryOptions>? options)
			: this(providers, filterOption, options, null)
		{
		}

		public LoggerFactory(IEnumerable<ILoggerProvider> providers, IOptionsMonitor<LoggerFilterOptions> filterOption, IOptions<LoggerFactoryOptions>? options = null, IExternalScopeProvider? scopeProvider = null)
		{
			_scopeProvider = scopeProvider;
			_factoryOptions = ((options == null || options!.Value == null) ? new LoggerFactoryOptions() : options!.Value);
			if (((uint)_factoryOptions.ActivityTrackingOptions & 0xFFFFFF80u) != 0)
			{
				throw new ArgumentException(_003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ESR.Format(_003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003ESR.InvalidActivityTrackingOptions, _factoryOptions.ActivityTrackingOptions), "options");
			}
			foreach (ILoggerProvider provider in providers)
			{
				AddProviderRegistration(provider, dispose: false);
			}
			_changeTokenRegistration = filterOption.OnChange(new Action<LoggerFilterOptions>(RefreshFilters));
			RefreshFilters(filterOption.CurrentValue);
		}

		public static ILoggerFactory Create(Action<ILoggingBuilder> configure)
		{
			ServiceCollection services = new ServiceCollection();
			services.AddLogging(configure);
			ServiceProvider serviceProvider = services.BuildServiceProvider();
			ILoggerFactory requiredService = serviceProvider.GetRequiredService<ILoggerFactory>();
			return new DisposingLoggerFactory(requiredService, serviceProvider);
		}

		[_003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EMemberNotNull("_filterOptions")]
		private void RefreshFilters(LoggerFilterOptions filterOptions)
		{
			lock (_sync)
			{
				_filterOptions = filterOptions;
				foreach (KeyValuePair<string, Logger> logger2 in _loggers)
				{
					Logger value = logger2.Value;
					(value.MessageLoggers, value.ScopeLoggers) = ApplyFilters(value.Loggers);
				}
			}
		}

		public ILogger CreateLogger(string categoryName)
		{
			if (CheckDisposed())
			{
				throw new ObjectDisposedException("LoggerFactory");
			}
			if (!_loggers.TryGetValue(categoryName, out var value))
			{
				lock (_sync)
				{
					if (_loggers.TryGetValue(categoryName, out value))
					{
						return value;
					}
					value = new Logger(categoryName, CreateLoggers(categoryName));
					(value.MessageLoggers, value.ScopeLoggers) = ApplyFilters(value.Loggers);
					_loggers[categoryName] = value;
					return value;
				}
			}
			return value;
		}

		public void AddProvider(ILoggerProvider provider)
		{
			if (CheckDisposed())
			{
				throw new ObjectDisposedException("LoggerFactory");
			}
			_003C3582827b_002D431d_002D46d5_002D929b_002D8e97229efdb0_003EThrowHelper.ThrowIfNull(provider, "provider");
			lock (_sync)
			{
				AddProviderRegistration(provider, dispose: true);
				foreach (KeyValuePair<string, Logger> logger2 in _loggers)
				{
					Logger value = logger2.Value;
					LoggerInformation[] array = value.Loggers;
					int num = array.Length;
					Array.Resize(ref array, array.Length + 1);
					array[num] = new LoggerInformation(provider, logger2.Key);
					value.Loggers = array;
					(value.MessageLoggers, value.ScopeLoggers) = ApplyFilters(value.Loggers);
				}
			}
		}

		private void AddProviderRegistration(ILoggerProvider provider, bool dispose)
		{
			_providerRegistrations.Add(new ProviderRegistration
			{
				Provider = provider,
				ShouldDispose = dispose
			});
			ISupportExternalScope supportExternalScope = provider as ISupportExternalScope;
			if (supportExternalScope != null)
			{
				if (_scopeProvider == null)
				{
					_scopeProvider = new LoggerFactoryScopeProvider(_factoryOptions.ActivityTrackingOptions);
				}
				supportExternalScope.SetScopeProvider(_scopeProvider);
			}
		}

		private LoggerInformation[] CreateLoggers(string categoryName)
		{
			LoggerInformation[] array = new LoggerInformation[_providerRegistrations.Count];
			for (int i = 0; i < _providerRegistrations.Count; i++)
			{
				array[i] = new LoggerInformation(_providerRegistrations[i].Provider, categoryName);
			}
			return array;
		}

		private (MessageLogger[] MessageLoggers, ScopeLogger[] ScopeLoggers) ApplyFilters(LoggerInformation[] loggers)
		{
			List<MessageLogger> list = new List<MessageLogger>();
			List<ScopeLogger> list2 = (_filterOptions.CaptureScopes ? new List<ScopeLogger>() : null);
			for (int i = 0; i < loggers.Length; i++)
			{
				LoggerInformation loggerInformation = loggers[i];
				LoggerRuleSelector.Select(_filterOptions, loggerInformation.ProviderType, loggerInformation.Category, out var minLevel, out var filter);
				if (!minLevel.HasValue || minLevel.GetValueOrDefault() <= LogLevel.Critical)
				{
					list.Add(new MessageLogger(loggerInformation.Logger, loggerInformation.Category, loggerInformation.ProviderType.FullName, minLevel, filter));
					if (!loggerInformation.ExternalScope)
					{
						list2?.Add(new ScopeLogger(loggerInformation.Logger, null));
					}
				}
			}
			if (_scopeProvider != null)
			{
				list2?.Add(new ScopeLogger(null, _scopeProvider));
			}
			return (list.ToArray(), list2?.ToArray());
		}

		protected virtual bool CheckDisposed()
		{
			return _disposed;
		}

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
			_changeTokenRegistration?.Dispose();
			foreach (ProviderRegistration providerRegistration in _providerRegistrations)
			{
				try
				{
					if (providerRegistration.ShouldDispose)
					{
						providerRegistration.Provider.Dispose();
					}
				}
				catch
				{
				}
			}
		}

		private string DebuggerToString()
		{
			return $"Providers = {_providerRegistrations.Count}, {_filterOptions.DebuggerToString()}";
		}
	}
}
