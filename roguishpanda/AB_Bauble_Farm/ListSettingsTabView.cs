using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace roguishpanda.AB_Bauble_Farm
{
	public class ListSettingsTabView : View
	{
		private MainWindowModule _BaubleFarmModule;

		private ViewContainer _settingsViewContainer;

		private Panel _timerEventsTitlePanel;

		private Label _timerEventsTitleLabel;

		private Label _LowTimerLabelDisplay;

		private Label _IntermediateLowTimerLabelDisplay;

		private Label _OpacityLabelDisplay;

		private Label _TTSVolumeLabelDisplay;

		private Label _TTSSpeedLabelDisplay;

		private SettingEntry<int> _LowTimerSettingEntry;

		private SettingEntry<int> _IntermediateLowTimerSettingEntry;

		private SettingEntry<float> _OpacityDefaultSettingEntry;

		private SettingEntry<int> _TTSVolumeSettingEntry;

		private SettingEntry<int> _TTSSpeedSettingEntry;

		private SettingCollection _Settings;

		protected override void Build(Container buildPanel)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Expected O, but got Unknown
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Expected O, but got Unknown
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Expected O, but got Unknown
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Expected O, but got Unknown
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Expected O, but got Unknown
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0490: Unknown result type (might be due to invalid IL or missing references)
			//IL_0497: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Expected O, but got Unknown
			//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Expected O, but got Unknown
			_BaubleFarmModule = MainWindowModule.ModuleInstance;
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			int num = ((Rectangle)(ref contentRegion)).get_Size().X + 200;
			contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(new Point(num, ((Rectangle)(ref contentRegion)).get_Size().Y + 300));
			contentRegion = buildPanel.get_ContentRegion();
			int x = ((Rectangle)(ref contentRegion)).get_Location().X;
			contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Location(new Point(x, ((Rectangle)(ref contentRegion)).get_Location().Y - 35));
			val.set_BackgroundTexture(MainWindowModule.ModuleInstance._asyncTimertexture);
			Panel listSettingsPanel = val;
			SettingCollection mainSettingsCollection = _BaubleFarmModule._MainSettingsCollection;
			ViewContainer val2 = new ViewContainer();
			((Control)val2).set_Parent((Container)(object)listSettingsPanel);
			((Control)val2).set_Padding(new Thickness(50f, 50f, 50f, 50f));
			((Control)val2).set_Location(new Point(100, 100));
			((Control)val2).set_Size(new Point(500, 600));
			_settingsViewContainer = val2;
			SettingsView settingsView = new SettingsView(mainSettingsCollection, -1);
			_settingsViewContainer.Show((IView)(object)settingsView);
			Label val3 = new Label();
			((Control)val3).set_Size(new Point(100, 40));
			((Control)val3).set_Location(new Point(580, 170));
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Parent((Container)(object)listSettingsPanel);
			_LowTimerLabelDisplay = val3;
			Label val4 = new Label();
			((Control)val4).set_Size(new Point(100, 40));
			((Control)val4).set_Location(new Point(580, 195));
			val4.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val4).set_Parent((Container)(object)listSettingsPanel);
			_IntermediateLowTimerLabelDisplay = val4;
			Label val5 = new Label();
			((Control)val5).set_Size(new Point(100, 40));
			((Control)val5).set_Location(new Point(580, 220));
			val5.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val5).set_Parent((Container)(object)listSettingsPanel);
			_OpacityLabelDisplay = val5;
			Label val6 = new Label();
			((Control)val6).set_Size(new Point(100, 40));
			((Control)val6).set_Location(new Point(580, 245));
			val6.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val6).set_Parent((Container)(object)listSettingsPanel);
			_TTSVolumeLabelDisplay = val6;
			Label val7 = new Label();
			((Control)val7).set_Size(new Point(100, 40));
			((Control)val7).set_Location(new Point(580, 270));
			val7.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val7).set_Parent((Container)(object)listSettingsPanel);
			_TTSSpeedLabelDisplay = val7;
			_Settings = _BaubleFarmModule._settings;
			SettingCollection TimerCollector = _Settings.AddSubCollection("MainSettings", false);
			if (TimerCollector != null)
			{
				_LowTimerSettingEntry = null;
				TimerCollector.TryGetSetting<int>("LowTimerDefaultTimer", ref _LowTimerSettingEntry);
				if (_LowTimerSettingEntry != null)
				{
					_LowTimerSettingEntry.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)LowTimerSettingEntry_SettingChanged);
					_LowTimerLabelDisplay.set_Text(_LowTimerSettingEntry.get_Value() + " seconds");
				}
				_IntermediateLowTimerSettingEntry = null;
				TimerCollector.TryGetSetting<int>("IntermediateLowTimerDefaultTimer", ref _IntermediateLowTimerSettingEntry);
				if (_IntermediateLowTimerSettingEntry != null)
				{
					_IntermediateLowTimerSettingEntry.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)IntermediateLowTimerSettingEntry_SettingChanged);
					_IntermediateLowTimerLabelDisplay.set_Text(_IntermediateLowTimerSettingEntry.get_Value() + " seconds");
				}
				_OpacityDefaultSettingEntry = null;
				TimerCollector.TryGetSetting<float>("OpacityDefault", ref _OpacityDefaultSettingEntry);
				if (_OpacityDefaultSettingEntry != null)
				{
					_OpacityDefaultSettingEntry.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OpacityDefaultSettingEntry_SettingChanged);
					_OpacityLabelDisplay.set_Text(Math.Round(_OpacityDefaultSettingEntry.get_Value() * 100f, 0) + "%");
				}
				_TTSVolumeSettingEntry = null;
				TimerCollector.TryGetSetting<int>("TTSVolumeDefaultTimer", ref _TTSVolumeSettingEntry);
				if (_TTSVolumeSettingEntry != null)
				{
					_TTSVolumeSettingEntry.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)TTSVolumeDefaultSettingEntry_SettingChanged);
					_TTSVolumeLabelDisplay.set_Text(_TTSVolumeSettingEntry.get_Value() + "%");
				}
				_TTSSpeedSettingEntry = null;
				TimerCollector.TryGetSetting<int>("TTSSpeedDefaultTimer", ref _TTSSpeedSettingEntry);
				if (_TTSSpeedSettingEntry != null)
				{
					_TTSSpeedSettingEntry.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)TTSSpeedDefaultSettingEntry_SettingChanged);
					_TTSSpeedLabelDisplay.set_Text(_TTSSpeedSettingEntry.get_Value().ToString());
				}
			}
			AsyncTexture2D TitleTexture = AsyncTexture2D.FromAssetId(1234872);
			Panel val8 = new Panel();
			((Control)val8).set_Parent((Container)(object)listSettingsPanel);
			((Control)val8).set_Size(new Point(700, 40));
			((Control)val8).set_Location(new Point(102, 60));
			val8.set_BackgroundTexture(TitleTexture);
			_timerEventsTitlePanel = val8;
			Label val9 = new Label();
			val9.set_Text("General Settings");
			((Control)val9).set_Size(new Point(300, 40));
			((Control)val9).set_Location(new Point(10, 0));
			val9.set_Font(GameService.Content.get_DefaultFont16());
			val9.set_TextColor(Color.get_White());
			((Control)val9).set_Parent((Container)(object)_timerEventsTitlePanel);
			_timerEventsTitleLabel = val9;
		}

		private void TTSVolumeDefaultSettingEntry_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			_TTSVolumeLabelDisplay.set_Text(_TTSVolumeSettingEntry.get_Value() + "%");
		}

		private void TTSSpeedDefaultSettingEntry_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			_TTSSpeedLabelDisplay.set_Text(_TTSSpeedSettingEntry.get_Value().ToString());
		}

		private void OpacityDefaultSettingEntry_SettingChanged(object sender, ValueChangedEventArgs<float> e)
		{
			_OpacityLabelDisplay.set_Text(Math.Round(_OpacityDefaultSettingEntry.get_Value() * 100f, 0) + "%");
		}

		private void LowTimerSettingEntry_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			_LowTimerLabelDisplay.set_Text(_LowTimerSettingEntry.get_Value() + " seconds");
		}

		private void IntermediateLowTimerSettingEntry_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			_IntermediateLowTimerLabelDisplay.set_Text(_IntermediateLowTimerSettingEntry.get_Value() + " seconds");
		}

		public ListSettingsTabView()
			: this()
		{
		}
	}
}
