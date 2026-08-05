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
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Expected O, but got Unknown
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Expected O, but got Unknown
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Expected O, but got Unknown
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0406: Expected O, but got Unknown
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Expected O, but got Unknown
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Expected O, but got Unknown
			//IL_0595: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bf: Expected O, but got Unknown
			//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Expected O, but got Unknown
			//IL_0667: Unknown result type (might be due to invalid IL or missing references)
			//IL_066c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0687: Unknown result type (might be due to invalid IL or missing references)
			//IL_068e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0695: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a1: Expected O, but got Unknown
			//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f0: Expected O, but got Unknown
			//IL_0707: Unknown result type (might be due to invalid IL or missing references)
			//IL_070c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0717: Unknown result type (might be due to invalid IL or missing references)
			//IL_071e: Unknown result type (might be due to invalid IL or missing references)
			//IL_072c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0731: Unknown result type (might be due to invalid IL or missing references)
			//IL_073c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0748: Expected O, but got Unknown
			//IL_0815: Unknown result type (might be due to invalid IL or missing references)
			//IL_081a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0825: Unknown result type (might be due to invalid IL or missing references)
			//IL_083b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0847: Expected O, but got Unknown
			//IL_085f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0864: Unknown result type (might be due to invalid IL or missing references)
			//IL_087f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0886: Unknown result type (might be due to invalid IL or missing references)
			//IL_088d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0899: Expected O, but got Unknown
			//IL_089a: Unknown result type (might be due to invalid IL or missing references)
			//IL_089f: Unknown result type (might be due to invalid IL or missing references)
			//IL_08aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e8: Expected O, but got Unknown
			//IL_0900: Unknown result type (might be due to invalid IL or missing references)
			//IL_0905: Unknown result type (might be due to invalid IL or missing references)
			//IL_0925: Unknown result type (might be due to invalid IL or missing references)
			//IL_092c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0933: Unknown result type (might be due to invalid IL or missing references)
			//IL_093f: Expected O, but got Unknown
			//IL_0940: Unknown result type (might be due to invalid IL or missing references)
			//IL_0945: Unknown result type (might be due to invalid IL or missing references)
			//IL_0950: Unknown result type (might be due to invalid IL or missing references)
			//IL_095b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0971: Unknown result type (might be due to invalid IL or missing references)
			//IL_097c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0988: Expected O, but got Unknown
			//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_09df: Expected O, but got Unknown
			//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a11: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a28: Expected O, but got Unknown
			//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a64: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a74: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a80: Expected O, but got Unknown
			//IL_0aeb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0afc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b07: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b13: Expected O, but got Unknown
			//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b45: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bbf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bd6: Expected O, but got Unknown
			//IL_0c40: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c45: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c50: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c57: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c65: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c81: Expected O, but got Unknown
			//IL_0cf4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d0b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d18: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d1d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d28: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d2f: Unknown result type (might be due to invalid IL or missing references)
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
			KeybindingAssigner val4 = new KeybindingAssigner(_module.PauseKeybindSetting.get_Value());
			val4.set_KeyBindingName("Pause / resume reading");
			((Control)val4).set_Parent((Container)(object)panel);
			KeybindingAssigner val5 = new KeybindingAssigner(_module.BookToggleKeybindSetting.get_Value());
			val5.set_KeyBindingName("Toggle lorebook detection");
			((Control)val5).set_Parent((Container)(object)panel);
			Checkbox val6 = new Checkbox();
			val6.set_Text("Show speaker icon on open books");
			val6.set_Checked(_module.ShowSpeakerButtonSetting.get_Value());
			((Control)val6).set_Parent((Container)(object)panel);
			val6.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.ShowSpeakerButtonSetting.set_Value(e.get_Checked());
			});
			Checkbox val7 = new Checkbox();
			val7.set_Text("Conversation capture mode (also detect NPC dialogues)");
			val7.set_Checked(_module.ConversationCaptureSetting.get_Value());
			((Control)val7).set_Parent((Container)(object)panel);
			Checkbox convCheckbox = val7;
			convCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.ConversationCaptureSetting.set_Value(e.get_Checked());
			});
			_module.ConversationCaptureSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				convCheckbox.set_Checked(e.get_NewValue());
			});
			KeybindingAssigner val8 = new KeybindingAssigner(_module.ConvToggleKeybindSetting.get_Value());
			val8.set_KeyBindingName("Toggle conversation capture");
			((Control)val8).set_Parent((Container)(object)panel);
			Label val9 = new Label();
			val9.set_Text((_module.DialogZoneSetting.get_Value().Length > 0) ? "Dialogue zone: calibrated" : "Dialogue zone: not calibrated");
			val9.set_AutoSizeWidth(true);
			val9.set_AutoSizeHeight(true);
			((Control)val9).set_Parent((Container)(object)panel);
			Label calibStatus = val9;
			KeybindingAssigner val10 = new KeybindingAssigner(_module.CalibrateKeybindSetting.get_Value());
			val10.set_KeyBindingName("Calibrate dialogue zone");
			((Control)val10).set_Parent((Container)(object)panel);
			StandardButton val11 = new StandardButton();
			val11.set_Text("Calibrate dialogue zone (drag a frame)");
			((Control)val11).set_Width(360);
			((Control)val11).set_Parent((Container)(object)panel);
			((Control)val11).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.StartCalibration();
			});
			StandardButton val12 = new StandardButton();
			val12.set_Text("Clear calibration (use auto-detect)");
			((Control)val12).set_Width(360);
			((Control)val12).set_Parent((Container)(object)panel);
			((Control)val12).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.DialogZoneSetting.set_Value("");
			});
			_module.DialogZoneSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				calibStatus.set_Text((!string.IsNullOrEmpty(e.get_NewValue())) ? "Dialogue zone: calibrated" : "Dialogue zone: not calibrated");
			});
			Label val13 = new Label();
			val13.set_Text((_module.BookZoneSetting.get_Value().Length > 0) ? "Lorebook OCR area: calibrated" : "Lorebook OCR area: auto (default)");
			val13.set_AutoSizeWidth(true);
			val13.set_AutoSizeHeight(true);
			((Control)val13).set_Parent((Container)(object)panel);
			Label bookCalibStatus = val13;
			KeybindingAssigner val14 = new KeybindingAssigner(_module.BookCalibrateKeybindSetting.get_Value());
			val14.set_KeyBindingName("Calibrate lorebook OCR area");
			((Control)val14).set_Parent((Container)(object)panel);
			StandardButton val15 = new StandardButton();
			val15.set_Text("Calibrate lorebook OCR area (open a book, drag a frame)");
			((Control)val15).set_Width(360);
			((Control)val15).set_Parent((Container)(object)panel);
			((Control)val15).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.StartBookCalibration();
			});
			StandardButton val16 = new StandardButton();
			val16.set_Text("Clear lorebook OCR area (use auto-detect)");
			((Control)val16).set_Width(360);
			((Control)val16).set_Parent((Container)(object)panel);
			((Control)val16).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.BookZoneSetting.set_Value("");
			});
			_module.BookZoneSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
			{
				bookCalibStatus.set_Text((!string.IsNullOrEmpty(e.get_NewValue())) ? "Lorebook OCR area: calibrated" : "Lorebook OCR area: auto (default)");
			});
			KeybindingAssigner val17 = new KeybindingAssigner(_module.DebugDumpKeybindSetting.get_Value());
			val17.set_KeyBindingName("Save debug capture");
			((Control)val17).set_Parent((Container)(object)panel);
			Label val18 = new Label();
			val18.set_Text("Voice engine");
			val18.set_AutoSizeWidth(true);
			val18.set_AutoSizeHeight(true);
			((Control)val18).set_Parent((Container)(object)panel);
			Dropdown val19 = new Dropdown();
			((Control)val19).set_Width(360);
			((Control)val19).set_Parent((Container)(object)panel);
			Dropdown engineDropdown = val19;
			engineDropdown.get_Items().Add("Windows voices (offline)");
			engineDropdown.get_Items().Add("Edge neural voices (online)");
			engineDropdown.set_SelectedItem((_module.VoiceEngineSetting.get_Value() == "edge") ? "Edge neural voices (online)" : "Windows voices (offline)");
			engineDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_module.VoiceEngineSetting.set_Value((engineDropdown.get_SelectedItem() == "Edge neural voices (online)") ? "edge" : "windows");
				UpdateEngineVisibility();
			});
			Label val20 = new Label();
			val20.set_Text("Windows voice");
			val20.set_AutoSizeWidth(true);
			val20.set_AutoSizeHeight(true);
			((Control)val20).set_Parent((Container)(object)panel);
			_winVoiceLabel = val20;
			Dropdown val21 = new Dropdown();
			((Control)val21).set_Width(360);
			((Control)val21).set_Parent((Container)(object)panel);
			_winVoiceDropdown = val21;
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
			Label val22 = new Label();
			val22.set_Text("Edge neural voice (requires internet)");
			val22.set_AutoSizeWidth(true);
			val22.set_AutoSizeHeight(true);
			((Control)val22).set_Parent((Container)(object)panel);
			_edgeVoiceLabel = val22;
			Dropdown val23 = new Dropdown();
			((Control)val23).set_Width(360);
			((Control)val23).set_Parent((Container)(object)panel);
			_edgeVoiceDropdown = val23;
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
			Label val24 = new Label();
			val24.set_Text(RateText(_module.SpeakingRateSetting.get_Value()));
			val24.set_AutoSizeWidth(true);
			val24.set_AutoSizeHeight(true);
			((Control)val24).set_Parent((Container)(object)panel);
			Label rateLabel = val24;
			TrackBar val25 = new TrackBar();
			val25.set_MinValue(50f);
			val25.set_MaxValue(200f);
			val25.set_Value(_module.SpeakingRateSetting.get_Value() * 100f);
			((Control)val25).set_Width(360);
			((Control)val25).set_Parent((Container)(object)panel);
			TrackBar rateBar = val25;
			rateBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num4 = (float)Math.Round(rateBar.get_Value()) / 100f;
				_module.SpeakingRateSetting.set_Value(num4);
				rateLabel.set_Text(RateText(num4));
			});
			Label val26 = new Label();
			val26.set_Text("OCR language (your GW2 client language)");
			val26.set_AutoSizeWidth(true);
			val26.set_AutoSizeHeight(true);
			((Control)val26).set_Parent((Container)(object)panel);
			Dropdown val27 = new Dropdown();
			((Control)val27).set_Width(360);
			((Control)val27).set_Parent((Container)(object)panel);
			Dropdown ocrDropdown = val27;
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
			Checkbox val28 = new Checkbox();
			val28.set_Text("Show subtitles while reading");
			val28.set_Checked(_module.ShowSubtitlesSetting.get_Value());
			((Control)val28).set_Parent((Container)(object)panel);
			Checkbox subsCheckbox = val28;
			subsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.ShowSubtitlesSetting.set_Value(e.get_Checked());
			});
			Label val29 = new Label();
			val29.set_Text(OpacityText(_module.SubtitleOpacitySetting.get_Value()));
			val29.set_AutoSizeWidth(true);
			val29.set_AutoSizeHeight(true);
			((Control)val29).set_Parent((Container)(object)panel);
			Label opacityLabel = val29;
			TrackBar val30 = new TrackBar();
			val30.set_MinValue(20f);
			val30.set_MaxValue(100f);
			val30.set_Value(_module.SubtitleOpacitySetting.get_Value() * 100f);
			((Control)val30).set_Width(360);
			((Control)val30).set_Parent((Container)(object)panel);
			TrackBar opacityBar = val30;
			opacityBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num3 = (float)Math.Round(opacityBar.get_Value()) / 100f;
				_module.SubtitleOpacitySetting.set_Value(num3);
				opacityLabel.set_Text(OpacityText(num3));
			});
			Label val31 = new Label();
			val31.set_Text(PosText("X", _module.SubtitleXSetting.get_Value()));
			val31.set_AutoSizeWidth(true);
			val31.set_AutoSizeHeight(true);
			((Control)val31).set_Parent((Container)(object)panel);
			Label posXLabel = val31;
			TrackBar val32 = new TrackBar();
			val32.set_MinValue(0f);
			val32.set_MaxValue(100f);
			val32.set_Value(_module.SubtitleXSetting.get_Value());
			((Control)val32).set_Width(360);
			((Control)val32).set_Parent((Container)(object)panel);
			TrackBar posXBar = val32;
			posXBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num2 = (float)Math.Round(posXBar.get_Value());
				_module.SubtitleXSetting.set_Value(num2);
				posXLabel.set_Text(PosText("X", num2));
			});
			Label val33 = new Label();
			val33.set_Text(PosText("Y", _module.SubtitleYSetting.get_Value()));
			val33.set_AutoSizeWidth(true);
			val33.set_AutoSizeHeight(true);
			((Control)val33).set_Parent((Container)(object)panel);
			Label posYLabel = val33;
			TrackBar val34 = new TrackBar();
			val34.set_MinValue(0f);
			val34.set_MaxValue(100f);
			val34.set_Value(_module.SubtitleYSetting.get_Value());
			((Control)val34).set_Width(360);
			((Control)val34).set_Parent((Container)(object)panel);
			TrackBar posYBar = val34;
			posYBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num = (float)Math.Round(posYBar.get_Value());
				_module.SubtitleYSetting.set_Value(num);
				posYLabel.set_Text(PosText("Y", num));
			});
			Label val35 = new Label();
			val35.set_Text("Subtitle size");
			val35.set_AutoSizeWidth(true);
			val35.set_AutoSizeHeight(true);
			((Control)val35).set_Parent((Container)(object)panel);
			Dropdown val36 = new Dropdown();
			((Control)val36).set_Width(360);
			((Control)val36).set_Parent((Container)(object)panel);
			Dropdown sizeDropdown = val36;
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
			StandardButton val37 = new StandardButton();
			val37.set_Text(EditButtonText());
			((Control)val37).set_Width(360);
			((Control)val37).set_Parent((Container)(object)panel);
			StandardButton editButton = val37;
			((Control)editButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.SubtitleEditMode = !_module.SubtitleEditMode;
				editButton.set_Text(EditButtonText());
			});
			StandardButton val38 = new StandardButton();
			val38.set_Text("Reset subtitles to defaults");
			((Control)val38).set_Width(360);
			((Control)val38).set_Parent((Container)(object)panel);
			((Control)val38).add_Click((EventHandler<MouseEventArgs>)delegate
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
			Label val39 = new Label();
			val39.set_Text("Translation");
			val39.set_AutoSizeWidth(true);
			val39.set_AutoSizeHeight(true);
			((Control)val39).set_Parent((Container)(object)panel);
			Dropdown val40 = new Dropdown();
			((Control)val40).set_Width(360);
			((Control)val40).set_Parent((Container)(object)panel);
			Dropdown modeDropdown = val40;
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
			Label val41 = new Label();
			val41.set_Text("Translate to");
			val41.set_AutoSizeWidth(true);
			val41.set_AutoSizeHeight(true);
			((Control)val41).set_Parent((Container)(object)panel);
			Dropdown val42 = new Dropdown();
			((Control)val42).set_Width(360);
			((Control)val42).set_Parent((Container)(object)panel);
			Dropdown langDropdown = val42;
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
			Label val43 = new Label();
			val43.set_Text("Note: translation uses a free online service and may occasionally be unavailable.");
			val43.set_AutoSizeWidth(true);
			val43.set_AutoSizeHeight(true);
			((Control)val43).set_Parent((Container)(object)panel);
			Label val44 = new Label();
			val44.set_Text("Reading history is available via the Lorebook Reader icon in the top-left icon bar.");
			val44.set_AutoSizeWidth(true);
			val44.set_AutoSizeHeight(true);
			((Control)val44).set_Parent((Container)(object)panel);
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
