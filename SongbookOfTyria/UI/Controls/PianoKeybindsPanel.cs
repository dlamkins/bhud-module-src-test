using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using SongbookOfTyria.Models;
using SongbookOfTyria.Services;

namespace SongbookOfTyria.UI.Controls
{
	public sealed class PianoKeybindsPanel : FlowPanel
	{
		private const int DefaultHeight = 120;

		private const string CircledOne = "①";

		private const string CircledTwo = "②";

		private const string CircledThree = "③";

		private const string CircledFour = "④";

		private const string CircledFive = "⑤";

		private readonly UserSettingsService _userSettingsService;

		private readonly Action _onKeybindsApplied;

		private TextBox _keybindCsDb;

		private TextBox _keybindDsEb;

		private TextBox _keybindFsGb;

		private TextBox _keybindGsAb;

		private TextBox _keybindAsBb;

		private PianoKeybinds _pianoKeybinds;

		private bool _lastCollapsedState;

		public PianoKeybinds Keybinds => _pianoKeybinds;

		public event EventHandler<bool> CollapsedChanged;

		public PianoKeybindsPanel(int sectionWidth, bool collapsed, PianoKeybinds initialKeybinds, UserSettingsService userSettingsService, Action onKeybindsApplied)
			: this()
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			_userSettingsService = userSettingsService;
			_onKeybindsApplied = onKeybindsApplied;
			_pianoKeybinds = initialKeybinds ?? new PianoKeybinds();
			_lastCollapsedState = collapsed;
			((Panel)this).set_ShowBorder(true);
			((Panel)this).set_Title("Piano Keybinds");
			((Panel)this).set_CanCollapse(true);
			((Control)this).set_Width(sectionWidth);
			((Control)this).set_Height(120);
			((Container)this).set_HeightSizingMode((SizingMode)0);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 5f));
			((FlowPanel)this).set_OuterControlPadding(new Vector2(0f, 12f));
			BuildContent(sectionWidth);
			((Panel)this).set_Collapsed(collapsed);
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)OnResized);
		}

		private void BuildContent(int sectionWidth)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Expected O, but got Unknown
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Expected O, but got Unknown
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			int keybindsRowWidth = 395;
			int keybindsRowPadding = (sectionWidth - keybindsRowWidth) / 2;
			int presetsRowWidth = 325;
			int presetsRowPadding = (sectionWidth - presetsRowWidth) / 2;
			Panel val = new Panel();
			((Control)val).set_Width(sectionWidth);
			((Control)val).set_Height(26);
			((Control)val).set_Parent((Container)(object)this);
			Panel keybindsWrapper = val;
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)2);
			((Control)val2).set_Width(keybindsRowWidth);
			((Control)val2).set_Height(26);
			((Control)val2).set_Location(new Point(keybindsRowPadding, 0));
			val2.set_ControlPadding(new Vector2(5f, 0f));
			((Control)val2).set_Parent((Container)(object)keybindsWrapper);
			FlowPanel keybindsRow = val2;
			_keybindCsDb = CreateKeybindInput(keybindsRow, "F1:", _pianoKeybinds.CsDb);
			_keybindDsEb = CreateKeybindInput(keybindsRow, "F2:", _pianoKeybinds.DsEb);
			_keybindFsGb = CreateKeybindInput(keybindsRow, "F3:", _pianoKeybinds.FsGb);
			_keybindGsAb = CreateKeybindInput(keybindsRow, "F4:", _pianoKeybinds.GsAb);
			_keybindAsBb = CreateKeybindInput(keybindsRow, "F5:", _pianoKeybinds.AsBb);
			Panel val3 = new Panel();
			((Control)val3).set_Width(sectionWidth);
			((Control)val3).set_Height(30);
			((Control)val3).set_Parent((Container)(object)this);
			Panel presetsWrapper = val3;
			FlowPanel val4 = new FlowPanel();
			val4.set_FlowDirection((ControlFlowDirection)2);
			((Control)val4).set_Width(presetsRowWidth);
			((Control)val4).set_Height(30);
			((Control)val4).set_Location(new Point(presetsRowPadding, 0));
			val4.set_ControlPadding(new Vector2(5f, 0f));
			((Control)val4).set_Parent((Container)(object)presetsWrapper);
			FlowPanel presetsRow = val4;
			CreatePresetButton(presetsRow, "Default", "Preset: Circled Numbers (displayed as 1-5)", 80, PianoKeybinds.CreateDefault);
			CreatePresetButton(presetsRow, "'1 '2 '3...", "Preset: Apostrophe", 80, PianoKeybinds.CreateApostrophe);
			CreatePresetButton(presetsRow, "#1 #2 #3...", "Preset: Hashtag", 80, PianoKeybinds.CreateHashtag);
			StandardButton val5 = new StandardButton();
			val5.set_Text("Apply");
			((Control)val5).set_BasicTooltipText("Apply keybinds to notation");
			((Control)val5).set_Width(70);
			((Control)val5).set_Parent((Container)(object)presetsRow);
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)OnApplyClicked);
		}

		private TextBox CreateKeybindInput(FlowPanel parent, string label, string value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			((Control)val).set_Width(75);
			((Control)val).set_Height(26);
			val.set_ControlPadding(new Vector2(2f, 0f));
			((Control)val).set_Parent((Container)(object)parent);
			FlowPanel container = val;
			Panel val2 = new Panel();
			((Control)val2).set_Width(25);
			((Control)val2).set_Height(26);
			((Control)val2).set_Parent((Container)(object)container);
			Panel labelContainer = val2;
			Label val3 = new Label();
			val3.set_Text(label);
			val3.set_Font(GameService.Content.get_DefaultFont12());
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Location(new Point(0, 5));
			((Control)val3).set_Parent((Container)(object)labelContainer);
			TextBox val4 = new TextBox();
			((TextInputBase)val4).set_Text(ToDisplayFormat(value));
			((Control)val4).set_Width(45);
			((TextInputBase)val4).set_MaxLength(3);
			((Control)val4).set_Parent((Container)(object)container);
			return val4;
		}

		private void CreatePresetButton(FlowPanel parent, string text, string tooltip, int width, Func<PianoKeybinds> presetFactory)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			val.set_Text(text);
			((Control)val).set_BasicTooltipText(tooltip);
			((Control)val).set_Width(width);
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ApplyPresetToInputs(presetFactory());
			});
		}

		private void ApplyPresetToInputs(PianoKeybinds preset)
		{
			if (_keybindCsDb != null)
			{
				((TextInputBase)_keybindCsDb).set_Text(ToDisplayFormat(preset.CsDb));
			}
			if (_keybindDsEb != null)
			{
				((TextInputBase)_keybindDsEb).set_Text(ToDisplayFormat(preset.DsEb));
			}
			if (_keybindFsGb != null)
			{
				((TextInputBase)_keybindFsGb).set_Text(ToDisplayFormat(preset.FsGb));
			}
			if (_keybindGsAb != null)
			{
				((TextInputBase)_keybindGsAb).set_Text(ToDisplayFormat(preset.GsAb));
			}
			if (_keybindAsBb != null)
			{
				((TextInputBase)_keybindAsBb).set_Text(ToDisplayFormat(preset.AsBb));
			}
		}

		private void OnApplyClicked(object sender, MouseEventArgs e)
		{
			PianoKeybinds pianoKeybinds = new PianoKeybinds();
			TextBox keybindCsDb = _keybindCsDb;
			pianoKeybinds.CsDb = ToStorageFormat((keybindCsDb != null) ? ((TextInputBase)keybindCsDb).get_Text() : null) ?? "①";
			TextBox keybindDsEb = _keybindDsEb;
			pianoKeybinds.DsEb = ToStorageFormat((keybindDsEb != null) ? ((TextInputBase)keybindDsEb).get_Text() : null) ?? "②";
			TextBox keybindFsGb = _keybindFsGb;
			pianoKeybinds.FsGb = ToStorageFormat((keybindFsGb != null) ? ((TextInputBase)keybindFsGb).get_Text() : null) ?? "③";
			TextBox keybindGsAb = _keybindGsAb;
			pianoKeybinds.GsAb = ToStorageFormat((keybindGsAb != null) ? ((TextInputBase)keybindGsAb).get_Text() : null) ?? "④";
			TextBox keybindAsBb = _keybindAsBb;
			pianoKeybinds.AsBb = ToStorageFormat((keybindAsBb != null) ? ((TextInputBase)keybindAsBb).get_Text() : null) ?? "⑤";
			_pianoKeybinds = pianoKeybinds;
			_userSettingsService?.SavePianoKeybinds(_pianoKeybinds);
			_onKeybindsApplied?.Invoke();
		}

		private void OnResized(object sender, ResizedEventArgs e)
		{
			if (((Panel)this).get_Collapsed() != _lastCollapsedState)
			{
				_lastCollapsedState = ((Panel)this).get_Collapsed();
				this.CollapsedChanged?.Invoke(this, ((Panel)this).get_Collapsed());
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			if (((Panel)this).get_Collapsed() != _lastCollapsedState)
			{
				_lastCollapsedState = ((Panel)this).get_Collapsed();
				this.CollapsedChanged?.Invoke(this, ((Panel)this).get_Collapsed());
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Resized((EventHandler<ResizedEventArgs>)OnResized);
			((FlowPanel)this).DisposeControl();
		}

		private static string ToDisplayFormat(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return "1";
			}
			return value.Replace("①", "1").Replace("②", "2").Replace("③", "3")
				.Replace("④", "4")
				.Replace("⑤", "5");
		}

		private static string ToStorageFormat(string displayValue)
		{
			if (string.IsNullOrEmpty(displayValue))
			{
				return "①";
			}
			return displayValue switch
			{
				"1" => "①", 
				"2" => "②", 
				"3" => "③", 
				"4" => "④", 
				"5" => "⑤", 
				_ => displayValue, 
			};
		}
	}
}
