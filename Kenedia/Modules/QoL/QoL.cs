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
				new ZoomOut()
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
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			foreach (SubModule module in Modules)
			{
				SettingCollection subSettings = settings.AddSubCollection(module.Name + " - Settings", true, false);
				module.DefineSettings(subSettings);
			}
			SettingCollection internal_settings = settings.AddSubCollection("Internal Settings", false);
			ReloadKey = internal_settings.DefineSetting<KeyBinding>("ReloadKey", new KeyBinding((Keys)0), (Func<string>)(() => "Reload Button"), (Func<string>)(() => ""));
			ReloadKey.get_Value().set_Enabled(true);
			ReloadKey.get_Value().add_Activated((EventHandler<EventArgs>)RebuildUI);
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
			LoadData();
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
				if (module.Loaded && module.Active)
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
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			Hotbar hotbar = new Hotbar();
			((Control)hotbar).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)hotbar).set_Location(new Point(0, 34));
			((Control)hotbar).set_Size(new Point(36, 36));
			hotbar.ButtonSize = new Point(28, 28);
			Hotbar = hotbar;
			foreach (SubModule module in Modules)
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
	}
}
