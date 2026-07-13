using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Windows.Globalization;
using Windows.Media.Ocr;

namespace Frtal.LorebookReader
{
	public class LorebookSettingsView : View
	{
		private const string AutoVoiceItem = "(auto — match OCR language)";

		private const string EngineWindows = "Windows voices (offline)";

		private const string EngineEdge = "Edge neural voices (online)";

		private readonly LorebookReaderModule _module;

		private Label _winVoiceLabel;

		private Dropdown _winVoiceDropdown;

		private Label _edgeVoiceLabel;

		private Dropdown _edgeVoiceDropdown;

		private static readonly string[] _translateModes = new string[3] { "Off", "Subtitles only", "Subtitles + speech" };

		private static readonly string[] _sizeItems = new string[4] { "Small (18)", "Medium (24)", "Large (32)", "Huge (36)" };

		public LorebookSettingsView(LorebookReaderModule module)
			: this()
		{
			_module = module;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Expected O, but got Unknown
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Expected O, but got Unknown
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Expected O, but got Unknown
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Expected O, but got Unknown
			//IL_042f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0459: Expected O, but got Unknown
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_046a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Expected O, but got Unknown
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_054e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_0567: Unknown result type (might be due to invalid IL or missing references)
			//IL_0573: Expected O, but got Unknown
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Expected O, but got Unknown
			//IL_061b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0620: Unknown result type (might be due to invalid IL or missing references)
			//IL_063b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0642: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Unknown result type (might be due to invalid IL or missing references)
			//IL_0655: Expected O, but got Unknown
			//IL_0656: Unknown result type (might be due to invalid IL or missing references)
			//IL_065b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0666: Unknown result type (might be due to invalid IL or missing references)
			//IL_0671: Unknown result type (might be due to invalid IL or missing references)
			//IL_068d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0698: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a4: Expected O, but got Unknown
			//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fc: Expected O, but got Unknown
			//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fb: Expected O, but got Unknown
			//IL_0813: Unknown result type (might be due to invalid IL or missing references)
			//IL_0818: Unknown result type (might be due to invalid IL or missing references)
			//IL_0833: Unknown result type (might be due to invalid IL or missing references)
			//IL_083a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0841: Unknown result type (might be due to invalid IL or missing references)
			//IL_084d: Expected O, but got Unknown
			//IL_084e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0853: Unknown result type (might be due to invalid IL or missing references)
			//IL_085e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0869: Unknown result type (might be due to invalid IL or missing references)
			//IL_0885: Unknown result type (might be due to invalid IL or missing references)
			//IL_0890: Unknown result type (might be due to invalid IL or missing references)
			//IL_089c: Expected O, but got Unknown
			//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f3: Expected O, but got Unknown
			//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0904: Unknown result type (might be due to invalid IL or missing references)
			//IL_090f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0925: Unknown result type (might be due to invalid IL or missing references)
			//IL_0930: Unknown result type (might be due to invalid IL or missing references)
			//IL_093c: Expected O, but got Unknown
			//IL_0954: Unknown result type (might be due to invalid IL or missing references)
			//IL_0959: Unknown result type (might be due to invalid IL or missing references)
			//IL_0979: Unknown result type (might be due to invalid IL or missing references)
			//IL_0980: Unknown result type (might be due to invalid IL or missing references)
			//IL_0987: Unknown result type (might be due to invalid IL or missing references)
			//IL_0993: Expected O, but got Unknown
			//IL_0994: Unknown result type (might be due to invalid IL or missing references)
			//IL_0999: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_09af: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09dc: Expected O, but got Unknown
			//IL_09f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a1d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a28: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a34: Expected O, but got Unknown
			//IL_0a9f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac7: Expected O, but got Unknown
			//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b49: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b59: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b60: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b73: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b8a: Expected O, but got Unknown
			//IL_0bf4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c0b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c19: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c29: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c35: Expected O, but got Unknown
			//IL_0ca8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cbf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cdc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d1f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d26: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d2d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d39: Expected O, but got Unknown
			//IL_0d3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d55: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d6c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d83: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 10f));
			val.set_OuterControlPadding(new Vector2(12f, 12f));
			FlowPanel panel = val;
			KeybindingAssigner val2 = new KeybindingAssigner(_module.ReadKeybindSetting.get_Value());
			val2.set_KeyBindingName("Read lorebook");
			((Control)val2).set_Parent((Container)(object)panel);
			KeybindingAssigner val3 = new KeybindingAssigner(_module.StopKeybindSetting.get_Value());
			val3.set_KeyBindingName("Stop reading");
			((Control)val3).set_Parent((Container)(object)panel);
			Checkbox val4 = new Checkbox();
			val4.set_Text("Show speaker icon on open books");
			val4.set_Checked(_module.ShowSpeakerButtonSetting.get_Value());
			((Control)val4).set_Parent((Container)(object)panel);
			val4.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.ShowSpeakerButtonSetting.set_Value(e.get_Checked());
			});
			Checkbox val5 = new Checkbox();
			val5.set_Text("Conversation capture mode (also detect NPC dialogues)");
			val5.set_Checked(_module.ConversationCaptureSetting.get_Value());
			((Control)val5).set_Parent((Container)(object)panel);
			Checkbox convCheckbox = val5;
			convCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.ConversationCaptureSetting.set_Value(e.get_Checked());
			});
			_module.ConversationCaptureSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				convCheckbox.set_Checked(e.get_NewValue());
			});
			KeybindingAssigner val6 = new KeybindingAssigner(_module.ConvToggleKeybindSetting.get_Value());
			val6.set_KeyBindingName("Toggle conversation capture");
			((Control)val6).set_Parent((Container)(object)panel);
			Label val7 = new Label();
			val7.set_Text((_module.DialogZoneSetting.get_Value().Length > 0) ? "Dialogue zone: calibrated" : "Dialogue zone: not calibrated");
			val7.set_AutoSizeWidth(true);
			val7.set_AutoSizeHeight(true);
			((Control)val7).set_Parent((Container)(object)panel);
			Label calibStatus = val7;
			KeybindingAssigner val8 = new KeybindingAssigner(_module.CalibrateKeybindSetting.get_Value());
			val8.set_KeyBindingName("Calibrate dialogue zone");
			((Control)val8).set_Parent((Container)(object)panel);
			StandardButton val9 = new StandardButton();
			val9.set_Text("Calibrate dialogue zone (drag a frame)");
			((Control)val9).set_Width(360);
			((Control)val9).set_Parent((Container)(object)panel);
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.StartCalibration();
			});
			StandardButton val10 = new StandardButton();
			val10.set_Text("Clear calibration (use auto-detect)");
			((Control)val10).set_Width(360);
			((Control)val10).set_Parent((Container)(object)panel);
			((Control)val10).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.DialogZoneSetting.set_Value("");
			});
			_module.DialogZoneSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				calibStatus.set_Text((!string.IsNullOrEmpty(e.get_NewValue())) ? "Dialogue zone: calibrated" : "Dialogue zone: not calibrated");
			});
			Label val11 = new Label();
			val11.set_Text((_module.BookZoneSetting.get_Value().Length > 0) ? "Lorebook OCR area: calibrated" : "Lorebook OCR area: auto (default)");
			val11.set_AutoSizeWidth(true);
			val11.set_AutoSizeHeight(true);
			((Control)val11).set_Parent((Container)(object)panel);
			Label bookCalibStatus = val11;
			KeybindingAssigner val12 = new KeybindingAssigner(_module.BookCalibrateKeybindSetting.get_Value());
			val12.set_KeyBindingName("Calibrate lorebook OCR area");
			((Control)val12).set_Parent((Container)(object)panel);
			StandardButton val13 = new StandardButton();
			val13.set_Text("Calibrate lorebook OCR area (open a book, drag a frame)");
			((Control)val13).set_Width(360);
			((Control)val13).set_Parent((Container)(object)panel);
			((Control)val13).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.StartBookCalibration();
			});
			StandardButton val14 = new StandardButton();
			val14.set_Text("Clear lorebook OCR area (use auto-detect)");
			((Control)val14).set_Width(360);
			((Control)val14).set_Parent((Container)(object)panel);
			((Control)val14).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.BookZoneSetting.set_Value("");
			});
			_module.BookZoneSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				bookCalibStatus.set_Text((!string.IsNullOrEmpty(e.get_NewValue())) ? "Lorebook OCR area: calibrated" : "Lorebook OCR area: auto (default)");
			});
			KeybindingAssigner val15 = new KeybindingAssigner(_module.DebugDumpKeybindSetting.get_Value());
			val15.set_KeyBindingName("Save debug capture");
			((Control)val15).set_Parent((Container)(object)panel);
			Label val16 = new Label();
			val16.set_Text("Voice engine");
			val16.set_AutoSizeWidth(true);
			val16.set_AutoSizeHeight(true);
			((Control)val16).set_Parent((Container)(object)panel);
			Dropdown val17 = new Dropdown();
			((Control)val17).set_Width(360);
			((Control)val17).set_Parent((Container)(object)panel);
			Dropdown engineDropdown = val17;
			engineDropdown.get_Items().Add("Windows voices (offline)");
			engineDropdown.get_Items().Add("Edge neural voices (online)");
			engineDropdown.set_SelectedItem((_module.VoiceEngineSetting.get_Value() == "edge") ? "Edge neural voices (online)" : "Windows voices (offline)");
			engineDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_module.VoiceEngineSetting.set_Value((engineDropdown.get_SelectedItem() == "Edge neural voices (online)") ? "edge" : "windows");
				UpdateEngineVisibility();
			});
			Label val18 = new Label();
			val18.set_Text("Windows voice");
			val18.set_AutoSizeWidth(true);
			val18.set_AutoSizeHeight(true);
			((Control)val18).set_Parent((Container)(object)panel);
			_winVoiceLabel = val18;
			Dropdown val19 = new Dropdown();
			((Control)val19).set_Width(360);
			((Control)val19).set_Parent((Container)(object)panel);
			_winVoiceDropdown = val19;
			_winVoiceDropdown.get_Items().Add("(auto — match OCR language)");
			foreach (var (name2, lang2) in TtsService.InstalledVoices())
			{
				_winVoiceDropdown.get_Items().Add(name2 + "  [" + lang2 + "]");
			}
			string savedVoice = _module.VoiceNameSetting.get_Value() ?? "";
			_winVoiceDropdown.set_SelectedItem(_winVoiceDropdown.get_Items().FirstOrDefault((string i) => savedVoice.Length > 0 && i.StartsWith(savedVoice, StringComparison.OrdinalIgnoreCase)) ?? "(auto — match OCR language)");
			_winVoiceDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				string selectedItem = _winVoiceDropdown.get_SelectedItem();
				_module.VoiceNameSetting.set_Value((selectedItem == "(auto — match OCR language)") ? "" : selectedItem.Split(new string[1] { "  [" }, StringSplitOptions.None)[0]);
			});
			Label val20 = new Label();
			val20.set_Text("Edge neural voice (requires internet)");
			val20.set_AutoSizeWidth(true);
			val20.set_AutoSizeHeight(true);
			((Control)val20).set_Parent((Container)(object)panel);
			_edgeVoiceLabel = val20;
			Dropdown val21 = new Dropdown();
			((Control)val21).set_Width(360);
			((Control)val21).set_Parent((Container)(object)panel);
			_edgeVoiceDropdown = val21;
			string[] curatedVoices = EdgeTtsService.CuratedVoices;
			foreach (string v in curatedVoices)
			{
				_edgeVoiceDropdown.get_Items().Add(v);
			}
			_edgeVoiceDropdown.set_SelectedItem(EdgeTtsService.CuratedVoices.Contains(_module.EdgeVoiceSetting.get_Value()) ? _module.EdgeVoiceSetting.get_Value() : EdgeTtsService.CuratedVoices[0]);
			_edgeVoiceDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_module.EdgeVoiceSetting.set_Value(_edgeVoiceDropdown.get_SelectedItem());
			});
			Label val22 = new Label();
			val22.set_Text(RateText(_module.SpeakingRateSetting.get_Value()));
			val22.set_AutoSizeWidth(true);
			val22.set_AutoSizeHeight(true);
			((Control)val22).set_Parent((Container)(object)panel);
			Label rateLabel = val22;
			TrackBar val23 = new TrackBar();
			val23.set_MinValue(50f);
			val23.set_MaxValue(200f);
			val23.set_Value(_module.SpeakingRateSetting.get_Value() * 100f);
			((Control)val23).set_Width(360);
			((Control)val23).set_Parent((Container)(object)panel);
			TrackBar rateBar = val23;
			rateBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num5 = (float)Math.Round(rateBar.get_Value()) / 100f;
				_module.SpeakingRateSetting.set_Value(num5);
				rateLabel.set_Text(RateText(num5));
			});
			Label val24 = new Label();
			val24.set_Text("OCR language (your GW2 client language)");
			val24.set_AutoSizeWidth(true);
			val24.set_AutoSizeHeight(true);
			((Control)val24).set_Parent((Container)(object)panel);
			Dropdown val25 = new Dropdown();
			((Control)val25).set_Width(360);
			((Control)val25).set_Parent((Container)(object)panel);
			Dropdown ocrDropdown = val25;
			foreach (Language lang in OcrEngine.AvailableRecognizerLanguages)
			{
				ocrDropdown.get_Items().Add(lang.LanguageTag + "  (" + lang.DisplayName + ")");
			}
			string savedLang = _module.OcrLanguageSetting.get_Value() ?? "en-US";
			ocrDropdown.set_SelectedItem(ocrDropdown.get_Items().FirstOrDefault((string i) => i.StartsWith(savedLang, StringComparison.OrdinalIgnoreCase)) ?? ocrDropdown.get_Items().FirstOrDefault());
			ocrDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_module.OcrLanguageSetting.set_Value(ocrDropdown.get_SelectedItem().Split(' ')[0]);
			});
			Checkbox val26 = new Checkbox();
			val26.set_Text("Show subtitles while reading");
			val26.set_Checked(_module.ShowSubtitlesSetting.get_Value());
			((Control)val26).set_Parent((Container)(object)panel);
			Checkbox subsCheckbox = val26;
			subsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.ShowSubtitlesSetting.set_Value(e.get_Checked());
			});
			Label val27 = new Label();
			val27.set_Text(OpacityText(_module.SubtitleOpacitySetting.get_Value()));
			val27.set_AutoSizeWidth(true);
			val27.set_AutoSizeHeight(true);
			((Control)val27).set_Parent((Container)(object)panel);
			Label opacityLabel = val27;
			TrackBar val28 = new TrackBar();
			val28.set_MinValue(20f);
			val28.set_MaxValue(100f);
			val28.set_Value(_module.SubtitleOpacitySetting.get_Value() * 100f);
			((Control)val28).set_Width(360);
			((Control)val28).set_Parent((Container)(object)panel);
			TrackBar opacityBar = val28;
			opacityBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num4 = (float)Math.Round(opacityBar.get_Value()) / 100f;
				_module.SubtitleOpacitySetting.set_Value(num4);
				opacityLabel.set_Text(OpacityText(num4));
			});
			Label val29 = new Label();
			val29.set_Text(PosText("X", _module.SubtitleXSetting.get_Value()));
			val29.set_AutoSizeWidth(true);
			val29.set_AutoSizeHeight(true);
			((Control)val29).set_Parent((Container)(object)panel);
			Label posXLabel = val29;
			TrackBar val30 = new TrackBar();
			val30.set_MinValue(0f);
			val30.set_MaxValue(100f);
			val30.set_Value(_module.SubtitleXSetting.get_Value());
			((Control)val30).set_Width(360);
			((Control)val30).set_Parent((Container)(object)panel);
			TrackBar posXBar = val30;
			posXBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num3 = (float)Math.Round(posXBar.get_Value());
				_module.SubtitleXSetting.set_Value(num3);
				posXLabel.set_Text(PosText("X", num3));
			});
			Label val31 = new Label();
			val31.set_Text(PosText("Y", _module.SubtitleYSetting.get_Value()));
			val31.set_AutoSizeWidth(true);
			val31.set_AutoSizeHeight(true);
			((Control)val31).set_Parent((Container)(object)panel);
			Label posYLabel = val31;
			TrackBar val32 = new TrackBar();
			val32.set_MinValue(0f);
			val32.set_MaxValue(100f);
			val32.set_Value(_module.SubtitleYSetting.get_Value());
			((Control)val32).set_Width(360);
			((Control)val32).set_Parent((Container)(object)panel);
			TrackBar posYBar = val32;
			posYBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num2 = (float)Math.Round(posYBar.get_Value());
				_module.SubtitleYSetting.set_Value(num2);
				posYLabel.set_Text(PosText("Y", num2));
			});
			Label val33 = new Label();
			val33.set_Text("Subtitle size");
			val33.set_AutoSizeWidth(true);
			val33.set_AutoSizeHeight(true);
			((Control)val33).set_Parent((Container)(object)panel);
			Dropdown val34 = new Dropdown();
			((Control)val34).set_Width(360);
			((Control)val34).set_Parent((Container)(object)panel);
			Dropdown sizeDropdown = val34;
			curatedVoices = _sizeItems;
			foreach (string item in curatedVoices)
			{
				sizeDropdown.get_Items().Add(item);
			}
			sizeDropdown.set_SelectedItem(SizeToItem(_module.SubtitleFontSizeSetting.get_Value()));
			sizeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_module.SubtitleFontSizeSetting.set_Value(ItemToSize(sizeDropdown.get_SelectedItem()));
			});
			StandardButton val35 = new StandardButton();
			val35.set_Text(EditButtonText());
			((Control)val35).set_Width(360);
			((Control)val35).set_Parent((Container)(object)panel);
			StandardButton editButton = val35;
			((Control)editButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.SubtitleEditMode = !_module.SubtitleEditMode;
				editButton.set_Text(EditButtonText());
			});
			StandardButton val36 = new StandardButton();
			val36.set_Text("Reset subtitles to defaults");
			((Control)val36).set_Width(360);
			((Control)val36).set_Parent((Container)(object)panel);
			((Control)val36).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.SubtitleEditMode = false;
				editButton.set_Text(EditButtonText());
				_module.ShowSubtitlesSetting.set_Value(true);
				_module.SubtitleOpacitySetting.set_Value(0.9f);
				_module.SubtitleXSetting.set_Value(50f);
				_module.SubtitleYSetting.set_Value(82f);
				_module.SubtitleFontSizeSetting.set_Value(24);
				subsCheckbox.set_Checked(true);
				opacityBar.set_Value(90f);
				opacityLabel.set_Text(OpacityText(0.9f));
				posXBar.set_Value(50f);
				posXLabel.set_Text(PosText("X", 50f));
				posYBar.set_Value(82f);
				posYLabel.set_Text(PosText("Y", 82f));
				sizeDropdown.set_SelectedItem(SizeToItem(24));
			});
			_module.SubtitleXSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)delegate(object s, ValueChangedEventArgs<float> e)
			{
				posXBar.set_Value(e.get_NewValue());
				posXLabel.set_Text(PosText("X", e.get_NewValue()));
			});
			_module.SubtitleYSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)delegate(object s, ValueChangedEventArgs<float> e)
			{
				posYBar.set_Value(e.get_NewValue());
				posYLabel.set_Text(PosText("Y", e.get_NewValue()));
			});
			Label val37 = new Label();
			val37.set_Text("Translation");
			val37.set_AutoSizeWidth(true);
			val37.set_AutoSizeHeight(true);
			((Control)val37).set_Parent((Container)(object)panel);
			Dropdown val38 = new Dropdown();
			((Control)val38).set_Width(360);
			((Control)val38).set_Parent((Container)(object)panel);
			Dropdown modeDropdown = val38;
			curatedVoices = _translateModes;
			foreach (string item2 in curatedVoices)
			{
				modeDropdown.get_Items().Add(item2);
			}
			modeDropdown.set_SelectedItem(ModeToItem(_module.TranslateModeSetting.get_Value()));
			modeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_module.TranslateModeSetting.set_Value(ItemToMode(modeDropdown.get_SelectedItem()));
			});
			Label val39 = new Label();
			val39.set_Text("Translate to");
			val39.set_AutoSizeWidth(true);
			val39.set_AutoSizeHeight(true);
			((Control)val39).set_Parent((Container)(object)panel);
			Dropdown val40 = new Dropdown();
			((Control)val40).set_Width(360);
			((Control)val40).set_Parent((Container)(object)panel);
			Dropdown langDropdown = val40;
			(string, string)[] targetLanguages = TranslationService.TargetLanguages;
			for (int j = 0; j < targetLanguages.Length; j++)
			{
				string name = targetLanguages[j].Item2;
				langDropdown.get_Items().Add(name);
			}
			langDropdown.set_SelectedItem(LangCodeToName(_module.TranslateTargetSetting.get_Value()));
			langDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_module.TranslateTargetSetting.set_Value(LangNameToCode(langDropdown.get_SelectedItem()));
			});
			Label val41 = new Label();
			val41.set_Text("Note: translation uses a free online service and may occasionally be unavailable.");
			val41.set_AutoSizeWidth(true);
			val41.set_AutoSizeHeight(true);
			((Control)val41).set_Parent((Container)(object)panel);
			Label val42 = new Label();
			val42.set_Text("Reading history is available via the Lorebook Reader icon in the top-left icon bar.");
			val42.set_AutoSizeWidth(true);
			val42.set_AutoSizeHeight(true);
			((Control)val42).set_Parent((Container)(object)panel);
			Label val43 = new Label();
			val43.set_Text("Catalog size: " + _module.HistoryCapacitySetting.get_Value());
			val43.set_AutoSizeWidth(true);
			val43.set_AutoSizeHeight(true);
			((Control)val43).set_Parent((Container)(object)panel);
			Label capLabel = val43;
			TrackBar val44 = new TrackBar();
			val44.set_MinValue(5f);
			val44.set_MaxValue(100f);
			val44.set_Value((float)_module.HistoryCapacitySetting.get_Value());
			((Control)val44).set_Width(360);
			((Control)val44).set_Parent((Container)(object)panel);
			TrackBar capBar = val44;
			capBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				int num = (int)Math.Round(capBar.get_Value());
				_module.HistoryCapacitySetting.set_Value(num);
				capLabel.set_Text("Catalog size: " + num);
				_module.Catalog?.SetCapacity(num);
			});
			UpdateEngineVisibility();
		}

		private static string ModeToItem(string mode)
		{
			if (mode == "subtitles")
			{
				return _translateModes[1];
			}
			if (mode == "full")
			{
				return _translateModes[2];
			}
			return _translateModes[0];
		}

		private static string ItemToMode(string item)
		{
			if (item == _translateModes[1])
			{
				return "subtitles";
			}
			if (item == _translateModes[2])
			{
				return "full";
			}
			return "off";
		}

		private static string LangCodeToName(string code)
		{
			(string, string)[] targetLanguages = TranslationService.TargetLanguages;
			for (int i = 0; i < targetLanguages.Length; i++)
			{
				var (c, name) = targetLanguages[i];
				if (c == code)
				{
					return name;
				}
			}
			return TranslationService.TargetLanguages[0].Name;
		}

		private static string LangNameToCode(string name)
		{
			(string, string)[] targetLanguages = TranslationService.TargetLanguages;
			for (int i = 0; i < targetLanguages.Length; i++)
			{
				(string, string) tuple = targetLanguages[i];
				var (c, _) = tuple;
				if (tuple.Item2 == name)
				{
					return c;
				}
			}
			return "cs";
		}

		protected override void Unload()
		{
			_module.SubtitleEditMode = false;
			((View<IPresenter>)this).Unload();
		}

		private string EditButtonText()
		{
			if (!_module.SubtitleEditMode)
			{
				return "Edit subtitle position (drag with mouse)";
			}
			return "Done editing position";
		}

		private static string SizeToItem(int size)
		{
			return size switch
			{
				18 => _sizeItems[0], 
				32 => _sizeItems[2], 
				36 => _sizeItems[3], 
				_ => _sizeItems[1], 
			};
		}

		private static int ItemToSize(string item)
		{
			if (item == _sizeItems[0])
			{
				return 18;
			}
			if (item == _sizeItems[2])
			{
				return 32;
			}
			if (item == _sizeItems[3])
			{
				return 36;
			}
			return 24;
		}

		private static string OpacityText(float v)
		{
			return $"Subtitle opacity: {v * 100f:0} %";
		}

		private static string PosText(string axis, float v)
		{
			return $"Subtitle position {axis}: {v:0} % of screen";
		}

		private void UpdateEngineVisibility()
		{
			bool edge = _module.VoiceEngineSetting.get_Value() == "edge";
			((Control)_winVoiceLabel).set_Visible(!edge);
			((Control)_winVoiceDropdown).set_Visible(!edge);
			((Control)_edgeVoiceLabel).set_Visible(edge);
			((Control)_edgeVoiceDropdown).set_Visible(edge);
			Container parent = ((Control)_winVoiceLabel).get_Parent();
			Container obj = ((parent is FlowPanel) ? parent : null);
			if (obj != null)
			{
				((Control)obj).RecalculateLayout();
			}
		}

		private static string RateText(float rate)
		{
			return $"Speaking rate: {rate:0.00}×";
		}
	}
}
