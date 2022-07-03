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

		internal SettingEntry<KeyBinding> ToggleKeySetting;

		internal SettingEntry<ColorType> ColorTypeSetting;

		internal SettingEntry<float> ColorIntensitySetting;

		internal SettingEntry<bool> DrawSectorsSetting;

		internal SettingEntry<float> ScaleRatioSetting;

		internal SettingEntry<bool> DrawObjectiveNamesSetting;

		internal SettingEntry<float> OpacitySetting;

		private CornerIcon _moduleIcon;

		private WvwService _wvwService;

		private MapService _mapService;

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
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			ToggleKeySetting = settings.DefineSetting<KeyBinding>("ToggleKey", new KeyBinding((Keys)78), (Func<string>)(() => "Toggle Key"), (Func<string>)(() => "Key used to show and hide the tactical map overlay."));
			ColorTypeSetting = settings.DefineSetting<ColorType>("ColorType", ColorType.Normal, (Func<string>)(() => "Color Type"), (Func<string>)(() => "Select a different color type if you have a color deficiency."));
			ColorIntensitySetting = settings.DefineSetting<float>("ColorIntensity", 80f, (Func<string>)(() => "Color Intensity"), (Func<string>)(() => "Intensity of the background color."));
			OpacitySetting = settings.DefineSetting<float>("Opacity", 80f, (Func<string>)(() => "Opacity"), (Func<string>)(() => "Changes the opacity of the tactical map interface."));
			ScaleRatioSetting = settings.DefineSetting<float>("ScaleRatio", 80f, (Func<string>)(() => "Scale Ratio"), (Func<string>)(() => "Changes the size of the tactical map interface"));
			DrawSectorsSetting = settings.DefineSetting<bool>("DrawSectors", true, (Func<string>)(() => "Draw Sector Boundaries"), (Func<string>)(() => "Indicates if the sector boundaries should be drawn."));
			DrawObjectiveNamesSetting = settings.DefineSetting<bool>("DrawObjectiveNames", true, (Func<string>)(() => "Draw Objective Names"), (Func<string>)(() => "Indicates if the names of the objectives should be drawn."));
		}

		protected override void Initialize()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			_moduleIcon = new CornerIcon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("corner_icon.png")), ((Module)this).get_Name());
			_wvwService = new WvwService(Gw2ApiManager);
			_mapService = new MapService(Gw2ApiManager, DirectoriesManager, _wvwService, GetModuleProgressHandler());
		}

		protected override async Task LoadAsync()
		{
			if (Gw2ApiManager.HasPermission((TokenPermission)1))
			{
				MapService mapService = _mapService;
				WvwService wvwService = _wvwService;
				mapService.DownloadMaps(await wvwService.GetWvWMapIds(await _wvwService.GetWorldId()));
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
			ColorIntensitySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnColorIntensitySettingChanged);
			ToggleKeySetting.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleKeyActivated);
			OpacitySetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnOpacitySettingChanged);
			ToggleKeySetting.get_Value().set_Enabled(true);
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
			if (_moduleIcon != null)
			{
				_moduleIcon.set_LoadingMessage(loadingMessage);
				if (loadingMessage == null && !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld())
				{
					((Control)_moduleIcon).Hide();
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
				WvwService wvwService = _wvwService;
				mapService.DownloadMaps(await wvwService.GetWvWMapIds(await _wvwService.GetWorldId()));
			}
		}

		private bool IsUiAvailable()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			return false;
		}

		private void OnOpacitySettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_mapService.Opacity = MathHelper.Clamp(e.get_NewValue() / 100f, 0f, 1f);
		}

		private void OnToggleKeyActivated(object o, EventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (IsUiAvailable() && GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld())
			{
				_mapService.Toggle();
			}
		}

		protected override async void Update(GameTime gameTime)
		{
			await _wvwService.Update();
		}

		private void OnColorIntensitySettingChanged(object o, ValueChangedEventArgs<float> e)
		{
			_mapService.ColorIntensity = (100f - e.get_NewValue()) / 100f;
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			ToggleKeySetting.get_Value().set_Enabled(GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld());
		}

		private void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWorldVsWorld())
			{
				((Control)_moduleIcon).Show();
				ToggleKeySetting.get_Value().set_Enabled(true);
			}
			else
			{
				((Control)_moduleIcon).Hide();
				ToggleKeySetting.get_Value().set_Enabled(false);
			}
		}

		protected override void Unload()
		{
			_mapService?.Dispose();
			CornerIcon moduleIcon = _moduleIcon;
			if (moduleIcon != null)
			{
				((Control)moduleIcon).Dispose();
			}
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			ToggleKeySetting.get_Value().set_Enabled(false);
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnSubtokenUpdated);
			ColorIntensitySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnColorIntensitySettingChanged);
			OpacitySetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnOpacitySettingChanged);
			ModuleInstance = null;
		}
	}
}
