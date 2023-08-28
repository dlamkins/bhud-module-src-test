using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Views;
using Kenedia.Modules.QoL.Services;
using Kenedia.Modules.QoL.SubModules;
using Kenedia.Modules.QoL.SubModules.CopyItemName;
using Kenedia.Modules.QoL.SubModules.GameResets;
using Kenedia.Modules.QoL.SubModules.ItemDestruction;
using Kenedia.Modules.QoL.SubModules.SkipCutscenes;
using Kenedia.Modules.QoL.SubModules.WaypointPaste;
using Kenedia.Modules.QoL.SubModules.WikiSearch;
using Kenedia.Modules.QoL.SubModules.ZoomOut;
using Kenedia.Modules.QoL.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL
{
	[Export(typeof(Module))]
	public class QoL : BaseModule<QoL, StandardWindow, Settings, PathCollection>
	{
		private double _tick;

		public Hotbar Hotbar { get; set; }

		public Dictionary<SubModuleType, SubModule> SubModules { get; } = new Dictionary<SubModuleType, SubModule>();


		[ImportingConstructor]
		public QoL([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			BaseModule<QoL, StandardWindow, Settings, PathCollection>.ModuleInstance = this;
			HasGUI = true;
			AutoLoadGUI = true;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			base.Settings = new Settings(settings);
			base.Settings.HotbarExpandDirection.add_SettingChanged((EventHandler<ValueChangedEventArgs<ExpandType>>)HotbarExpandDirection_SettingChanged);
		}

		private void HotbarExpandDirection_SettingChanged(object sender, ValueChangedEventArgs<ExpandType> e)
		{
			if (Hotbar != null)
			{
				Hotbar.ExpandType = e.get_NewValue();
			}
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView(delegate
			{
				BaseSettingsWindow settingsWindow = base.SettingsWindow;
				if (settingsWindow != null)
				{
					((WindowBase2)settingsWindow).ToggleWindow();
				}
			});
		}

		protected override void Initialize()
		{
			base.Initialize();
			base.Paths = new PathCollection(base.DirectoriesManager, ((Module)this).get_Name());
			BaseModule<QoL, StandardWindow, Settings, PathCollection>.Logger.Info("Starting " + ((Module)this).get_Name() + " v." + (object)((Module)this).get_Version().BaseVersion());
			LoadSubModules();
		}

		protected override async Task LoadAsync()
		{
			await base.LoadAsync();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (Hotbar != null)
			{
				((Control)Hotbar).set_Visible(GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen());
			}
			foreach (KeyValuePair<SubModuleType, SubModule> subModule in SubModules)
			{
				subModule.Value.Update(gameTime);
			}
		}

		protected override void LoadGUI()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			base.LoadGUI();
			Hotbar hotbar = Hotbar;
			if (hotbar != null)
			{
				((Control)hotbar).Dispose();
			}
			Hotbar hotbar2 = new Hotbar();
			((Control)hotbar2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			hotbar2.TextureRectangle = new Rectangle(new Point(50, 50), new Point(200, 50));
			((Control)hotbar2).set_Location(base.Settings.HotbarPosition.get_Value());
			hotbar2.ExpandType = base.Settings.HotbarExpandDirection.get_Value();
			hotbar2.OnMoveAction = delegate(Point p)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				base.Settings.HotbarPosition.set_Value(p);
			};
			hotbar2.OpenSettingsAction = delegate
			{
				BaseSettingsWindow settingsWindow2 = base.SettingsWindow;
				if (settingsWindow2 != null)
				{
					((WindowBase2)settingsWindow2).ToggleWindow();
				}
			};
			Hotbar = hotbar2;
			foreach (SubModule subModule in SubModules.Values)
			{
				Hotbar.AddItem((ICheckable)(object)subModule.ToggleControl);
			}
			AsyncTexture2D settingsBg = AsyncTexture2D.FromAssetId(155997);
			Texture2D cutSettingsBg = Texture2DExtension.GetRegion(settingsBg.get_Texture(), 0, 0, settingsBg.get_Width() - 482, settingsBg.get_Height() - 390);
			SettingsWindow settingsWindow = new SettingsWindow(settingsBg, new Rectangle(30, 30, cutSettingsBg.get_Width() + 10, cutSettingsBg.get_Height()), new Rectangle(30, 35, cutSettingsBg.get_Width() - 5, cutSettingsBg.get_Height() - 15), base.Settings, base.SharedSettingsView, SubModules);
			((Control)settingsWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)settingsWindow).set_Title("❤");
			((WindowBase2)settingsWindow).set_Subtitle("❤");
			((WindowBase2)settingsWindow).set_SavesPosition(true);
			((WindowBase2)settingsWindow).set_Id(((Module)this).get_Name() + " SettingsWindow");
			settingsWindow.Version = base.ModuleVersion;
			base.SettingsWindow = settingsWindow;
		}

		protected override void UnloadGUI()
		{
			base.UnloadGUI();
			BaseSettingsWindow settingsWindow = base.SettingsWindow;
			if (settingsWindow != null)
			{
				((Control)settingsWindow).Dispose();
			}
			Hotbar hotbar = Hotbar;
			if (hotbar != null)
			{
				((Control)hotbar).Dispose();
			}
		}

		protected override void Unload()
		{
			base.Unload();
			foreach (SubModule value in SubModules.Values)
			{
				value?.Unload();
			}
			SubModules.Clear();
		}

		protected override void ReloadKey_Activated(object sender, EventArgs e)
		{
			base.ReloadKey_Activated(sender, e);
			foreach (SubModule value in SubModules.Values)
			{
				value?.Unload();
			}
			SubModules.Clear();
			LoadSubModules();
		}

		private void LoadSubModules()
		{
			SubModules.Add(SubModuleType.GameResets, new GameResets(base.SettingCollection));
			SubModules.Add(SubModuleType.ZoomOut, new ZoomOut(base.SettingCollection));
			SubModules.Add(SubModuleType.SkipCutscenes, new SkipCutscenes(base.SettingCollection, base.Services.GameStateDetectionService));
			SubModules.Add(SubModuleType.ItemDestruction, new ItemDestruction(base.SettingCollection));
			SubModules.Add(SubModuleType.WikiSearch, new WikiSearch(base.SettingCollection));
			SubModules.Add(SubModuleType.WaypointPaste, new WaypointPaste(base.SettingCollection));
			SubModules.Add(SubModuleType.CopyItemName, new CopyItemName(base.SettingCollection));
			foreach (SubModule value in SubModules.Values)
			{
				value?.Load();
			}
		}
	}
}
