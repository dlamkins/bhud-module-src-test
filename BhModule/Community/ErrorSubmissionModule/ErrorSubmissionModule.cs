using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Flurl.Http;
using NLog;
using NLog.Config;
using NLog.Layouts;
using SemVer;
using Sentry;
using Sentry.NLog;

namespace BhModule.Community.ErrorSubmissionModule
{
	[Export(typeof(Module))]
	public class ErrorSubmissionModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<ErrorSubmissionModule>();

		private const string SENTRY_DSN = "https://a3aeb0597daa404199a7dedba9e6fe87@sentry.blishhud.com:2083/2";

		private const string ETMCONFIG_URL = "https://etm.blishhud.com/config.json";

		private SettingEntry<string> _userDiscordId;

		private SettingEntry<bool> _autoSubmit;

		private ContextHandle<EtmContext> _etmContextHandle;

		private EtmConfig _config;

		private const int MAXREPORTS = 10;

		private int _reports;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public ErrorSubmissionModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override async Task LoadAsync()
		{
			try
			{
				_config = await GeneratedExtensions.GetJsonAsync<EtmConfig>("https://etm.blishhud.com/config.json", default(CancellationToken), (HttpCompletionOption)0);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to download ETM config from {etmConfigUrl}. Using defaults.", new object[1] { "https://etm.blishhud.com/config.json" });
				_config = new EtmConfig();
			}
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_userDiscordId = settings.DefineSetting<string>("_userDiscordId", "", "Discord Username", "Your full Discord username (yourname#1234) so that we can let you know when the issue is resolved.", (SettingTypeRendererDelegate)null);
			_userDiscordId.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateUser);
			UpdateUser(_userDiscordId, new ValueChangedEventArgs<string>(_userDiscordId.get_Value(), _userDiscordId.get_Value()));
		}

		private void UpdateUser(object sender, ValueChangedEventArgs<string> e)
		{
			SentrySdk.ConfigureScope(delegate(Scope scope)
			{
				scope.User = (string.IsNullOrWhiteSpace(e.get_NewValue()) ? null : new User
				{
					Username = e.get_NewValue()
				});
			});
		}

		private void HookLogger()
		{
			object value = typeof(DebugService).GetField("_logConfiguration", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			((LoggingConfiguration)((value is LoggingConfiguration) ? value : null)).AddSentry(new Action<SentryNLogOptions>(ConfigureSentry));
			LogManager.ReconfigExistingLoggers();
		}

		private void ConfigureSentry(SentryNLogOptions sentry)
		{
			sentry.Dsn = "https://a3aeb0597daa404199a7dedba9e6fe87@sentry.blishhud.com:2083/2";
			sentry.Environment = (string.IsNullOrEmpty(Program.get_OverlayVersion().PreRelease) ? "Release" : Program.get_OverlayVersion().PreRelease);
			sentry.Debug = true;
			sentry.BreadcrumbLayout = Layout.op_Implicit("${logger}: ${message}");
			sentry.MaxBreadcrumbs = 10;
			sentry.TracesSampleRate = 0.2;
			sentry.AutoSessionTracking = true;
			sentry.DetectStartupTime = StartupTimeDetectionMode.None;
			sentry.ReportAssembliesMode = ReportAssembliesMode.None;
			sentry.DisableTaskUnobservedTaskExceptionCapture();
			sentry.DisableNetFxInstallationsIntegration();
			sentry.MinimumBreadcrumbLevel = LogLevel.Debug;
			sentry.BeforeSend = delegate(SentryEvent d)
			{
				ErrorSubmissionModule errorSubmissionModule = this;
				int reports = _reports;
				errorSubmissionModule._reports = reports + 1;
				if (reports > 10)
				{
					return null;
				}
				sentry.Dsn = DsnAssocUtil.GetDsnFromEvent(d, _config);
				d.SetExtra("launch-options", Environment.GetCommandLineArgs().Select(FilterUtil.FilterAll).ToArray());
				try
				{
					if (GameService.Module == null)
					{
						return d;
					}
					if (!((GameService)GameService.Module).get_Loaded())
					{
						return d;
					}
					var source = from m in GameService.Module.get_Modules()
						select new
						{
							Name = m.get_Manifest().get_Name(),
							Namespace = m.get_Manifest().get_Namespace(),
							Version = m.get_Manifest().get_Version().ToString(),
							Enabled = m.get_Enabled()
						};
					d.SetExtra("Modules", source.ToArray());
					return d;
				}
				catch (Exception ex)
				{
					d.SetExtra("Modules", "Exception: " + ex.Message);
					return d;
				}
			};
			Logger.Info("Sentry hook enabled.");
		}

		private void EnableSdkContext()
		{
			_etmContextHandle = GameService.Contexts.RegisterContext<EtmContext>(new EtmContext());
			Logger.Info("etm.sdk context registered.");
		}

		protected override void Initialize()
		{
			if (Program.get_OverlayVersion().BaseVersion() != new SemVer.Version(0, 0, 0))
			{
				HookLogger();
				EnableSdkContext();
			}
		}

		protected override void Unload()
		{
			_etmContextHandle?.Expire();
		}
	}
}
