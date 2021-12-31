using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using BhModule.Community.Pathing.UI.Views;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SemVer;

namespace BhModule.Community.Pathing
{
	[Export(typeof(Module))]
	public class PathingModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<PathingModule>();

		private ModuleSettings _moduleSettings;

		private CornerIcon _pathingIcon;

		private TabbedWindow2 _settingsWindow;

		private bool _packsLoading;

		private PackInitiator _watcher;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal static PathingModule Instance { get; private set; }

		[ImportingConstructor]
		public PathingModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_moduleSettings = new ModuleSettings(settings);
		}

		private IEnumerable<ContextMenuStripItem> GetPathingMenuItems()
		{
			if (_watcher != null)
			{
				foreach (ContextMenuStripItem menuItem in _watcher.GetPackMenuItems())
				{
					((Control)menuItem).set_Enabled(((Control)menuItem).get_Enabled() && !_packsLoading);
					yield return menuItem;
				}
			}
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text("Pathing Module Settings");
			ContextMenuStripItem openSettings = val;
			((Control)openSettings).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)_settingsWindow).ToggleWindow();
			});
			yield return openSettings;
		}

		protected override void Initialize()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Expected O, but got Unknown
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Expected O, but got Unknown
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Expected O, but got Unknown
			CornerIcon val = new CornerIcon();
			val.set_IconName(Strings.General_UiName);
			val.set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\pathing-icon.png")));
			val.set_Priority(Strings.General_UiName.GetHashCode());
			_pathingIcon = val;
			TabbedWindow2 val2 = new TabbedWindow2(ContentsManager.GetTexture("png\\controls\\156006.png"), new Rectangle(35, 36, 900, 640), new Rectangle(95, 42, 821, 592));
			((WindowBase2)val2).set_Title(Strings.General_UiName);
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val2).set_Location(new Point(100, 100));
			((Control)val2).set_ClipsBounds(Program.get_OverlayVersion() == new Version(0, 11, 2, (string)null, (string)null) && GameService.Graphics.GetDpiScaleRatio() != 1f);
			((WindowBase2)val2).set_Emblem(ContentsManager.GetTexture("png\\controls\\1615829.png"));
			_settingsWindow = val2;
			_settingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\156740+155150.png")), (Func<IView>)(() => (IView)new SettingsView(_moduleSettings.PackSettings, -1)), Strings.Window_MainSettingsTab, (int?)null));
			_settingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\157123+155150.png")), (Func<IView>)(() => (IView)new SettingsView(_moduleSettings.MapSettings, -1)), Strings.Window_MapSettingsTab, (int?)null));
			_settingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\156734+155150.png")), (Func<IView>)(() => (IView)new SettingsView(_moduleSettings.KeyBindSettings, -1)), Strings.Window_KeyBindSettingsTab, (int?)null));
			((Control)_pathingIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				if (((Enum)GameService.Input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					_moduleSettings.GlobalPathablesEnabled.set_Value(!_moduleSettings.GlobalPathablesEnabled.get_Value());
				}
				else
				{
					ShowPathingContextMenu();
				}
			});
			((Control)_pathingIcon).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)_pathingIcon).get_Enabled())
				{
					ShowPathingContextMenu();
				}
			});
		}

		private void ShowPathingContextMenu()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStrip val = new ContextMenuStrip();
			val.AddMenuItems(GetPathingMenuItems());
			val.Show((Control)(object)_pathingIcon);
		}

		private void UpdateModuleLoading(string loadingMessage)
		{
			_pathingIcon.set_LoadingMessage(loadingMessage);
			_packsLoading = !string.IsNullOrWhiteSpace(loadingMessage);
		}

		public IProgress<string> GetModuleProgressHandler()
		{
			return new Progress<string>(UpdateModuleLoading);
		}

		protected override async Task LoadAsync()
		{
			Stopwatch sw = Stopwatch.StartNew();
			_watcher = new PackInitiator(DirectoriesManager.GetFullDirectoryPath("markers"), _moduleSettings, GetModuleProgressHandler());
			await _watcher.Init();
			sw.Stop();
			Logger.Debug($"Took {sw.ElapsedMilliseconds} ms to complete loading Pathing module...");
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsHintView(((Action)((Control)_settingsWindow).Show, _watcher));
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			_watcher?.Update(gameTime);
		}

		protected override void Unload()
		{
			_watcher?.Unload();
			CornerIcon pathingIcon = _pathingIcon;
			if (pathingIcon != null)
			{
				((Control)pathingIcon).Dispose();
			}
			TabbedWindow2 settingsWindow = _settingsWindow;
			if (settingsWindow != null)
			{
				((Control)settingsWindow).Dispose();
			}
			Instance = null;
		}
	}
}