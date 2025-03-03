using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Microsoft.Xna.Framework.Input;

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

		public GenericSetting<float> SpeedometerAnchorX { get; }

		public GenericSetting<float> SpeedometerAnchorY { get; }

		public GenericSetting<bool> ShowSpeedometer { get; }

		private KeyBindingSetting ToggleSpeedometer { get; }

		public IReadOnlyDictionary<MountType, GenericSetting<bool>> Meters { get; }

		private KeyBindingSetting BeetleDriftKey { get; }

		public GenericSetting<bool> EnableSimpleSpeedometer { get; }

		public GenericSetting<float> SimpleSpeedometerAnchorX { get; }

		public GenericSetting<float> SimpleSpeedometerAnchorY { get; }

		public GenericSetting<float> SfxVolumeMultiplier { get; }

		public GenericSetting<int> MaxGhostData { get; }

		public GenericSetting<bool> AutoLocalGhost { get; }

		public GenericSetting<int> ShownCheckpoints { get; }

		public GenericSetting<float> CheckpointLineThickness { get; }

		public GenericSetting<float> CheckpointAlpha { get; }

		public GenericSetting<bool> CheckpointArrow { get; }

		public GenericSetting<bool> ShowGuides { get; }

		public GenericSetting<bool> NormalizedOfficialCheckpoints { get; }

		public GenericSetting<float> RacerAnchorX { get; }

		public GenericSetting<float> RacerAnchorY { get; }

		public GenericSetting<HorizontalAlignment> RacerHAlignment { get; }

		public GenericSetting<VerticalAlignment> RacerVAlignment { get; }

		public KeyBindingSetting EditorUndo { get; }

		public KeyBindingSetting EditorRedo { get; }

		public KeyBindingSetting EditorSave { get; }

		public KeyBindingSetting EditorInsertPointAfter { get; }

		public GenericSetting<string> OnlineUrl { get; }

		public GenericSetting<string> OnlineDefaultNickname { get; }

		public GenericSetting<bool> OnlineMarkRacers { get; }

		public KeyBindingSetting ToggleDebug { get; }

		public bool? IsDriftKeyDown()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if ((int)BeetleDriftKey.Value.get_PrimaryKey() == 0)
			{
				return null;
			}
			KeyboardState state = GameService.Input.get_Keyboard().get_State();
			return ((KeyboardState)(ref state)).IsKeyDown(BeetleDriftKey.Value.get_PrimaryKey());
		}

		public RacingSettings(SettingCollection settings)
		{
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Expected O, but got Unknown
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Expected O, but got Unknown
			//IL_0a37: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7f: Expected O, but got Unknown
			//IL_0a99: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae1: Expected O, but got Unknown
			//IL_0afb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b43: Expected O, but got Unknown
			//IL_0b5a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba2: Expected O, but got Unknown
			//IL_0cd2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d1a: Expected O, but got Unknown
			MumblePollingRate = _dc.Add(settings.Slider("MumblePollingRate", 50, 25, 200, () => Strings.SettingMumblePollingRate, () => Strings.SettingMumblePollingRateText));
			SpeedometerAnchorX = _dc.Add(settings.PercentageSlider("SpeedometerAnchorX", 0.5f, 0f, 1f, () => Strings.SettingSpeedometerAnchorX, () => Strings.SettingSpeedometerAnchorXText));
			SpeedometerAnchorY = _dc.Add(settings.PercentageSlider("SpeedometerAnchorY", 0.7f, 0f, 1f, () => Strings.SettingSpeedometerAnchorY, () => Strings.SettingSpeedometerAnchorYText));
			ShowSpeedometer = _dc.Add(settings.Generic("ShowSpeedometer", defaultValue: true, () => Strings.SettingShowSpeedometer, () => Strings.SettingShowSpeedometerText));
			ToggleSpeedometer = _dc.Add(settings.KeyBinding("ToggleSpeedometer", new KeyBinding(), () => Strings.SettingToggleSpeedometer, () => Strings.SettingToggleSpeedometerText));
			_dc.Add(ToggleSpeedometer.OnActivated(delegate
			{
				ShowSpeedometer.Value = !ShowSpeedometer.Value;
			}));
			Meters = new Dictionary<MountType, GenericSetting<bool>>
			{
				[(MountType)4] = _dc.Add(settings.Generic("SkimmerMeter", defaultValue: true, () => Strings.SettingSkimmerMeter)),
				[(MountType)2] = _dc.Add(settings.Generic("GriffonMeter", defaultValue: true, () => Strings.SettingGriffonMeter)),
				[(MountType)6] = _dc.Add(settings.Generic("RollerBeetleMeter", defaultValue: true, () => Strings.SettingRollerBeetleMeter)),
				[(MountType)7] = _dc.Add(settings.Generic("WarclawMeter", defaultValue: true, () => Strings.SettingWarclawMeter)),
				[(MountType)9] = _dc.Add(settings.Generic("SkiffMeter", defaultValue: true, () => Strings.SettingSkiffMeter)),
				[(MountType)10] = _dc.Add(settings.Generic("SiegeTurtleMeter", defaultValue: true, () => Strings.SettingSiegeTurtleMeter))
			};
			BeetleDriftKey = _dc.Add(settings.KeyBinding("BeetleDriftKey", new KeyBinding(), () => Strings.SettingBeetleDriftKey, () => Strings.SettingBeetleDriftKeyText));
			EnableSimpleSpeedometer = _dc.Add(settings.Generic("EnableSimpleSpeedometer", defaultValue: false, () => Strings.SettingEnableSimpleSpeedometer, () => Strings.SettingEnableSimpleSpeedometerText));
			SimpleSpeedometerAnchorX = _dc.Add(settings.PercentageSlider("SimpleSpeedometerAnchorX", 0.5f, 0f, 1f, () => Strings.SettingSimpleSpeedometerAnchorX, () => Strings.SettingSimpleSpeedometerAnchorXText));
			SimpleSpeedometerAnchorY = _dc.Add(settings.PercentageSlider("SimpleSpeedometerAnchorY", 0.8f, 0f, 1f, () => Strings.SettingSimpleSpeedometerAnchorY, () => Strings.SettingSimpleSpeedometerAnchorYText));
			SfxVolumeMultiplier = _dc.Add(settings.PercentageSlider("SFXVolumeMultiplier", 1.5f, 0f, 3f, () => Strings.SettingSFXVolumeMultiplier, () => Strings.SettingSFXVolumeMultiplierText));
			MaxGhostData = _dc.Add(settings.Slider("MaxGhostData", 3, 0, 10, () => Strings.SettingMaxGhostData, () => Strings.SettingMaxGhostDataText));
			AutoLocalGhost = _dc.Add(settings.Generic("AutoLocalGhost", defaultValue: true, () => Strings.SettingAutoLocalGhost, () => Strings.SettingAutoLocalGhostText));
			ShownCheckpoints = _dc.Add(settings.Slider("ShownCheckpoints", 3, -1, 10, () => Strings.SettingShownCheckpoints, () => Strings.SettingShownCheckpointsText));
			CheckpointLineThickness = _dc.Add(settings.PercentageSlider("CheckpointLineThickness", 1.5f, 0f, 3f, () => Strings.SettingCheckpointLineThickness, () => Strings.SettingCheckpointLineThicknessText));
			CheckpointAlpha = _dc.Add(settings.PercentageSlider("CheckpointAlpha", 1f, 0f, 1f, () => Strings.SettingCheckpointAlpha, () => Strings.SettingCheckpointAlphaText));
			CheckpointArrow = _dc.Add(settings.Generic("CheckpointArrow", defaultValue: true, () => Strings.SettingCheckpointArrow, () => Strings.SettingCheckpointArrowText));
			ShowGuides = _dc.Add(settings.Generic("ShowGuides", defaultValue: true, () => Strings.SettingShowGuides, () => Strings.SettingShowGuidesText));
			NormalizedOfficialCheckpoints = _dc.Add(settings.Generic("NormalizedOfficialCheckpoints", defaultValue: true, () => Strings.SettingNormalizedOfficialCheckpoints, () => Strings.SettingNormalizedOfficialCheckpointsText));
			RacerAnchorX = _dc.Add(settings.PercentageSlider("RacerAnchorX", 0.5f, 0f, 1f, () => Strings.SettingRacerAnchorX, () => Strings.SettingRacerAnchorXText));
			RacerAnchorY = _dc.Add(settings.PercentageSlider("RacerAnchorY", 0.7f, 0f, 1f, () => Strings.SettingRacerAnchorY, () => Strings.SettingRacerAnchorYText));
			RacerHAlignment = _dc.Add(settings.Generic("RacerHAlignment", HorizontalAlignment.Center, () => Strings.SettingRacerHAlignment, () => Strings.SettingRacerHAlignmentText));
			RacerVAlignment = _dc.Add(settings.Generic("RacerVAlignment", VerticalAlignment.TopDown, () => Strings.SettingRacerVAlignment, () => Strings.SettingRacerVAlignmentText));
			EditorUndo = _dc.Add(settings.KeyBinding("EditorUndo", new KeyBinding((ModifierKeys)1, (Keys)90), () => Strings.SettingEditorUndo, () => Strings.SettingEditorUndoText));
			EditorRedo = _dc.Add(settings.KeyBinding("EditorRedo", new KeyBinding((ModifierKeys)1, (Keys)89), () => Strings.SettingEditorRedo, () => Strings.SettingEditorRedoText));
			EditorSave = _dc.Add(settings.KeyBinding("EditorSave", new KeyBinding((ModifierKeys)1, (Keys)83), () => Strings.SettingEditorSave, () => Strings.SettingEditorSaveText));
			EditorInsertPointAfter = _dc.Add(settings.KeyBinding("EditorInsertPointAfter", new KeyBinding(), () => Strings.SettingEditorInsertPointAfter, () => Strings.SettingEditorInsertPointAfterText));
			OnlineUrl = _dc.Add(settings.Generic("OnlineUrl", "", () => Strings.SettingOnlineUrl, () => Strings.SettingOnlineUrlText));
			OnlineDefaultNickname = _dc.Add(settings.Generic("OnlineDefaultNickname", "", () => Strings.SettingOnlineDefaultNickname, () => Strings.SettingOnlineDefaultNicknameText));
			OnlineMarkRacers = _dc.Add(settings.Generic("OnlineMarkRacers", defaultValue: false, () => Strings.SettingOnlineMarkRacers, () => Strings.SettingOnlineMarkRacersText));
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
