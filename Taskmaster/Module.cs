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
using Taskmaster.Services;
using Taskmaster.Settings;
using Taskmaster.UI;

namespace Taskmaster
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private ModuleSettings _settings;

		private TaskStore _store;

		private TaskmasterWindow _window;

		private CornerIcon _cornerIcon;

		private readonly MapWindowVisibilityController _windowVisibility = new MapWindowVisibilityController();

		private double _minuteAccumulator;

		internal static Module Instance { get; private set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settings = new ModuleSettings(settings);
		}

		protected override Task LoadAsync()
		{
			string dir = DirectoriesManager.GetFullDirectoryPath("taskmaster");
			_store = new TaskStore(dir);
			TaskStoreLoadResult result = _store.Load();
			Logger.Info($"Task store loaded: {result.Outcome}, {_store.Tabs.Count} tab(s)");
			switch (result.Outcome)
			{
			case TaskStoreLoadOutcome.LoadedBackup:
				ScreenNotification.ShowNotification("Taskmaster: tasks.json was corrupt - restored from backup", (NotificationType)1, (Texture2D)null, 4);
				break;
			case TaskStoreLoadOutcome.StartedEmptyAfterCorruption:
				ScreenNotification.ShowNotification("Taskmaster: task file was corrupt and no backup found. Quarantined at " + result.QuarantinedPath, (NotificationType)2, (Texture2D)null, 4);
				break;
			case TaskStoreLoadOutcome.VersionTooNew:
				ScreenNotification.ShowNotification("Taskmaster: your tasks were saved by a newer version - update the module. Running read-only.", (NotificationType)2, (Texture2D)null, 4);
				break;
			}
			return Task.CompletedTask;
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			int startupResets = ResetEngine.ApplyResets(_store.Tabs, DateTime.UtcNow);
			if (startupResets > 0)
			{
				_store.MarkDirty(DateTime.UtcNow);
				Logger.Info($"Applied {startupResets} reset(s) at startup");
			}
			_window = new TaskmasterWindow(_store, _settings);
			((Control)_window).Hide();
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("corner-icon.png")));
			((Control)val).set_BasicTooltipText("Taskmaster");
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ToggleWindow();
			});
			_settings.ToggleWindow.get_Value().set_Enabled(true);
			_settings.ToggleWindow.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				ToggleWindow();
			});
			((Module)this).OnModuleLoaded(e);
		}

		private void ToggleWindow()
		{
			if (_window != null)
			{
				ApplyWindowVisibility(_windowVisibility.Toggle(((Control)_window).get_Visible(), IsWindowHiddenByMap()));
			}
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new TaskmasterSettingsView(_settings);
		}

		protected override void Update(GameTime gameTime)
		{
			if (_store != null)
			{
				_store.FlushIfDue(DateTime.UtcNow);
				_minuteAccumulator += gameTime.get_ElapsedGameTime().TotalSeconds;
				if (_minuteAccumulator >= 60.0)
				{
					_minuteAccumulator = 0.0;
					_window?.OnMinuteTick(DateTime.UtcNow);
				}
				if (_window != null)
				{
					ApplyWindowVisibility(_windowVisibility.Update(((Control)_window).get_Visible(), IsWindowHiddenByMap()));
				}
			}
		}

		private bool IsWindowHiddenByMap()
		{
			if (!_settings.ShowOnMap.get_Value() && GameService.Gw2Mumble.get_IsAvailable())
			{
				return GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			return false;
		}

		private void ApplyWindowVisibility(WindowVisibilityAction action)
		{
			switch (action)
			{
			case WindowVisibilityAction.Show:
				((Control)_window).Show();
				break;
			case WindowVisibilityAction.Hide:
				((Control)_window).Hide();
				break;
			}
		}

		protected override void Unload()
		{
			_settings.ToggleWindow.get_Value().set_Enabled(false);
			_store?.Save();
			TaskmasterWindow window = _window;
			if (window != null)
			{
				((Control)window).Dispose();
			}
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			Instance = null;
		}
	}
}
