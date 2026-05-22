using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TyriaPlanner.Hud.Api;
using TyriaPlanner.Hud.Services;
using TyriaPlanner.Hud.Settings;
using TyriaPlanner.Hud.Ui;

namespace TyriaPlanner.Hud
{
	[Export(typeof(Module))]
	public sealed class TyriaPlannerHudModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger<TyriaPlannerHudModule>();

		private readonly ContentsManager _contents;

		private ModuleSettings _settings;

		private ApiClient _api;

		private ToastStack _stack;

		private NotificationService _notify;

		private NotificationHistory _history;

		private PollingService _poller;

		private StreamSubscriber _stream;

		private WeeklyResetReminder _resetReminder;

		private MenuWindow _menu;

		private CornerIcon _cornerIcon;

		private Texture2D _iconTexture;

		[ImportingConstructor]
		public TyriaPlannerHudModule([Import("ModuleParameters")] ModuleParameters parameters)
			: this(parameters)
		{
			_contents = parameters.get_ContentsManager();
		}

		protected override void DefineSettings(SettingCollection root)
		{
			_settings = new ModuleSettings(root);
		}

		protected override async Task LoadAsync()
		{
			_api = new ApiClient();
			_history = new NotificationHistory();
			_stack = new ToastStack(_settings);
			_notify = new NotificationService(_stack, _settings, _history);
			_poller = new PollingService(_api, _settings, _notify);
			_stream = new StreamSubscriber(_settings, delegate
			{
				_poller?.RefreshNow();
			});
			_resetReminder = new WeeklyResetReminder(_settings, _stack);
			_menu = new MenuWindow(_api, _settings, _notify, _history);
			await Task.CompletedTask;
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			((Module)this).OnModuleLoaded(e);
			try
			{
				_iconTexture = _contents.GetTexture("icon.png");
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load corner icon · falling back to text-only.");
			}
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_iconTexture));
			val.set_IconName("Tyria Planner");
			val.set_Priority(7111111);
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_menu?.Toggle();
			});
			_poller.Start();
			_stream.Start();
			_resetReminder.Start();
			Logger.Info("Tyria Planner loaded.");
		}

		protected override void Unload()
		{
			_poller?.Dispose();
			_stream?.Dispose();
			_resetReminder?.Dispose();
			_stack?.Clear();
			MenuWindow menu = _menu;
			if (menu != null)
			{
				((Control)menu).Dispose();
			}
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			_api?.Dispose();
		}

		protected override void Update(GameTime gameTime)
		{
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new BrandedSettingsView(_settings.Root, _iconTexture);
		}
	}
}
