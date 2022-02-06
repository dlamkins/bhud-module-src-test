using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
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

		private SettingEntry<string> _userDiscordId;

		private SettingEntry<bool> _autoSubmit;

		private ContextHandle<EtmContext> _etmContextHandle;

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
			(typeof(DebugService).GetField("_logConfiguration", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as LoggingConfiguration).AddSentry(new Action<SentryNLogOptions>(ConfigureSentry));
			LogManager.ReconfigExistingLoggers();
		}

		private void ConfigureSentry(SentryNLogOptions sentry)
		{
			sentry.Dsn = "https://a3aeb0597daa404199a7dedba9e6fe87@sentry.blishhud.com:2083/2";
			sentry.Environment = (string.IsNullOrEmpty(Program.get_OverlayVersion().PreRelease) ? "Release" : Program.get_OverlayVersion().PreRelease);
			sentry.Debug = true;
			sentry.BreadcrumbLayout = (Layout)"${logger}: ${message}";
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
				if (_reports++ > 10)
				{
					return null;
				}
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

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			_etmContextHandle.Expire();
		}
	}
}
