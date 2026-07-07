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
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Expected O, but got Unknown
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Expected O, but got Unknown
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Expected O, but got Unknown
			//IL_0457: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Expected O, but got Unknown
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Expected O, but got Unknown
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Expected O, but got Unknown
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_057f: Unknown result type (might be due to invalid IL or missing references)
			//IL_059b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Expected O, but got Unknown
			//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Expected O, but got Unknown
			//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Expected O, but got Unknown
			//IL_0721: Unknown result type (might be due to invalid IL or missing references)
			//IL_0726: Unknown result type (might be due to invalid IL or missing references)
			//IL_0741: Unknown result type (might be due to invalid IL or missing references)
			//IL_0748: Unknown result type (might be due to invalid IL or missing references)
			//IL_074f: Unknown result type (might be due to invalid IL or missing references)
			//IL_075b: Expected O, but got Unknown
			//IL_075c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0761: Unknown result type (might be due to invalid IL or missing references)
			//IL_076c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0777: Unknown result type (might be due to invalid IL or missing references)
			//IL_0793: Unknown result type (might be due to invalid IL or missing references)
			//IL_079e: Unknown result type (might be due to invalid IL or missing references)
			//IL_07aa: Expected O, but got Unknown
			//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0801: Expected O, but got Unknown
			//IL_0802: Unknown result type (might be due to invalid IL or missing references)
			//IL_0807: Unknown result type (might be due to invalid IL or missing references)
			//IL_0812: Unknown result type (might be due to invalid IL or missing references)
			//IL_081d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0833: Unknown result type (might be due to invalid IL or missing references)
			//IL_083e: Unknown result type (might be due to invalid IL or missing references)
			//IL_084a: Expected O, but got Unknown
			//IL_0862: Unknown result type (might be due to invalid IL or missing references)
			//IL_0867: Unknown result type (might be due to invalid IL or missing references)
			//IL_0887: Unknown result type (might be due to invalid IL or missing references)
			//IL_088e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0895: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a1: Expected O, but got Unknown
			//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_08de: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ea: Expected O, but got Unknown
			//IL_0901: Unknown result type (might be due to invalid IL or missing references)
			//IL_0906: Unknown result type (might be due to invalid IL or missing references)
			//IL_0911: Unknown result type (might be due to invalid IL or missing references)
			//IL_0918: Unknown result type (might be due to invalid IL or missing references)
			//IL_0926: Unknown result type (might be due to invalid IL or missing references)
			//IL_092b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0936: Unknown result type (might be due to invalid IL or missing references)
			//IL_0942: Expected O, but got Unknown
			//IL_09ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_09be: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d5: Expected O, but got Unknown
			//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a57: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a67: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a81: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a8c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a98: Expected O, but got Unknown
			//IL_0b02: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b07: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b19: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b27: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b37: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b43: Expected O, but got Unknown
			//IL_0bb6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bc6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bcd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bda: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bdf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c2d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c34: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c3b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c47: Expected O, but got Unknown
			//IL_0c48: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c58: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c85: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c91: Expected O, but got Unknown
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
			KeybindingAssigner val11 = new KeybindingAssigner(_module.DebugDumpKeybindSetting.get_Value());
			val11.set_KeyBindingName("Save debug capture");
			((Control)val11).set_Parent((Container)(object)panel);
			Label val12 = new Label();
			val12.set_Text("Voice engine");
			val12.set_AutoSizeWidth(true);
			val12.set_AutoSizeHeight(true);
			((Control)val12).set_Parent((Container)(object)panel);
			Dropdown val13 = new Dropdown();
			((Control)val13).set_Width(360);
			((Control)val13).set_Parent((Container)(object)panel);
			Dropdown engineDropdown = val13;
			engineDropdown.get_Items().Add("Windows voices (offline)");
			engineDropdown.get_Items().Add("Edge neural voices (online)");
			engineDropdown.set_SelectedItem((_module.VoiceEngineSetting.get_Value() == "edge") ? "Edge neural voices (online)" : "Windows voices (offline)");
			engineDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				_module.VoiceEngineSetting.set_Value((engineDropdown.get_SelectedItem() == "Edge neural voices (online)") ? "edge" : "windows");
				UpdateEngineVisibility();
			});
			Label val14 = new Label();
			val14.set_Text("Windows voice");
			val14.set_AutoSizeWidth(true);
			val14.set_AutoSizeHeight(true);
			((Control)val14).set_Parent((Container)(object)panel);
			_winVoiceLabel = val14;
			Dropdown val15 = new Dropdown();
			((Control)val15).set_Width(360);
			((Control)val15).set_Parent((Container)(object)panel);
			_winVoiceDropdown = val15;
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
			Label val16 = new Label();
			val16.set_Text("Edge neural voice (requires internet)");
			val16.set_AutoSizeWidth(true);
			val16.set_AutoSizeHeight(true);
			((Control)val16).set_Parent((Container)(object)panel);
			_edgeVoiceLabel = val16;
			Dropdown val17 = new Dropdown();
			((Control)val17).set_Width(360);
			((Control)val17).set_Parent((Container)(object)panel);
			_edgeVoiceDropdown = val17;
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
			Label val18 = new Label();
			val18.set_Text(RateText(_module.SpeakingRateSetting.get_Value()));
			val18.set_AutoSizeWidth(true);
			val18.set_AutoSizeHeight(true);
			((Control)val18).set_Parent((Container)(object)panel);
			Label rateLabel = val18;
			TrackBar val19 = new TrackBar();
			val19.set_MinValue(50f);
			val19.set_MaxValue(200f);
			val19.set_Value(_module.SpeakingRateSetting.get_Value() * 100f);
			((Control)val19).set_Width(360);
			((Control)val19).set_Parent((Container)(object)panel);
			TrackBar rateBar = val19;
			rateBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num5 = (float)Math.Round(rateBar.get_Value()) / 100f;
				_module.SpeakingRateSetting.set_Value(num5);
				rateLabel.set_Text(RateText(num5));
			});
			Label val20 = new Label();
			val20.set_Text("OCR language (your GW2 client language)");
			val20.set_AutoSizeWidth(true);
			val20.set_AutoSizeHeight(true);
			((Control)val20).set_Parent((Container)(object)panel);
			Dropdown val21 = new Dropdown();
			((Control)val21).set_Width(360);
			((Control)val21).set_Parent((Container)(object)panel);
			Dropdown ocrDropdown = val21;
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
			Checkbox val22 = new Checkbox();
			val22.set_Text("Show subtitles while reading");
			val22.set_Checked(_module.ShowSubtitlesSetting.get_Value());
			((Control)val22).set_Parent((Container)(object)panel);
			Checkbox subsCheckbox = val22;
			subsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.ShowSubtitlesSetting.set_Value(e.get_Checked());
			});
			Label val23 = new Label();
			val23.set_Text(OpacityText(_module.SubtitleOpacitySetting.get_Value()));
			val23.set_AutoSizeWidth(true);
			val23.set_AutoSizeHeight(true);
			((Control)val23).set_Parent((Container)(object)panel);
			Label opacityLabel = val23;
			TrackBar val24 = new TrackBar();
			val24.set_MinValue(20f);
			val24.set_MaxValue(100f);
			val24.set_Value(_module.SubtitleOpacitySetting.get_Value() * 100f);
			((Control)val24).set_Width(360);
			((Control)val24).set_Parent((Container)(object)panel);
			TrackBar opacityBar = val24;
			opacityBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num4 = (float)Math.Round(opacityBar.get_Value()) / 100f;
				_module.SubtitleOpacitySetting.set_Value(num4);
				opacityLabel.set_Text(OpacityText(num4));
			});
			Label val25 = new Label();
			val25.set_Text(PosText("X", _module.SubtitleXSetting.get_Value()));
			val25.set_AutoSizeWidth(true);
			val25.set_AutoSizeHeight(true);
			((Control)val25).set_Parent((Container)(object)panel);
			Label posXLabel = val25;
			TrackBar val26 = new TrackBar();
			val26.set_MinValue(0f);
			val26.set_MaxValue(100f);
			val26.set_Value(_module.SubtitleXSetting.get_Value());
			((Control)val26).set_Width(360);
			((Control)val26).set_Parent((Container)(object)panel);
			TrackBar posXBar = val26;
			posXBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num3 = (float)Math.Round(posXBar.get_Value());
				_module.SubtitleXSetting.set_Value(num3);
				posXLabel.set_Text(PosText("X", num3));
			});
			Label val27 = new Label();
			val27.set_Text(PosText("Y", _module.SubtitleYSetting.get_Value()));
			val27.set_AutoSizeWidth(true);
			val27.set_AutoSizeHeight(true);
			((Control)val27).set_Parent((Container)(object)panel);
			Label posYLabel = val27;
			TrackBar val28 = new TrackBar();
			val28.set_MinValue(0f);
			val28.set_MaxValue(100f);
			val28.set_Value(_module.SubtitleYSetting.get_Value());
			((Control)val28).set_Width(360);
			((Control)val28).set_Parent((Container)(object)panel);
			TrackBar posYBar = val28;
			posYBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				float num2 = (float)Math.Round(posYBar.get_Value());
				_module.SubtitleYSetting.set_Value(num2);
				posYLabel.set_Text(PosText("Y", num2));
			});
			Label val29 = new Label();
			val29.set_Text("Subtitle size");
			val29.set_AutoSizeWidth(true);
			val29.set_AutoSizeHeight(true);
			((Control)val29).set_Parent((Container)(object)panel);
			Dropdown val30 = new Dropdown();
			((Control)val30).set_Width(360);
			((Control)val30).set_Parent((Container)(object)panel);
			Dropdown sizeDropdown = val30;
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
			StandardButton val31 = new StandardButton();
			val31.set_Text(EditButtonText());
			((Control)val31).set_Width(360);
			((Control)val31).set_Parent((Container)(object)panel);
			StandardButton editButton = val31;
			((Control)editButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.SubtitleEditMode = !_module.SubtitleEditMode;
				editButton.set_Text(EditButtonText());
			});
			StandardButton val32 = new StandardButton();
			val32.set_Text("Reset subtitles to defaults");
			((Control)val32).set_Width(360);
			((Control)val32).set_Parent((Container)(object)panel);
			((Control)val32).add_Click((EventHandler<MouseEventArgs>)delegate
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
			Label val33 = new Label();
			val33.set_Text("Translation");
			val33.set_AutoSizeWidth(true);
			val33.set_AutoSizeHeight(true);
			((Control)val33).set_Parent((Container)(object)panel);
			Dropdown val34 = new Dropdown();
			((Control)val34).set_Width(360);
			((Control)val34).set_Parent((Container)(object)panel);
			Dropdown modeDropdown = val34;
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
			Label val35 = new Label();
			val35.set_Text("Translate to");
			val35.set_AutoSizeWidth(true);
			val35.set_AutoSizeHeight(true);
			((Control)val35).set_Parent((Container)(object)panel);
			Dropdown val36 = new Dropdown();
			((Control)val36).set_Width(360);
			((Control)val36).set_Parent((Container)(object)panel);
			Dropdown langDropdown = val36;
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
			Label val37 = new Label();
			val37.set_Text("Note: translation uses a free online service and may occasionally be unavailable.");
			val37.set_AutoSizeWidth(true);
			val37.set_AutoSizeHeight(true);
			((Control)val37).set_Parent((Container)(object)panel);
			Label val38 = new Label();
			val38.set_Text("Reading history is available via the Lorebook Reader icon in the top-left icon bar.");
			val38.set_AutoSizeWidth(true);
			val38.set_AutoSizeHeight(true);
			((Control)val38).set_Parent((Container)(object)panel);
			Label val39 = new Label();
			val39.set_Text("Catalog size: " + _module.HistoryCapacitySetting.get_Value());
			val39.set_AutoSizeWidth(true);
			val39.set_AutoSizeHeight(true);
			((Control)val39).set_Parent((Container)(object)panel);
			Label capLabel = val39;
			TrackBar val40 = new TrackBar();
			val40.set_MinValue(5f);
			val40.set_MaxValue(100f);
			val40.set_Value((float)_module.HistoryCapacitySetting.get_Value());
			((Control)val40).set_Width(360);
			((Control)val40).set_Parent((Container)(object)panel);
			TrackBar capBar = val40;
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
