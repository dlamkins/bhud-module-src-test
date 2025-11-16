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

		private Label _OpacityLabelDisplay;

		private SettingEntry<int> _LowTimerSettingEntry;

		private SettingEntry<float> _OpacityDefaultSettingEntry;

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
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Expected O, but got Unknown
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Expected O, but got Unknown
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
			((Control)val2).set_Size(new Point(500, 300));
			_settingsViewContainer = val2;
			SettingsView settingsView = new SettingsView(mainSettingsCollection, -1);
			_settingsViewContainer.Show((IView)(object)settingsView);
			Label val3 = new Label();
			((Control)val3).set_Size(new Point(100, 40));
			((Control)val3).set_Location(new Point(580, 150));
			val3.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val3).set_Parent((Container)(object)listSettingsPanel);
			_LowTimerLabelDisplay = val3;
			Label val4 = new Label();
			((Control)val4).set_Size(new Point(100, 40));
			((Control)val4).set_Location(new Point(580, 175));
			val4.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val4).set_Parent((Container)(object)listSettingsPanel);
			_OpacityLabelDisplay = val4;
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
				_OpacityDefaultSettingEntry = null;
				TimerCollector.TryGetSetting<float>("OpacityDefault", ref _OpacityDefaultSettingEntry);
				if (_OpacityDefaultSettingEntry != null)
				{
					_OpacityDefaultSettingEntry.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OpacityDefaultSettingEntry_SettingChanged);
					_OpacityLabelDisplay.set_Text(Math.Round(_OpacityDefaultSettingEntry.get_Value() * 100f, 0) + "%");
				}
			}
			AsyncTexture2D TitleTexture = AsyncTexture2D.FromAssetId(1234872);
			Panel val5 = new Panel();
			((Control)val5).set_Parent((Container)(object)listSettingsPanel);
			((Control)val5).set_Size(new Point(700, 40));
			((Control)val5).set_Location(new Point(102, 60));
			val5.set_BackgroundTexture(TitleTexture);
			_timerEventsTitlePanel = val5;
			Label val6 = new Label();
			val6.set_Text("General Settings");
			((Control)val6).set_Size(new Point(300, 40));
			((Control)val6).set_Location(new Point(10, 0));
			val6.set_Font(GameService.Content.get_DefaultFont16());
			val6.set_TextColor(Color.get_White());
			((Control)val6).set_Parent((Container)(object)_timerEventsTitlePanel);
			_timerEventsTitleLabel = val6;
		}

		private void OpacityDefaultSettingEntry_SettingChanged(object sender, ValueChangedEventArgs<float> e)
		{
			_OpacityLabelDisplay.set_Text(Math.Round(_OpacityDefaultSettingEntry.get_Value() * 100f, 0) + "%");
		}

		private void LowTimerSettingEntry_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			_LowTimerLabelDisplay.set_Text(_LowTimerSettingEntry.get_Value() + " seconds");
		}

		public ListSettingsTabView()
			: this()
		{
		}
	}
}
