using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Estreya.BlishHUD.Shared.Controls.Input;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Microsoft.Xna.Framework;
using SemVer;

namespace Estreya.BlishHUD.Shared.Services
{
	public class MetricsService : ManagedService
	{
		private readonly IFlurlClient _flurlClient;

		private readonly string _apiBaseUrl;

		private readonly string _moduleName;

		private readonly string _moduleNamespace;

		private readonly Version _moduleVersion;

		private readonly BaseModuleSettings _moduleSettings;

		private readonly IconService _iconService;

		private ConcurrentQueue<string> _metricsQueue;

		private static TimeSpan _metricsQueueInterval = TimeSpan.FromSeconds(10.0);

		private AsyncRef<double> _timeSincelastMetricQueueInterval = new AsyncRef<double>(0.0);

		public bool ConsentGiven
		{
			get
			{
				if (_moduleSettings.AskedMetricsConsent.get_Value())
				{
					return _moduleSettings.SendMetrics.get_Value();
				}
				return false;
			}
		}

		public bool NeedsConsentRenewal
		{
			get
			{
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Expected O, but got Unknown
				if (_moduleSettings.SendMetrics.get_Value() && _moduleSettings.MetricsConsentGivenVersion.get_Value() != null)
				{
					return _moduleSettings.MetricsConsentGivenVersion.get_Value() < new Version("3.6.1", false);
				}
				return false;
			}
		}

		public MetricsService(ServiceConfiguration configuration, IFlurlClient flurlClient, string apiBaseUrl, string moduleName, string moduleNamespace, Version moduleVersion, BaseModuleSettings moduleSettings, IconService iconService)
			: base(configuration)
		{
			_flurlClient = flurlClient;
			_apiBaseUrl = apiBaseUrl;
			_moduleName = moduleName;
			_moduleNamespace = moduleNamespace;
			_moduleVersion = moduleVersion;
			_moduleSettings = moduleSettings;
			_iconService = iconService;
		}

		protected override Task Initialize()
		{
			_metricsQueue = new ConcurrentQueue<string>();
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
			_metricsQueue = null;
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
			UpdateUtil.UpdateAsync(HandleQueue, gameTime, _metricsQueueInterval.TotalMilliseconds, _timeSincelastMetricQueueInterval, doLogging: false);
		}

		protected override Task Load()
		{
			return Task.CompletedTask;
		}

		private async Task HandleQueue()
		{
			int max = 50;
			int handled = 0;
			string metricKey;
			while (_metricsQueue.TryDequeue(out metricKey) && handled <= max)
			{
				await SendMetricAsync(metricKey);
				handled++;
			}
		}

		public void QueueMetric(string key)
		{
			if (ConsentGiven)
			{
				_metricsQueue.Enqueue(key);
			}
		}

		public async Task SendMetricAsync(string key)
		{
			if (ConsentGiven)
			{
				try
				{
					await _flurlClient.Request(_apiBaseUrl, "metrics/modules", _moduleNamespace, key).SendAsync(HttpMethod.get_Post(), null, default(CancellationToken), (HttpCompletionOption)0);
				}
				catch (Exception ex)
				{
					Logger.Debug(ex, "Could not send metric \"" + key + "\".");
				}
			}
		}

		public async Task AskMetricsConsent(bool forceAsk = false)
		{
			bool needNewConsent = NeedsConsentRenewal;
			if (forceAsk || needNewConsent || !_moduleSettings.AskedMetricsConsent.get_Value())
			{
				bool consentGiven = await new ConfirmDialog("Allow Metrics?", "The module \"" + _moduleName + "\" (" + _moduleNamespace + ") would like to collect anonymous metric data.\n\nThe collected data will be used for advanced usage statistics for specific module functions to see how often and in which combination they are used.\n\nAll data is completely anonymous and a reference to your profile/account can't be created.", _iconService, new ButtonDefinition[2]
				{
					new ButtonDefinition("Yes", DialogResult.Yes),
					new ButtonDefinition("No", DialogResult.No)
				})
				{
					SelectedButtonIndex = 1
				}.ShowDialog() == DialogResult.Yes;
				_moduleSettings.AskedMetricsConsent.set_Value(true);
				_moduleSettings.SendMetrics.set_Value(consentGiven);
				_moduleSettings.MetricsConsentGivenVersion.set_Value((Version)(consentGiven ? ((object)_moduleVersion) : ((object)new Version("0.0.0", false))));
			}
		}
	}
}
