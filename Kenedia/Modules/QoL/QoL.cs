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
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.QoL.Controls;
using Kenedia.Modules.QoL.Services;
using Kenedia.Modules.QoL.SubModules;
using Kenedia.Modules.QoL.SubModules.AutoSniff;
using Kenedia.Modules.QoL.SubModules.CopyItemName;
using Kenedia.Modules.QoL.SubModules.GameResets;
using Kenedia.Modules.QoL.SubModules.ItemDestruction;
using Kenedia.Modules.QoL.SubModules.SchemanticProcessing;
using Kenedia.Modules.QoL.SubModules.SkipCutscenes;
using Kenedia.Modules.QoL.SubModules.WaypointPaste;
using Kenedia.Modules.QoL.SubModules.WikiSearch;
using Kenedia.Modules.QoL.SubModules.ZoomOut;
using Kenedia.Modules.QoL.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL
{
	[Export(typeof(Module))]
	public class QoL : BaseModule<QoL, StandardWindow, Settings, PathCollection>
	{
		private double _tick;

		public ModuleHotbar Hotbar { get; set; }

		public ClientWindowService ClientWindowService { get; } = new ClientWindowService();


		public Dictionary<SubModuleType, SubModule> SubModules { get; } = new Dictionary<SubModuleType, SubModule>();


		[ImportingConstructor]
		public QoL([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			HasGUI = true;
			AutoLoadGUI = true;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			base.Settings.HotbarExpandDirection.SettingChanged += HotbarExpandDirection_SettingChanged;
			base.Settings.HotbarButtonSorting.SettingChanged += HotbarButtonSorting_SettingChanged;
		}

		private void HotbarButtonSorting_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<SortType> e)
		{
			if (Hotbar != null)
			{
				Hotbar.SortType = e.NewValue;
			}
		}

		private void HotbarExpandDirection_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<ExpandType> e)
		{
			if (Hotbar != null)
			{
				Hotbar.ExpandType = e.NewValue;
			}
		}

		public override IView GetSettingsView()
		{
			return new SettingsView(delegate
			{
				base.SettingsWindow?.ToggleWindow();
			});
		}

		protected override void Initialize()
		{
			base.Initialize();
			BaseModule<QoL, StandardWindow, Settings, PathCollection>.Logger.Info("Starting " + base.Name + " v." + (object)base.Version.BaseVersion());
			LoadSubModules();
		}

		protected override ServiceCollection DefineServices(ServiceCollection services)
		{
			ServiceCollection result = base.DefineServices(services);
			services.AddSingleton<GameResets>();
			services.AddSingleton<ZoomOut>();
			services.AddSingleton<SkipCutscenes>();
			services.AddSingleton<ItemDestruction>();
			services.AddSingleton<WikiSearch>();
			services.AddSingleton<WaypointPaste>();
			services.AddSingleton<CopyItemName>();
			services.AddSingleton<SchemanticProcessing>();
			services.AddSingleton<AutoSniff>();
			return result;
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
				Hotbar.Visible = GameService.GameIntegration.Gw2Instance.IsInGame && !GameService.Gw2Mumble.UI.IsMapOpen;
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
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			base.LoadGUI();
			Hotbar?.Dispose();
			Hotbar = new ModuleHotbar
			{
				Parent = GameService.Graphics.SpriteScreen,
				TextureRectangle = new Rectangle(new Point(50, 50), new Point(200, 50)),
				Location = base.Settings.HotbarPosition.Value,
				ExpandType = base.Settings.HotbarExpandDirection.Value,
				SortType = base.Settings.HotbarButtonSorting.Value,
				OnMoveAction = delegate(Point p)
				{
					//IL_000b: Unknown result type (might be due to invalid IL or missing references)
					base.Settings.HotbarPosition.Value = p;
				},
				OpenSettingsAction = delegate
				{
					base.SettingsWindow?.ToggleWindow();
				}
			};
			foreach (SubModule subModule in SubModules.Values)
			{
				Hotbar.AddItem(subModule.ToggleControl);
			}
			AsyncTexture2D settingsBg = AsyncTexture2D.FromAssetId(155997);
			Texture2D cutSettingsBg = settingsBg.Texture.GetRegion(0, 0, settingsBg.Width - 482, settingsBg.Height - 390);
			base.SettingsWindow = new SettingsWindow(settingsBg, new Rectangle(30, 30, cutSettingsBg.get_Width() + 10, cutSettingsBg.get_Height()), new Rectangle(30, 35, cutSettingsBg.get_Width() - 5, cutSettingsBg.get_Height() - 15), base.Settings, base.SharedSettingsView, SubModules)
			{
				Parent = GameService.Graphics.SpriteScreen,
				Title = "❤",
				Subtitle = "❤",
				SavesPosition = true,
				Id = base.Name + " SettingsWindow",
				Version = base.ModuleVersion
			};
		}

		protected override void UnloadGUI()
		{
			base.UnloadGUI();
			base.SettingsWindow?.Dispose();
			Hotbar?.Dispose();
		}

		protected override void Unload()
		{
			base.Unload();
			foreach (SubModule value in SubModules.Values)
			{
				value?.Unload();
			}
			SubModules.Clear();
			base.Settings.HotbarExpandDirection.SettingChanged -= HotbarExpandDirection_SettingChanged;
			base.Settings.HotbarButtonSorting.SettingChanged -= HotbarButtonSorting_SettingChanged;
		}

		protected override void ReloadKey_Activated(object sender, EventArgs e)
		{
			BaseModule<QoL, StandardWindow, Settings, PathCollection>.Logger.Debug("ReloadKey_Activated: " + base.Name);
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
			SubModules.Add(SubModuleType.GameResets, ServiceProviderServiceExtensions.GetRequiredService<GameResets>(base.ServiceProvider));
			SubModules.Add(SubModuleType.ZoomOut, ServiceProviderServiceExtensions.GetRequiredService<ZoomOut>(base.ServiceProvider));
			SubModules.Add(SubModuleType.SkipCutscenes, ServiceProviderServiceExtensions.GetRequiredService<SkipCutscenes>(base.ServiceProvider));
			SubModules.Add(SubModuleType.ItemDestruction, ServiceProviderServiceExtensions.GetRequiredService<ItemDestruction>(base.ServiceProvider));
			SubModules.Add(SubModuleType.WikiSearch, ServiceProviderServiceExtensions.GetRequiredService<WikiSearch>(base.ServiceProvider));
			SubModules.Add(SubModuleType.WaypointPaste, ServiceProviderServiceExtensions.GetRequiredService<WaypointPaste>(base.ServiceProvider));
			SubModules.Add(SubModuleType.CopyItemName, ServiceProviderServiceExtensions.GetRequiredService<CopyItemName>(base.ServiceProvider));
			SubModules.Add(SubModuleType.SchemanticProcessing, ServiceProviderServiceExtensions.GetRequiredService<SchemanticProcessing>(base.ServiceProvider));
			SubModules.Add(SubModuleType.AutoSniff, ServiceProviderServiceExtensions.GetRequiredService<AutoSniff>(base.ServiceProvider));
			foreach (SubModule value in SubModules.Values)
			{
				value?.Load();
			}
		}
	}
}
