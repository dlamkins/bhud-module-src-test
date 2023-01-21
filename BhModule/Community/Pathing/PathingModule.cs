using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.LocalHttp;
using BhModule.Community.Pathing.MarkerPackRepo;
using BhModule.Community.Pathing.Scripting;
using BhModule.Community.Pathing.Scripting.Console;
using BhModule.Community.Pathing.UI.Views;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Entities;
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

		private CornerIcon _pathingIcon;

		private TabbedWindow2 _settingsWindow;

		private bool _packsLoading;

		private Tab _packSettingsTab;

		private Tab _mapSettingsTab;

		private Tab _keybindSettingsTab;

		private Tab _scriptSettingsTab;

		private Tab _markerRepoTab;

		private HttpHost _apiHost;

		private ConsoleWindow _scriptConsoleWindow;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal static PathingModule Instance { get; private set; }

		public ScriptEngine ScriptEngine { get; private set; }

		public ModuleSettings Settings { get; private set; }

		public PackInitiator PackInitiator { get; private set; }

		public BhModule.Community.Pathing.MarkerPackRepo.MarkerPackRepo MarkerPackRepo { get; private set; }

		[ImportingConstructor]
		public PathingModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = new ModuleSettings(settings);
		}

		private IEnumerable<ContextMenuStripItem> GetPathingMenuItems()
		{
			if (PackInitiator != null)
			{
				foreach (ContextMenuStripItem menuItem in PackInitiator.GetPackMenuItems())
				{
					((Control)menuItem).set_Enabled(((Control)menuItem).get_Enabled() && !_packsLoading);
					yield return menuItem;
				}
			}
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text("Download Marker Packs");
			ContextMenuStripItem downloadMarkers = val;
			((Control)downloadMarkers).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_settingsWindow.set_SelectedTab(_markerRepoTab);
				((Control)_settingsWindow).Show();
			});
			yield return downloadMarkers;
			if (Settings.ScriptsConsoleEnabled.get_Value() || ((Enum)GameService.Input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)4))
			{
				ContextMenuStripItem val2 = new ContextMenuStripItem();
				val2.set_Text("Script Console");
				ContextMenuStripItem scriptConsole = val2;
				((Control)scriptConsole).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					ShowScriptWindow();
				});
				yield return scriptConsole;
			}
			ContextMenuStripItem val3 = new ContextMenuStripItem();
			val3.set_Text("Pathing Module Settings");
			ContextMenuStripItem openSettings = val3;
			((Control)openSettings).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_settingsWindow.get_SelectedTab() == _markerRepoTab)
				{
					_settingsWindow.set_SelectedTab(_packSettingsTab);
					if (((Control)_settingsWindow).get_Visible())
					{
						return;
					}
				}
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
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Expected O, but got Unknown
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Expected O, but got Unknown
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Expected O, but got Unknown
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Expected O, but got Unknown
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
			((WindowBase2)val2).set_Id(((Module)this).get_Namespace() + "_SettingsWindow");
			((WindowBase2)val2).set_SavesPosition(true);
			_settingsWindow = val2;
			_packSettingsTab = new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\156740+155150.png")), (Func<IView>)(() => (IView)new SettingsView(Settings.PackSettings, -1)), Strings.Window_MainSettingsTab, (int?)null);
			_mapSettingsTab = new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\157123+155150.png")), (Func<IView>)(() => (IView)new SettingsView(Settings.MapSettings, -1)), Strings.Window_MapSettingsTab, (int?)null);
			_scriptSettingsTab = new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\156701.png")), (Func<IView>)(() => (IView)new SettingsView(Settings.ScriptSettings, -1)), "Script Options", (int?)null);
			_keybindSettingsTab = new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\156734+155150.png")), (Func<IView>)(() => (IView)new SettingsView(Settings.KeyBindSettings, -1)), Strings.Window_KeyBindSettingsTab, (int?)null);
			_markerRepoTab = new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\156909.png")), (Func<IView>)(() => (IView)(object)new PackRepoView(this)), Strings.Window_DownloadMarkerPacks, (int?)null);
			_settingsWindow.get_Tabs().Add(_packSettingsTab);
			_settingsWindow.get_Tabs().Add(_mapSettingsTab);
			_settingsWindow.get_Tabs().Add(_scriptSettingsTab);
			_settingsWindow.get_Tabs().Add(_keybindSettingsTab);
			_settingsWindow.get_Tabs().Add(_markerRepoTab);
			((Control)_pathingIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				if (((Enum)GameService.Input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Settings.GlobalPathablesEnabled.set_Value(!Settings.GlobalPathablesEnabled.get_Value());
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

		private void ShowScriptWindow()
		{
			if (_scriptConsoleWindow == null)
			{
				_scriptConsoleWindow = new ConsoleWindow(this);
			}
			_scriptConsoleWindow.Show();
			_scriptConsoleWindow.BringToFront();
			_scriptConsoleWindow.FormClosed += delegate
			{
				_scriptConsoleWindow = null;
			};
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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)((Module)this).get_RunState() == 2)
			{
				_pathingIcon.set_LoadingMessage(loadingMessage);
				_packsLoading = !string.IsNullOrWhiteSpace(loadingMessage);
			}
			if (!_packsLoading)
			{
				((Control)_pathingIcon).set_BasicTooltipText(Strings.General_UiName);
			}
		}

		public IProgress<string> GetModuleProgressHandler()
		{
			return new Progress<string>(UpdateModuleLoading);
		}

		protected override async Task LoadAsync()
		{
			Stopwatch sw = Stopwatch.StartNew();
			ScriptEngine = new ScriptEngine(this);
			MarkerPackRepo = new BhModule.Community.Pathing.MarkerPackRepo.MarkerPackRepo(this);
			MarkerPackRepo.Init();
			PackInitiator = new PackInitiator(DirectoriesManager.GetFullDirectoryPath("markers"), this, GetModuleProgressHandler());
			await PackInitiator.Init();
			sw.Stop();
			Logger.Debug($"Took {sw.ElapsedMilliseconds} ms to complete loading Pathing module...");
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsHintView(((Action)((Control)_settingsWindow).Show, PackInitiator));
		}

		protected override void Update(GameTime gameTime)
		{
			ScriptEngine?.Update(gameTime);
			PackInitiator?.Update(gameTime);
		}

		private void UnloadPathingElements()
		{
			IEntity[] array = GameService.Graphics.get_World().get_Entities() as IEntity[];
			foreach (IEntity entity in array)
			{
				if (entity is IPathingEntity)
				{
					GameService.Graphics.get_World().RemoveEntity(entity);
				}
			}
		}

		protected override void Unload()
		{
			ScriptEngine?.Unload();
			_apiHost?.Close();
			PackInitiator?.Unload();
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
			_scriptConsoleWindow?.Dispose();
			UnloadPathingElements();
			Instance = null;
		}
	}
}
