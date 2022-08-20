using System;
using System.Collections.Generic;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Ideka.BHUDCommon;

namespace Ideka.RacingMeter
{
	public class RacingSettings : IDisposable
	{
		public enum HorizontalAlignment
		{
			LeftToRight,
			RightToLeft,
			Center
		}

		public enum VerticalAlignment
		{
			TopDown,
			BottomUp,
			Middle
		}

		private readonly DisposableCollection _dc = new DisposableCollection();

		public GenericSetting<int> MumblePollingRate { get; }

		public GenericSetting<float> SpeedometerAnchorY { get; }

		public GenericSetting<bool> ShowSpeedometer { get; }

		private KeyBindingSetting ToggleSpeedometer { get; }

		public IReadOnlyDictionary<MountType, (GenericSetting<bool> setting, RectAnchor meter)> Meters { get; }

		public GenericSetting<float> SfxVolumeMultiplier { get; }

		public GenericSetting<int> MaxGhostData { get; }

		public GenericSetting<bool> AutoLocalGhost { get; }

		public GenericSetting<int> ShownCheckpoints { get; }

		public GenericSetting<bool> ShowGuides { get; }

		public GenericSetting<bool> NormalizedOfficialCheckpoints { get; }

		public GenericSetting<float> RacerAnchorX { get; }

		public GenericSetting<float> RacerAnchorY { get; }

		public GenericSetting<HorizontalAlignment> RacerHAlignment { get; }

		public GenericSetting<VerticalAlignment> RacerVAlignment { get; }

		public KeyBindingSetting ToggleDebug { get; }

		public RacingSettings(SettingCollection settings)
		{
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Expected O, but got Unknown
			//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f4: Expected O, but got Unknown
			MumblePollingRate = _dc.Add(settings.Slider("MumblePollingRate", 50, 25, 200, () => Strings.SettingMumblePollingRate, () => Strings.SettingMumblePollingRateText));
			SpeedometerAnchorY = _dc.Add(settings.PercentageSlider("SpeedometerAnchorY", 0.7f, 0f, 1f, () => Strings.SettingSpeedometerAnchorY, () => Strings.SettingSpeedometerAnchorYText));
			ShowSpeedometer = _dc.Add(settings.Generic("ShowSpeedometer", defaultValue: true, () => Strings.SettingShowSpeedometer, () => Strings.SettingShowSpeedometerText));
			ToggleSpeedometer = _dc.Add(settings.KeyBinding("ToggleSpeedometer", new KeyBinding(), () => Strings.SettingToggleSpeedometer, () => Strings.SettingToggleSpeedometerText));
			ToggleSpeedometer.OnActivated(delegate
			{
				ShowSpeedometer.Value = !ShowSpeedometer.Value;
			});
			Meters = new Dictionary<MountType, (GenericSetting<bool>, RectAnchor)>
			{
				[(MountType)4] = (_dc.Add(settings.Generic("SkimmerMeter", defaultValue: true, () => Strings.SettingSkimmerMeter)), SkimmerMeter.Construct()),
				[(MountType)2] = (_dc.Add(settings.Generic("GriffonMeter", defaultValue: true, () => Strings.SettingGriffonMeter)), GriffonMeter.Construct()),
				[(MountType)6] = (_dc.Add(settings.Generic("RollerBeetleMeter", defaultValue: true, () => Strings.SettingRollerBeetleMeter)), BeetleMeter.Construct()),
				[(MountType)9] = (_dc.Add(settings.Generic("SkiffMeter", defaultValue: true, () => Strings.SettingSkiffMeter)), SkiffMeter.Construct())
			};
			SfxVolumeMultiplier = _dc.Add(settings.PercentageSlider("SFXVolumeMultiplier", 1.5f, 0f, 3f, () => Strings.SettingSFXVolumeMultiplier, () => Strings.SettingSFXVolumeMultiplierText));
			MaxGhostData = _dc.Add(settings.Slider("MaxGhostData", 3, 0, 10, () => Strings.SettingMaxGhostData, () => Strings.SettingMaxGhostDataText));
			AutoLocalGhost = _dc.Add(settings.Generic("AutoLocalGhost", defaultValue: true, () => Strings.SettingAutoLocalGhost, () => Strings.SettingAutoLocalGhostText));
			ShownCheckpoints = _dc.Add(settings.Slider("ShownCheckpoints", 3, 0, 10, () => Strings.SettingShownCheckpoints, () => Strings.SettingShownCheckpointsText));
			ShowGuides = _dc.Add(settings.Generic("ShowGuides", defaultValue: true, () => Strings.SettingShowGuides, () => Strings.SettingShowGuidesText));
			NormalizedOfficialCheckpoints = _dc.Add(settings.Generic("NormalizedOfficialCheckpoints", defaultValue: true, () => Strings.SettingNormalizedOfficialCheckpoints, () => Strings.SettingNormalizedOfficialCheckpointsText));
			RacerAnchorX = _dc.Add(settings.PercentageSlider("RacerAnchorX", 0.5f, 0f, 1f, () => Strings.SettingRacerAnchorX, () => Strings.SettingRacerAnchorXText));
			RacerAnchorY = _dc.Add(settings.PercentageSlider("RacerAnchorY", 0.7f, 0f, 1f, () => Strings.SettingRacerAnchorY, () => Strings.SettingRacerAnchorYText));
			RacerHAlignment = _dc.Add(settings.Generic("RacerHAlignment", HorizontalAlignment.Center, () => Strings.SettingRacerHAlignment, () => Strings.SettingRacerHAlignmentText));
			RacerVAlignment = _dc.Add(settings.Generic("RacerVAlignment", VerticalAlignment.TopDown, () => Strings.SettingRacerVAlignment, () => Strings.SettingRacerVAlignmentText));
			ToggleDebug = _dc.Add(settings.KeyBinding("ToggleDebug", new KeyBinding(), () => Strings.SettingToggleDebug, () => Strings.SettingToggleDebugText));
		}

		public static float AlignmentPercentage(HorizontalAlignment h)
		{
			return h switch
			{
				HorizontalAlignment.LeftToRight => 0f, 
				HorizontalAlignment.RightToLeft => 1f, 
				_ => 0.5f, 
			};
		}

		public static float AlignmentPercentage(VerticalAlignment h)
		{
			return h switch
			{
				VerticalAlignment.TopDown => 0f, 
				VerticalAlignment.BottomUp => 1f, 
				_ => 0.5f, 
			};
		}

		public void Dispose()
		{
			_dc.Dispose();
		}
	}
}
