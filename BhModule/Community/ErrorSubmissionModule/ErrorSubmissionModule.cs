using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using NLog;
using NLog.Config;
using NLog.Layouts;
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
			sentry.MaxBreadcrumbs = 20;
			sentry.TracesSampleRate = 0.2;
			sentry.AutoSessionTracking = true;
			sentry.BeforeSend = delegate(SentryEvent d)
			{
				d.SetExtra("launch-options", Environment.GetCommandLineArgs().ToArray());
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
		}

		protected override void Initialize()
		{
			HookLogger();
		}

		protected override async Task LoadAsync()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
		}
	}
}
