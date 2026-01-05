using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.MarkerPackRepo;
using BhModule.Community.Pathing.Scripting;
using BhModule.Community.Pathing.Scripting.Console;
using BhModule.Community.Pathing.UI.Events;
using BhModule.Community.Pathing.UI.Views;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Entities;
using Blish_HUD.GameIntegration;
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

		private bool _packsLoading;

		private ConsoleWindow _scriptConsoleWindow;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal static PathingModule Instance { get; private set; }

		public ScriptEngine ScriptEngine { get; private set; }

		public ModuleSettings Settings { get; private set; }

		public TabbedWindow2 SettingsWindow { get; private set; }

		public PackInitiator PackInitiator { get; private set; }

		public BhModule.Community.Pathing.MarkerPackRepo.MarkerPackRepo MarkerPackRepo { get; private set; }

		public CategoryTreeView CategoryTreeView { get; private set; }

		public Tab CategoryTreeTab { get; private set; }

		public Tab PackSettingsTab { get; private set; }

		public Tab MapSettingsTab { get; private set; }

		public Tab KeybindSettingsTab { get; private set; }

		public Tab ScriptSettingsTab { get; private set; }

		public Tab MarkerRepoTab { get; private set; }

		[ImportingConstructor]
		public PathingModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = new ModuleSettings(this, settings);
		}

		[IteratorStateMachine(typeof(_003CGetPathingMenuItems_003Ed__66))]
		private IEnumerable<ContextMenuStripItem> GetPathingMenuItems()
		{
			return new _003CGetPathingMenuItems_003Ed__66(-2)
			{
				_003C_003E4__this = this
			};
		}

		protected override void Initialize()
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Expected O, but got Unknown
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Expected O, but got Unknown
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Expected O, but got Unknown
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Expected O, but got Unknown
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Expected O, but got Unknown
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Expected O, but got Unknown
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Expected O, but got Unknown
			if (DateTime.UtcNow.Date >= new DateTime(2023, 8, 22, 0, 0, 0, DateTimeKind.Utc) && Program.get_OverlayVersion() < new SemVer.Version(1, 1, 0) && Program.get_OverlayVersion() > new SemVer.Version(0, 1, 0))
			{
				try
				{
					typeof(TacOIntegration).GetProperty("TacOIsRunning").GetSetMethod(nonPublic: true)?.Invoke(GameService.GameIntegration.get_TacO(), new object[1] { true });
				}
				catch
				{
				}
			}
			CornerIcon val = new CornerIcon();
			val.set_IconName(Strings.General_UiName);
			val.set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\pathing-icon.png")));
			val.set_Priority("Markers & Trails".GetHashCode());
			_pathingIcon = val;
			TabbedWindow2 val2 = new TabbedWindow2(ContentsManager.GetTexture("png\\controls\\156006.png"), new Rectangle(35, 36, 900, 640), new Rectangle(95, 42, 821, 592));
			((WindowBase2)val2).set_Title(Strings.General_UiName);
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val2).set_Location(new Point(100, 100));
			((WindowBase2)val2).set_Emblem(ContentsManager.GetTexture("png\\controls\\1615829.png"));
			((WindowBase2)val2).set_Id(((Module)this).get_Namespace() + "_SettingsWindow");
			((WindowBase2)val2).set_SavesPosition(true);
			SettingsWindow = val2;
			CategoryTreeView = new CategoryTreeView(this);
			CategoryTreeTab = new Tab(AsyncTexture2D.FromAssetId(1654244), (Func<IView>)(() => (IView)(object)CategoryTreeView), "Category Explorer", (int?)null);
			PackSettingsTab = new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\156740+155150.png")), (Func<IView>)(() => (IView)new SettingsView(Settings.PackSettings, -1)), Strings.Window_MainSettingsTab, (int?)null);
			MapSettingsTab = new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\157123+155150.png")), (Func<IView>)(() => (IView)new SettingsView(Settings.MapSettings, -1)), Strings.Window_MapSettingsTab, (int?)null);
			ScriptSettingsTab = new Tab(AsyncTexture2D.FromAssetId(156701), (Func<IView>)(() => (IView)new SettingsView(Settings.ScriptSettings, -1)), "Script Options", (int?)null);
			KeybindSettingsTab = new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("png\\156734+155150.png")), (Func<IView>)(() => (IView)new SettingsView(Settings.KeyBindSettings, -1)), Strings.Window_KeyBindSettingsTab, (int?)null);
			MarkerRepoTab = new Tab(AsyncTexture2D.FromAssetId(156909), (Func<IView>)(() => (IView)(object)new PackRepoView(this)), Strings.Window_DownloadMarkerPacks, (int?)null);
			SettingsWindow.get_Tabs().Add(CategoryTreeTab);
			SettingsWindow.get_Tabs().Add(PackSettingsTab);
			SettingsWindow.get_Tabs().Add(MapSettingsTab);
			SettingsWindow.get_Tabs().Add(ScriptSettingsTab);
			SettingsWindow.get_Tabs().Add(KeybindSettingsTab);
			SettingsWindow.get_Tabs().Add(MarkerRepoTab);
			((Control)_pathingIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				if (((Enum)GameService.Input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Settings.GlobalPathablesEnabled.set_Value(!Settings.GlobalPathablesEnabled.get_Value());
				}
				else if (((Control)_pathingIcon).get_Enabled())
				{
					TogglePathingContextMenu();
				}
			});
			((Control)_pathingIcon).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)_pathingIcon).get_Enabled())
				{
					TogglePathingContextMenu();
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

		private void TogglePathingContextMenu()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStrip val = new ContextMenuStrip();
			val.AddMenuItems(GetPathingMenuItems());
			val.Show((Control)(object)_pathingIcon);
		}

		private void UpdateModuleLoading(string loadingMessage)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between Unknown and I4
			_pathingIcon.set_LoadingMessage(loadingMessage);
			if ((int)((Module)this).get_RunState() == 2 && _pathingIcon != null)
			{
				_pathingIcon.set_LoadingMessage(loadingMessage);
				_packsLoading = !string.IsNullOrWhiteSpace(loadingMessage);
				if (!_packsLoading)
				{
					((Control)_pathingIcon).set_BasicTooltipText(Strings.General_UiName);
				}
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
			PackInitiator.PackState.CategoryStates.TriggerOpenCategoryView += CategoryStatesOnTriggerOpenCategoryView;
			sw.Stop();
			Logger.Debug($"Took {sw.ElapsedMilliseconds} ms to complete loading Pathing module...");
		}

		private void CategoryStatesOnTriggerOpenCategoryView(object sender, PathingCategoryEventArgs e)
		{
			if (SettingsWindow.get_SelectedTab() != CategoryTreeTab)
			{
				CategoryTreeView.TargetCategory = e.Category;
				SettingsWindow.set_SelectedTab(CategoryTreeTab);
			}
			else
			{
				CategoryTreeView.NavigateToCategory(e.Category);
			}
			((Control)SettingsWindow).Show();
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsHintView((delegate
			{
				SettingsWindow.set_SelectedTab(PackSettingsTab);
				((Control)SettingsWindow).Show();
			}, delegate
			{
				SettingsWindow.set_SelectedTab(MarkerRepoTab);
				((Control)SettingsWindow).Show();
			}, PackInitiator));
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
			Settings?.Unload();
			PackInitiator?.Unload();
			CornerIcon pathingIcon = _pathingIcon;
			if (pathingIcon != null)
			{
				((Control)pathingIcon).Dispose();
			}
			TabbedWindow2 settingsWindow = SettingsWindow;
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
