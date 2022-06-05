using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Mumble.Models;
using Gw2Sharp.WebApi;
using Kenedia.Modules.QoL.Classes;
using Kenedia.Modules.QoL.Strings;
using Kenedia.Modules.QoL.SubModules;
using Kenedia.Modules.QoL.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.QoL
{
	[Export(typeof(Module))]
	public class QoL : Module
	{
		internal static QoL ModuleInstance;

		public static readonly Logger Logger = Logger.GetLogger<QoL>();

		public string CultureString;

		public TextureManager TextureManager;

		public Ticks Ticks;

		public WindowBase2 MainWindow;

		public Hotbar Hotbar;

		public List<SubModule> Modules;

		public SettingEntry<KeyBinding> ReloadKey;

		public SettingEntry<ExpandDirection> HotbarExpandDirection;

		public SettingEntry<Point> HotbarPosition;

		public SettingEntry<bool> HotbarForceOnScreen;

		private bool _DataLoaded;

		public bool FetchingAPI;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public bool DataLoaded
		{
			get
			{
				return _DataLoaded;
			}
			set
			{
				_DataLoaded = value;
				if (value)
				{
					ModuleInstance.OnDataLoaded();
				}
			}
		}

		public event EventHandler DataLoaded_Event;

		public event EventHandler LanguageChanged;

		[ImportingConstructor]
		public QoL([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
			TextureManager = new TextureManager();
			Ticks = new Ticks();
			Modules = new List<SubModule>
			{
				new ItemDestruction(),
				new SkipCutscenes(),
				new ZoomOut(),
				new Resets()
			};
		}

		private void OnDataLoaded()
		{
			this.DataLoaded_Event?.Invoke(this, EventArgs.Empty);
		}

		public void OnLanguageChanged(object sender, EventArgs e)
		{
			this.LanguageChanged?.Invoke(this, EventArgs.Empty);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Expected O, but got Unknown
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			foreach (SubModule module in Modules)
			{
				SettingCollection subSettings = settings.AddSubCollection(module.Name + " - Settings", true, false);
				module.DefineSettings(subSettings);
			}
			HotbarForceOnScreen = settings.DefineSetting<bool>("HotbarForceOnScreen", true, (Func<string>)(() => "Force Hotbar on Screen"), (Func<string>)(() => "When the Hotbar is moved outside the game bounds the bar gets moved back inside."));
			HotbarExpandDirection = settings.DefineSetting<ExpandDirection>("HotbarExpandDirection", ExpandDirection.LeftToRight, (Func<string>)(() => "Expand Direction"), (Func<string>)(() => "Direction the Hotbar is supposed to expand."));
			HotbarExpandDirection.add_SettingChanged((EventHandler<ValueChangedEventArgs<ExpandDirection>>)HotbarExpandDirection_SettingChanged);
			SettingCollection internal_settings = settings.AddSubCollection("Internal Settings", false);
			ReloadKey = internal_settings.DefineSetting<KeyBinding>("ReloadKey", new KeyBinding((Keys)0), (Func<string>)(() => "Reload Button"), (Func<string>)(() => ""));
			ReloadKey.get_Value().set_Enabled(true);
			ReloadKey.get_Value().add_Activated((EventHandler<EventArgs>)RebuildUI);
			HotbarPosition = internal_settings.DefineSetting<Point>("HotbarPosition", new Point(0, 34), (Func<string>)null, (Func<string>)null);
		}

		private void HotbarExpandDirection_SettingChanged(object sender, ValueChangedEventArgs<ExpandDirection> e)
		{
			Hotbar.ExpandDirection = e.get_NewValue();
		}

		protected override void Initialize()
		{
			Logger.Info("Starting  " + ((Module)this).get_Name() + " v." + (object)((Module)this).get_Version().BaseVersion());
			DataLoaded = false;
		}

		private void ToggleWindow_Activated(object sender, EventArgs e)
		{
			WindowBase2 mainWindow = MainWindow;
			if (mainWindow != null)
			{
				mainWindow.ToggleWindow();
			}
		}

		protected override async Task LoadAsync()
		{
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			DataLoaded_Event += QoL_DataLoaded_Event;
			GameService.Overlay.get_UserLocale().add_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
			((Module)this).OnModuleLoaded(e);
			foreach (SubModule module in Modules)
			{
				if (module.Enabled.get_Value())
				{
					module.Initialize();
				}
			}
			GameService.Gw2Mumble.get_UI().add_UISizeChanged((EventHandler<ValueEventArgs<UiSize>>)UI_UISizeChanged);
			LoadData();
		}

		private void UI_UISizeChanged(object sender, ValueEventArgs<UiSize> e)
		{
			EnsureHotbarOnScreen();
		}

		private void QoL_DataLoaded_Event(object sender, EventArgs e)
		{
			CreateUI();
		}

		private void ToggleModule(object sender, MouseEventArgs e)
		{
			if (MainWindow != null)
			{
				MainWindow.ToggleWindow();
			}
		}

		protected override void Update(GameTime gameTime)
		{
			if (!(gameTime.get_TotalGameTime().TotalMilliseconds - Ticks.global >= 5.0))
			{
				return;
			}
			Ticks.global = gameTime.get_TotalGameTime().TotalMilliseconds;
			foreach (SubModule module in Modules)
			{
				if (module.Enabled.get_Value() && module.Loaded && module.Active)
				{
					module.Update(gameTime);
				}
			}
		}

		protected override void Unload()
		{
			DisposeUI();
			foreach (SubModule module in Modules)
			{
				module.Dispose();
			}
			Modules.Clear();
			TextureManager.Dispose();
			TextureManager = null;
			DataLoaded_Event -= QoL_DataLoaded_Event;
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
			ReloadKey.get_Value().remove_Activated((EventHandler<EventArgs>)RebuildUI);
			DataLoaded = false;
			ModuleInstance = null;
		}

		public async Task Fetch_APIData(bool force = false)
		{
		}

		private async Task LoadData()
		{
			DataLoaded = true;
		}

		private async void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			await LoadData();
			OnLanguageChanged(null, null);
		}

		private void RebuildUI(object sender, EventArgs e)
		{
			ScreenNotification.ShowNotification("Rebuilding the UI", (NotificationType)1, (Texture2D)null, 4);
			DisposeUI();
			CreateUI();
		}

		private void DisposeUI()
		{
			WindowBase2 mainWindow = MainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			Hotbar hotbar = Hotbar;
			if (hotbar != null)
			{
				((Control)hotbar).Dispose();
			}
		}

		private void CreateUI()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			Hotbar hotbar = new Hotbar();
			((Control)hotbar).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)hotbar).set_Size(new Point(36, 36));
			hotbar.ButtonSize = new Point(28, 28);
			hotbar.ExpandDirection = HotbarExpandDirection.get_Value();
			((Control)hotbar).set_Location(HotbarPosition.get_Value());
			Hotbar = hotbar;
			foreach (SubModule module in Modules)
			{
				if (module.Enabled.get_Value() && module.ShowOnBar.get_Value())
				{
					Hotbar_Button obj = new Hotbar_Button
					{
						SubModule = module
					};
					((Control)obj).set_BasicTooltipText(string.Format(common.Toggle, module.Name ?? ""));
					module.Hotbar_Button = obj;
					Hotbar.AddButton(module.Hotbar_Button);
				}
			}
			((Control)Hotbar).add_Moved((EventHandler<MovedEventArgs>)Hotbar_Moved);
		}

		private void Hotbar_Moved(object sender, MovedEventArgs e)
		{
			EnsureHotbarOnScreen();
		}

		private void EnsureHotbarOnScreen()
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
				Rectangle localBounds = ((Control)GameService.Graphics.get_SpriteScreen()).get_LocalBounds();
				if (!HotbarForceOnScreen.get_Value() || (((Rectangle)(ref localBounds)).Contains(((Control)Hotbar).get_Location()) && ((Rectangle)(ref localBounds)).Contains(((Control)Hotbar).get_Location().Add(((Control)Hotbar).get_Size()))))
				{
					HotbarPosition.set_Value(((Control)Hotbar).get_Location());
				}
				else
				{
					((Control)Hotbar).set_Location(new Point(Math.Max(0, Math.Min(((Rectangle)(ref localBounds)).get_Right() - Hotbar.CollapsedSize.X, ((Control)Hotbar).get_Location().X)), Math.Max(0, Math.Min(((Rectangle)(ref localBounds)).get_Bottom() - Hotbar.CollapsedSize.Y, ((Control)Hotbar).get_Location().Y))));
				}
			});
		}
	}
}
