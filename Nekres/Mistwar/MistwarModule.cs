using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nekres.Mistwar.Entities;
using Nekres.Mistwar.Services;

namespace Nekres.Mistwar
{
	[Export(typeof(Module))]
	public class MistwarModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger<MistwarModule>();

		internal static MistwarModule ModuleInstance;

		internal SettingEntry<ColorType> ColorTypeSetting;

		internal SettingEntry<bool> TeamShapesSetting;

		internal SettingEntry<KeyBinding> ToggleMapKeySetting;

		internal SettingEntry<KeyBinding> ToggleMarkersKeySetting;

		internal SettingEntry<KeyBinding> ChatMessageKeySetting;

		internal SettingEntry<float> ColorIntensitySetting;

		internal SettingEntry<bool> DrawSectorsSetting;

		internal SettingEntry<bool> DrawObjectiveNamesSetting;

		internal SettingEntry<bool> DrawRuinMapSetting;

		internal SettingEntry<bool> EnableMarkersSetting;

		internal SettingEntry<bool> HideInCombatSetting;

		internal SettingEntry<bool> HideAlliedMarkersSetting;

		internal SettingEntry<bool> DrawRuinMarkersSetting;

		internal SettingEntry<bool> DrawEmergencyWayPointsSetting;

		internal SettingEntry<bool> DrawDistanceSetting;

		internal SettingEntry<float> MaxViewDistanceSetting;

		internal SettingEntry<float> MarkerScaleSetting;

		internal SettingEntry<bool> MarkerFixedSizeSetting;

		internal SettingEntry<bool> MarkerStickySetting;

		internal Texture2D CornerTex;

		private CornerIcon _moduleIcon;

		internal WvwService WvwService;

		private MapService _mapService;

		internal MarkerService MarkerService;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public MistwarModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Expected O, but got Unknown
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Expected O, but got Unknown
			SettingCollection generalSettings = settings.AddSubCollection("General", false);
			generalSettings.set_RenderInUi(true);
			ColorTypeSetting = generalSettings.DefineSetting<ColorType>("ColorType", ColorType.Normal, (Func<string>)(() => "Color Type"), (Func<string>)(() => "Select a different color type if you have a color deficiency."));
			TeamShapesSetting = generalSettings.DefineSetting<bool>("TeamShapes", true, (Func<string>)(() => "Team Shapes"), (Func<string>)(() => "Enables uniquely shaped objective markers per team."));
			SettingCollection hotKeySettings = settings.AddSubCollection("Control Options", false);
			hotKeySettings.set_RenderInUi(true);
			ToggleMapKeySetting = hotKeySettings.DefineSetting<KeyBinding>("ToggleKey", new KeyBinding((Keys)78), (Func<string>)(() => "Toggle Map"), (Func<string>)(() => "Key used to show and hide the strategic map."));
			ToggleMarkersKeySetting = hotKeySettings.DefineSetting<KeyBinding>("ToggleMarkersKey", new KeyBinding((Keys)219), (Func<string>)(() => "Toggle Markers"), (Func<string>)(() => "Key used to show and hide the objective markers."));
			ChatMessageKeySetting = hotKeySettings.DefineSetting<KeyBinding>("ChatMessageKey", new KeyBinding((Keys)13), (Func<string>)(() => "Chat Message"), (Func<string>)(() => "Give focus to the chat edit box."));
			SettingCollection mapSettings = settings.AddSubCollection("Map", false);
			mapSettings.set_RenderInUi(true);
			DrawSectorsSetting = mapSettings.DefineSetting<bool>("DrawSectors", true, (Func<string>)(() => "Show Sector Boundaries"), (Func<string>)(() => "Indicates if the sector boundaries should be drawn."));
			DrawObjectiveNamesSetting = mapSettings.DefineSetting<bool>("DrawObjectiveNames", true, (Func<string>)(() => "Show Objective Names"), (Func<string>)(() => "Indicates if the names of the objectives should be drawn."));
			DrawRuinMapSetting = mapSettings.DefineSetting<bool>("ShowRuins", true, (Func<string>)(() => "Show Ruins"), (Func<string>)(() => "Indicates if the ruins should be shown."));
			DrawEmergencyWayPointsSetting = mapSettings.DefineSetting<bool>("ShowEmergencyWayPoints", false, (Func<string>)(() => "Show Emergency Waypoints"), (Func<string>)(() => "Shows your team's Emergency Waypoints."));
			ColorIntensitySetting = mapSettings.DefineSetting<float>("ColorIntensity", 80f, (Func<string>)(() => "Color Intensity"), (Func<string>)(() => "Intensity of the background color."));
			SettingCollection markerSettings = settings.AddSubCollection("Markers", true, false);
			markerSettings.set_RenderInUi(true);
			EnableMarkersSetting = markerSettings.DefineSetting<bool>("EnableMarkers", true, (Func<string>)(() => "Enable Markers"), (Func<string>)(() => "Enables the markers overlay which shows objectives at their world position."));
			HideAlliedMarkersSetting = markerSettings.DefineSetting<bool>("HideAlliedMarkers", false, (Func<string>)(() => "Hide Allied Objectives"), (Func<string>)(() => "Only hostile objectives will be shown."));
			HideInCombatSetting = markerSettings.DefineSetting<bool>("HideInCombat", true, (Func<string>)(() => "Hide in Combat"), (Func<string>)(() => "Only the closest objective will be shown when in combat."));
			DrawRuinMarkersSetting = markerSettings.DefineSetting<bool>("ShowRuinMarkers", true, (Func<string>)(() => "Show Ruins"), (Func<string>)(() => "Show markers for the ruins."));
			DrawDistanceSetting = markerSettings.DefineSetting<bool>("ShowDistance", true, (Func<string>)(() => "Show Distance"), (Func<string>)(() => "Show flight distance to objectives."));
			MaxViewDistanceSetting = markerSettings.DefineSetting<float>("MaxViewDistance", 50f, (Func<string>)(() => "Max View Distance"), (Func<string>)(() => "The max view distance at which an objective marker can be seen."));
			MarkerScaleSetting = markerSettings.DefineSetting<float>("ScaleRatio", 70f, (Func<string>)(() => "Marker Size"), (Func<string>)(() => "Changes the maximum size of the markers."));
			MarkerFixedSizeSetting = markerSettings.DefineSetting<bool>("FixedSize", false, (Func<string>)(() => "Fixed Size"), (Func<string>)(() => "Disables the distance-based down-scaling of objective markers."));
			MarkerStickySetting = markerSettings.DefineSetting<bool>("Sticky", true, (Func<string>)(() => "Sticky"), (Func<string>)(() => "Objectives which are out of view will have their marker stick to the edge of your screen if enabled."));
		}

		protected override void Initialize()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			ChatMessageKeySetting.get_Value().set_Enabled(false);
			CornerTex = ContentsManager.GetTexture("corner_icon.png");
			_moduleIcon = new CornerIcon(AsyncTexture2D.op_Implicit(CornerTex), ((Module)this).get_Name());
			WvwService = new WvwService(Gw2ApiManager);
			if (EnableMarkersSetting.get_Value())
			{
				MarkerService = new MarkerService();
			}
			_mapService = new MapService(DirectoriesManager, WvwService, GetModuleProgressHandler());
		}

		protected override async Task LoadAsync()
		{
			await WvwService.LoadAsync();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			ColorIntensitySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnColorIntensitySettingChanged);
			ToggleMapKeySetting.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleKeyActivated);
			ToggleMarkersKeySetting.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleMarkersKeyActivated);
			EnableMarkersSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnableMarkersSettingChanged);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			ToggleMapKeySetting.get_Value().set_Enabled(true);
			ToggleMarkersKeySetting.get_Value().set_Enabled(true);
			OnColorIntensitySettingChanged(null, new ValueChangedEventArgs<float>(0f, ColorIntensitySetting.get_Value()));
			((Control)_moduleIcon).add_Click((EventHandler<MouseEventArgs>)OnModuleIconClick);
			((Module)this).OnModuleLoaded(e);
		}

		private void OnModuleIconClick(object o, MouseEventArgs e)
		{
			_mapService.Toggle();
		}

		private void UpdateModuleLoading(string loadingMessage)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (_moduleIcon == null)
			{
				return;
			}
			_moduleIcon.set_LoadingMessage(loadingMessage);
			if (loadingMessage == null && !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch())
			{
				CornerIcon moduleIcon = _moduleIcon;
				if (moduleIcon != null)
				{
					((Control)moduleIcon).Dispose();
				}
			}
		}

		public IProgress<string> GetModuleProgressHandler()
		{
			return new Progress<string>(UpdateModuleLoading);
		}

		private void OnToggleKeyActivated(object o, EventArgs e)
		{
			_mapService.Toggle();
		}

		private void OnToggleMarkersKeyActivated(object o, EventArgs e)
		{
			EnableMarkersSetting.set_Value(!EnableMarkersSetting.get_Value());
		}

		protected override async void Update(GameTime gameTime)
		{
			_mapService.DownloadMaps(WvwService.GetWvWMapIds());
			if (Gw2ApiManager.HasPermission((TokenPermission)1))
			{
				await WvwService.Update();
			}
		}

		private void OnColorIntensitySettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_mapService.ColorIntensity = (100f - e.get_NewValue()) / 100f;
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			ToggleMapKeySetting.get_Value().set_Enabled(GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch());
		}

		private void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			if (GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch())
			{
				CornerIcon moduleIcon = _moduleIcon;
				if (moduleIcon != null)
				{
					((Control)moduleIcon).Dispose();
				}
				_moduleIcon = new CornerIcon(AsyncTexture2D.op_Implicit(CornerTex), ((Module)this).get_Name());
				((Control)_moduleIcon).add_Click((EventHandler<MouseEventArgs>)OnModuleIconClick);
				ToggleMapKeySetting.get_Value().set_Enabled(true);
			}
			else
			{
				CornerIcon moduleIcon2 = _moduleIcon;
				if (moduleIcon2 != null)
				{
					((Control)moduleIcon2).Dispose();
				}
				ToggleMapKeySetting.get_Value().set_Enabled(false);
			}
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			if (e.get_Value() && GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch())
			{
				CornerIcon moduleIcon = _moduleIcon;
				if (moduleIcon != null)
				{
					((Control)moduleIcon).Dispose();
				}
				_moduleIcon = new CornerIcon(AsyncTexture2D.op_Implicit(CornerTex), ((Module)this).get_Name());
				((Control)_moduleIcon).add_Click((EventHandler<MouseEventArgs>)OnModuleIconClick);
				ToggleMapKeySetting.get_Value().set_Enabled(true);
			}
			else
			{
				CornerIcon moduleIcon2 = _moduleIcon;
				if (moduleIcon2 != null)
				{
					((Control)moduleIcon2).Dispose();
				}
				ToggleMapKeySetting.get_Value().set_Enabled(false);
			}
		}

		private async void OnEnableMarkersSettingChanged(object o, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue())
			{
				if (MarkerService == null)
				{
					MarkerService = new MarkerService();
				}
				if (GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch())
				{
					List<WvwObjectiveEntity> obj = await WvwService.GetObjectives(GameService.Gw2Mumble.get_CurrentMap().get_Id());
					if (MarkerService != null)
					{
						MarkerService.ReloadMarkers(obj);
						MarkerService.Toggle();
					}
				}
			}
			else
			{
				MarkerService?.Dispose();
				MarkerService = null;
			}
		}

		protected override void Unload()
		{
			ColorIntensitySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnColorIntensitySettingChanged);
			ToggleMapKeySetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnToggleKeyActivated);
			ToggleMarkersKeySetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnToggleMarkersKeyActivated);
			EnableMarkersSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnableMarkersSettingChanged);
			ToggleMapKeySetting.get_Value().set_Enabled(false);
			ToggleMarkersKeySetting.get_Value().set_Enabled(false);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			_mapService?.Dispose();
			CornerIcon moduleIcon = _moduleIcon;
			if (moduleIcon != null)
			{
				((Control)moduleIcon).Dispose();
			}
			Texture2D cornerTex = CornerTex;
			if (cornerTex != null)
			{
				((GraphicsResource)cornerTex).Dispose();
			}
			MarkerService?.Dispose();
			WvwService?.Dispose();
			ModuleInstance = null;
		}
	}
}
