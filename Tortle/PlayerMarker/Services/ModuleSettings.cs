using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Tortle.PlayerMarker.Entity;
using Tortle.PlayerMarker.Localization;
using Tortle.PlayerMarker.Util;

namespace Tortle.PlayerMarker.Services
{
	internal sealed class ModuleSettings : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(ModuleSettings));

		private readonly Tortle.PlayerMarker.Entity.PlayerMarker _playerMarker;

		private readonly MarkerTextureManager _markerTextureManager;

		public SettingEntry<bool> Enabled { get; private set; }

		public SettingEntry<string> Color { get; private set; }

		public SettingEntry<float> Opacity { get; private set; }

		public SettingEntry<float> Size { get; private set; }

		public SettingEntry<float> VerticalOffset { get; private set; }

		public SettingEntry<string> ImageName { get; private set; }

		public ModuleSettings(MarkerTextureManager markerTextureManager, Tortle.PlayerMarker.Entity.PlayerMarker playerMarker)
		{
			_markerTextureManager = markerTextureManager;
			_playerMarker = playerMarker;
		}

		public void Initialize(SettingCollection settingCollection)
		{
			Logger.Info("Initializing");
			Enabled = settingCollection.DefineSetting<bool>("PlayerMarkerEnable", true, (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerEnable_Name), (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerEnable_Tooltip));
			Color = settingCollection.DefineSetting<string>("PlayerMarkerColor", "White", (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerColor_Name), (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerColor_Tooltip));
			Size = settingCollection.DefineSetting<float>("PlayerMarkerSize", 0.25f, (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerSize_Name), (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerSize_Tooltip));
			Opacity = settingCollection.DefineSetting<float>("PlayerMarkerOpacity", 1f, (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerOpacity_Name), (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerOpacity_Tooltip));
			VerticalOffset = settingCollection.DefineSetting<float>("PlayerMarkerVerticalOffset", 2.5f, (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerVerticalOffset_Name), (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerVerticalOffset_Tooltip));
			ImageName = settingCollection.DefineSetting<string>("PlayerMarkerImage", _markerTextureManager.DefaultTextures.First().Id, (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerImage_Name), (Func<string>)(() => Tortle.PlayerMarker.Localization.ModuleSettings.PlayerMarkerImage_Tooltip));
			Enabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings_Enabled);
			Color.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings_Color);
			Size.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings_Size);
			Opacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings_Opacity);
			VerticalOffset.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings_VerticalOffset);
			ImageName.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings_Image);
		}

		public void Dispose()
		{
			Logger.Debug("Disposing");
			Enabled.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateSettings_Enabled);
			Size.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings_Size);
			Color.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings_Color);
			Opacity.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings_Opacity);
			VerticalOffset.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)UpdateSettings_VerticalOffset);
			ImageName.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateSettings_Image);
		}

		private void UpdateSettings_Enabled(object sender, ValueChangedEventArgs<bool> e)
		{
			_playerMarker.Visible = Enabled.get_Value();
		}

		private void UpdateSettings_Size(object sender, ValueChangedEventArgs<float> e)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			_playerMarker.Size = new Vector3(e.get_NewValue(), e.get_NewValue(), 0f);
			_playerMarker.UpdateMarker();
		}

		private void UpdateSettings_Color(object sender, ValueChangedEventArgs<string> e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_playerMarker.MarkerColor = ConversionUtil.ToRgb(e.get_NewValue());
			_playerMarker.UpdateMarker();
		}

		private void UpdateSettings_Opacity(object sender, ValueChangedEventArgs<float> e)
		{
			_playerMarker.MarkerOpacity = e.get_NewValue();
		}

		private void UpdateSettings_VerticalOffset(object sender, ValueChangedEventArgs<float> e)
		{
			_playerMarker.VerticalOffset = e.get_NewValue();
		}

		private void UpdateSettings_Image(object sender, ValueChangedEventArgs<string> e)
		{
			_playerMarker.MarkerTexture = _markerTextureManager.Get(e.get_NewValue());
		}
	}
}
