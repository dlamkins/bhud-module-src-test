using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

		internal SettingEntry<float> ColorIntensitySetting;

		internal SettingEntry<bool> DrawSectorsSetting;

		internal SettingEntry<float> ScaleRatioSetting;

		internal SettingEntry<bool> DrawObjectiveNamesSetting;

		internal SettingEntry<float> OpacitySetting;

		internal SettingEntry<bool> DrawRuinMapSetting;

		internal SettingEntry<bool> EnableMarkersSetting;

		internal SettingEntry<bool> HideInCombatSetting;

		internal SettingEntry<bool> DrawRuinMarkersSetting;

		internal SettingEntry<float> MaxViewDistanceSetting;

		internal SettingEntry<float> MarkerScaleSetting;

		private CornerIcon _moduleIcon;

		internal WvwService WvwService;

		private MapService _mapService;

		internal MarkerService MarkerService;

		private AsyncTexture2D _cornerTex;

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
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Expected O, but got Unknown
			SettingCollection generalSettings = settings.AddSubCollection("General", true, false);
			ColorTypeSetting = generalSettings.DefineSetting<ColorType>("ColorType", ColorType.Normal, (Func<string>)(() => "Color Type"), (Func<string>)(() => "Select a different color type if you have a color deficiency."));
			TeamShapesSetting = generalSettings.DefineSetting<bool>("TeamShapes", true, (Func<string>)(() => "Team Shapes"), (Func<string>)(() => "Enables uniquely shaped objective markers per team."));
			SettingCollection hotKeySettings = settings.AddSubCollection("Control Options", true, false);
			ToggleMapKeySetting = hotKeySettings.DefineSetting<KeyBinding>("ToggleKey", new KeyBinding((Keys)78), (Func<string>)(() => "Toggle Map"), (Func<string>)(() => "Key used to show and hide the strategic map."));
			ToggleMarkersKeySetting = hotKeySettings.DefineSetting<KeyBinding>("ToggleMarkersKey", new KeyBinding((Keys)219), (Func<string>)(() => "Toggle Markers"), (Func<string>)(() => "Key used to show and hide the objective markers."));
			SettingCollection mapSettings = settings.AddSubCollection("Map", true, false);
			DrawSectorsSetting = mapSettings.DefineSetting<bool>("DrawSectors", true, (Func<string>)(() => "Show Sector Boundaries"), (Func<string>)(() => "Indicates if the sector boundaries should be drawn."));
			DrawObjectiveNamesSetting = mapSettings.DefineSetting<bool>("DrawObjectiveNames", true, (Func<string>)(() => "Show Objective Names"), (Func<string>)(() => "Indicates if the names of the objectives should be drawn."));
			DrawRuinMapSetting = mapSettings.DefineSetting<bool>("ShowRuins", true, (Func<string>)(() => "Show Ruins"), (Func<string>)(() => "Indicates if the ruins should be shown."));
			OpacitySetting = mapSettings.DefineSetting<float>("Opacity", 80f, (Func<string>)(() => "Opacity"), (Func<string>)(() => "Changes the opacity of the tactical map interface."));
			ColorIntensitySetting = mapSettings.DefineSetting<float>("ColorIntensity", 80f, (Func<string>)(() => "Color Intensity"), (Func<string>)(() => "Intensity of the background color."));
			ScaleRatioSetting = mapSettings.DefineSetting<float>("ScaleRatio", 80f, (Func<string>)(() => "Scale Ratio"), (Func<string>)(() => "Changes the size of the tactical map interface"));
			SettingCollection markerSettings = settings.AddSubCollection("Markers", true, false);
			EnableMarkersSetting = markerSettings.DefineSetting<bool>("EnableMarkers", true, (Func<string>)(() => "Enable Markers"), (Func<string>)(() => "Enables the markers overlay which shows objectives at their world position."));
			HideInCombatSetting = markerSettings.DefineSetting<bool>("HideInCombat", true, (Func<string>)(() => "Hide in Combat"), (Func<string>)(() => "Shows only the closest objective in combat and hides all others."));
			DrawRuinMarkersSetting = markerSettings.DefineSetting<bool>("ShowRuinMarkers", true, (Func<string>)(() => "Show Ruins"), (Func<string>)(() => "Shows markers for the ruins."));
			MaxViewDistanceSetting = markerSettings.DefineSetting<float>("MaxViewDistance", 50f, (Func<string>)(() => "Max View Distance"), (Func<string>)(() => "The max view distance at which an objective marker can be seen."));
			MarkerScaleSetting = markerSettings.DefineSetting<float>("ScaleRatio", 70f, (Func<string>)(() => "Scale Ratio"), (Func<string>)(() => "Changes the size of the markers."));
		}

		protected override void Initialize()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			_cornerTex = new AsyncTexture2D(ContentsManager.GetTexture("corner_icon.png"));
			_moduleIcon = new CornerIcon(_cornerTex, ((Module)this).get_Name());
			WvwService = new WvwService(Gw2ApiManager);
			if (EnableMarkersSetting.get_Value())
			{
				MarkerService = new MarkerService();
			}
			_mapService = new MapService(DirectoriesManager, WvwService, GetModuleProgressHandler());
		}

		protected override async Task LoadAsync()
		{
			if (Gw2ApiManager.HasPermission((TokenPermission)1))
			{
				MapService mapService = _mapService;
				WvwService wvwService = WvwService;
				mapService.DownloadMaps(await wvwService.GetWvWMapIds(await WvwService.GetWorldId()));
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
			ColorIntensitySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnColorIntensitySettingChanged);
			ToggleMapKeySetting.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleKeyActivated);
			ToggleMarkersKeySetting.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleMarkersKeyActivated);
			OpacitySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnOpacitySettingChanged);
			EnableMarkersSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnableMarkersSettingChanged);
			ToggleMapKeySetting.get_Value().set_Enabled(true);
			ToggleMarkersKeySetting.get_Value().set_Enabled(true);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			OnColorIntensitySettingChanged(null, new ValueChangedEventArgs<float>(0f, ColorIntensitySetting.get_Value()));
			OnOpacitySettingChanged(null, new ValueChangedEventArgs<float>(0f, OpacitySetting.get_Value()));
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
			if (loadingMessage == null && !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld())
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

		private async void OnSubtokenUpdated(object o, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			if (e.get_Value().Contains((TokenPermission)1))
			{
				MapService mapService = _mapService;
				WvwService wvwService = WvwService;
				mapService.DownloadMaps(await wvwService.GetWvWMapIds(await WvwService.GetWorldId()));
			}
		}

		private void OnOpacitySettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_mapService.Opacity = MathHelper.Clamp(e.get_NewValue() / 100f, 0f, 1f);
		}

		private void OnToggleKeyActivated(object o, EventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (GameUtil.IsUiAvailable() && GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld())
			{
				_mapService.Toggle();
			}
		}

		private void OnToggleMarkersKeyActivated(object o, EventArgs e)
		{
			EnableMarkersSetting.set_Value(!EnableMarkersSetting.get_Value());
			if (EnableMarkersSetting.get_Value())
			{
				MarkerService?.Dispose();
			}
			else
			{
				MarkerService = new MarkerService(WvwService.CurrentObjectives);
			}
		}

		protected override async void Update(GameTime gameTime)
		{
			await WvwService.Update();
		}

		private void OnColorIntensitySettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_mapService.ColorIntensity = (100f - e.get_NewValue()) / 100f;
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			ToggleMapKeySetting.get_Value().set_Enabled(GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld());
		}

		private void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			if (GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld())
			{
				CornerIcon moduleIcon = _moduleIcon;
				if (moduleIcon != null)
				{
					((Control)moduleIcon).Dispose();
				}
				_moduleIcon = new CornerIcon(_cornerTex, ((Module)this).get_Name());
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

		private void OnEnableMarkersSettingChanged(object o, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue())
			{
				if (MarkerService == null)
				{
					MarkerService = new MarkerService(WvwService.CurrentObjectives);
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
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
			ColorIntensitySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnColorIntensitySettingChanged);
			ToggleMapKeySetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnToggleKeyActivated);
			ToggleMarkersKeySetting.get_Value().remove_Activated((EventHandler<EventArgs>)OnToggleMarkersKeyActivated);
			OpacitySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnOpacitySettingChanged);
			EnableMarkersSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnableMarkersSettingChanged);
			ToggleMapKeySetting.get_Value().set_Enabled(false);
			ToggleMarkersKeySetting.get_Value().set_Enabled(false);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			MarkerService?.Dispose();
			_mapService?.Dispose();
			CornerIcon moduleIcon = _moduleIcon;
			if (moduleIcon != null)
			{
				((Control)moduleIcon).Dispose();
			}
			AsyncTexture2D cornerTex = _cornerTex;
			if (cornerTex != null)
			{
				cornerTex.Dispose();
			}
			ModuleInstance = null;
		}
	}
}
