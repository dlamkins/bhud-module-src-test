using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Modules;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.Utils;
using Estreya.BlishHUD.StartupNotifications.UI.Views;

namespace Estreya.BlishHUD.StartupNotifications
{
	[Export(typeof(Module))]
	public class StartupNotificationsModule : BaseModule<StartupNotificationsModule, ModuleSettings>
	{
		protected override string UrlModuleName => "startup-notifications";

		protected override string API_VERSION_NO => "1";

		protected override bool NeedsBackend => false;

		protected override int CornerIconPriority => 1289351265;

		[ImportingConstructor]
		public StartupNotificationsModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override async Task LoadAsync()
		{
			await base.LoadAsync();
			await LoadAndShowNotifications();
		}

		private async Task LoadAndShowNotifications()
		{
			string directoryPath = base.DirectoriesManager.GetFullDirectoryPath(GetDirectoryName());
			List<string> files = Directory.GetFiles(directoryPath, "*.txt", SearchOption.TopDirectoryOnly).ToList();
			if (files.Count == 0)
			{
				List<string> list = files;
				list.Add(await CreateDummyFile(directoryPath));
			}
			ManualResetEvent resetEvent = new ManualResetEvent(initialState: true);
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			foreach (string file in files)
			{
				resetEvent.Reset();
				string content = await FileUtil.ReadStringAsync(file);
				if (string.IsNullOrWhiteSpace(content))
				{
					base.Logger.Warn("Content of file \"" + file + "\" is empty.");
					continue;
				}
				ScreenNotification notification = ScreenNotification.ShowNotification(content, base.ModuleSettings.Type.get_Value(), null, base.ModuleSettings.Duration.get_Value());
				if (base.ModuleSettings.AwaitEach.get_Value())
				{
					EventHandler<EventArgs> notificationDisposedHandler = delegate
					{
						resetEvent.Set();
					};
					((Control)notification).add_Disposed(notificationDisposedHandler);
					await resetEvent.WaitOneAsync(TimeSpan.FromSeconds(60.0), cancellationTokenSource.Token);
					((Control)notification).remove_Disposed(notificationDisposedHandler);
				}
			}
		}

		private async Task<string> CreateDummyFile(string directoryPath)
		{
			string filePath = Path.Combine(directoryPath, "01_example.txt");
			await FileUtil.WriteStringAsync(filePath, "[Startup Notifications] Change me");
			return filePath;
		}

		protected override void Unload()
		{
			base.Unload();
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView(base.Gw2ApiManager, base.IconService, base.TranslationService, base.SettingEventService, base.ModuleSettings);
		}

		protected override BaseModuleSettings DefineModuleSettings(SettingCollection settings)
		{
			return new ModuleSettings(settings);
		}

		protected override string GetDirectoryName()
		{
			return "startup";
		}

		protected override AsyncTexture2D GetEmblem()
		{
			return null;
		}

		protected override AsyncTexture2D GetCornerIcon()
		{
			return null;
		}
	}
}
