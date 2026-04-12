using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Microsoft.Xna.Framework;

namespace Maestro.UI.MaestroCreator
{
	public class DurationSelector : Panel
	{
		public static class Layout
		{
			public const int Height = 30;

			public const int BpmLabelWidth = 40;

			public const int BpmInputWidth = 50;

			public const int NoteButtonWidth = 40;

			public const int NoteButtonHeight = 26;

			public const int Spacing = 5;

			public const int NoteButtonsLeftMargin = 15;

			public const int NoteButtonGap = 2;

			public const int MinBpm = 20;

			public const int MaxBpm = 300;
		}

		private int _bpm = 120;

		private string _lastValidBpmText = "120";

		private NoteType _selectedNoteType = NoteType.Quarter;

		private bool _isDotted;

		private readonly Label _bpmLabel;

		private readonly TextBox _bpmInput;

		private readonly Panel[] _noteButtons;

		private readonly Label[] _noteLabels;

		private readonly NoteType[] _noteTypes = new NoteType[5]
		{
			NoteType.Whole,
			NoteType.Half,
			NoteType.Quarter,
			NoteType.Eighth,
			NoteType.Sixteenth
		};

		private readonly Panel _dottedButton;

		private readonly Label _dottedLabel;

		private Color _accentColor = MaestroTheme.AmberGold;

		private Color _accentColorDark = MaestroTheme.DeepAmber;

		public int Bpm
		{
			get
			{
				return _bpm;
			}
			set
			{
				_bpm = ClampBpm(value);
				UpdateBpmDisplay();
				this.DurationChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public NoteType SelectedNoteType
		{
			get
			{
				return _selectedNoteType;
			}
			set
			{
				_selectedNoteType = value;
				UpdateNoteTypeSelection();
				UpdateTooltips();
				this.DurationChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public bool IsDotted
		{
			get
			{
				return _isDotted;
			}
			set
			{
				_isDotted = value;
				UpdateDottedVisual();
				this.DurationChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public int CurrentDurationMs
		{
			get
			{
				int baseDuration = _selectedNoteType.GetDurationMs(_bpm);
				if (!_isDotted)
				{
					return baseDuration;
				}
				return (int)((double)baseDuration * 1.5);
			}
		}

		public event EventHandler DurationChanged;

		private static int ClampBpm(int value)
		{
			if (value >= 20)
			{
				if (value <= 300)
				{
					return value;
				}
				return 300;
			}
			return 20;
		}

		public void SetAccentColor(InstrumentType instrument)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			_accentColor = MaestroTheme.GetInstrumentAccent(instrument);
			_accentColorDark = MaestroTheme.GetInstrumentAccentDark(instrument);
			UpdateNoteTypeSelection();
			UpdateDottedVisual();
		}

		public DurationSelector(int width)
			: this()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected O, but got Unknown
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Expected O, but got Unknown
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Expected O, but got Unknown
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Expected O, but got Unknown
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Expected O, but got Unknown
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Expected O, but got Unknown
			((Control)this).set_Size(new Point(width, 30));
			((Control)this).set_BackgroundColor(Color.get_Transparent());
			_noteButtons = (Panel[])(object)new Panel[_noteTypes.Length];
			_noteLabels = (Label[])(object)new Label[_noteTypes.Length];
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("BPM:");
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(40, 26));
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.InputLabelColor);
			val.set_VerticalAlignment((VerticalAlignment)1);
			_bpmLabel = val;
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)this);
			((TextInputBase)val2).set_Text(_bpm.ToString());
			((Control)val2).set_Location(new Point(40, 0));
			((Control)val2).set_Size(new Point(50, 26));
			((TextInputBase)val2).set_Font(GameService.Content.get_DefaultFont12());
			((Control)val2).set_BasicTooltipText($"Tempo in beats per minute ({20}-{300})");
			_bpmInput = val2;
			((TextInputBase)_bpmInput).add_TextChanged((EventHandler<EventArgs>)OnBpmTextChanged);
			((TextInputBase)_bpmInput).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)OnBpmInputFocusChanged);
			int noteX = 105;
			for (int i = 0; i < _noteTypes.Length; i++)
			{
				NoteType noteType = _noteTypes[i];
				bool isSelected = noteType == _selectedNoteType;
				string tooltipText = $"{noteType.GetDisplayName()} note ({noteType.GetDurationMs(_bpm)}ms @ {_bpm} BPM)";
				Panel val3 = new Panel();
				((Control)val3).set_Parent((Container)(object)this);
				((Control)val3).set_Location(new Point(noteX + i * 42, 0));
				((Control)val3).set_Size(new Point(40, 26));
				((Control)val3).set_BackgroundColor(isSelected ? _accentColor : MaestroTheme.GhostButtonBackground);
				((Control)val3).set_BasicTooltipText(tooltipText);
				Panel button = val3;
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)button);
				val4.set_Text(GetNoteButtonText(noteType));
				((Control)val4).set_Location(new Point(0, 0));
				((Control)val4).set_Size(new Point(40, 26));
				val4.set_Font(GameService.Content.get_DefaultFont12());
				val4.set_TextColor(isSelected ? MaestroTheme.DarkCharcoal : MaestroTheme.GhostButtonText);
				val4.set_HorizontalAlignment((HorizontalAlignment)1);
				val4.set_VerticalAlignment((VerticalAlignment)1);
				((Control)val4).set_BasicTooltipText(tooltipText);
				Label label = val4;
				int capturedIndex = i;
				NoteType capturedNoteType = noteType;
				((Control)button).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					//IL_0036: Unknown result type (might be due to invalid IL or missing references)
					bool flag2 = _noteTypes[capturedIndex] == _selectedNoteType;
					((Control)button).set_BackgroundColor(flag2 ? _accentColorDark : MaestroTheme.GhostButtonHover);
				});
				((Control)button).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					//IL_0036: Unknown result type (might be due to invalid IL or missing references)
					bool flag = _noteTypes[capturedIndex] == _selectedNoteType;
					((Control)button).set_BackgroundColor(flag ? _accentColor : MaestroTheme.GhostButtonBackground);
				});
				((Control)button).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					SelectedNoteType = capturedNoteType;
				});
				_noteButtons[i] = button;
				_noteLabels[i] = label;
			}
			int dottedX = noteX + _noteTypes.Length * 42 + 10;
			Panel val5 = new Panel();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Location(new Point(dottedX, 0));
			((Control)val5).set_Size(new Point(42, 26));
			((Control)val5).set_BackgroundColor(MaestroTheme.GhostButtonBackground);
			((Control)val5).set_BasicTooltipText("Dotted note (+50% duration)");
			_dottedButton = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)_dottedButton);
			val6.set_Text("Dot");
			((Control)val6).set_Location(new Point(0, 0));
			((Control)val6).set_Size(new Point(42, 26));
			val6.set_Font(GameService.Content.get_DefaultFont12());
			val6.set_TextColor(MaestroTheme.GhostButtonText);
			val6.set_HorizontalAlignment((HorizontalAlignment)1);
			val6.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val6).set_BasicTooltipText("Dotted note (+50% duration)");
			_dottedLabel = val6;
			((Control)_dottedButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				((Control)_dottedButton).set_BackgroundColor(_isDotted ? _accentColorDark : MaestroTheme.GhostButtonHover);
			});
			((Control)_dottedButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				((Control)_dottedButton).set_BackgroundColor(_isDotted ? _accentColor : MaestroTheme.GhostButtonBackground);
			});
			((Control)_dottedButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				IsDotted = !_isDotted;
			});
			UpdateNoteTypeSelection();
			UpdateTooltips();
		}

		private string GetNoteButtonText(NoteType noteType)
		{
			return noteType switch
			{
				NoteType.Whole => "1", 
				NoteType.Half => "1/2", 
				NoteType.Quarter => "1/4", 
				NoteType.Eighth => "1/8", 
				NoteType.Sixteenth => "1/16", 
				_ => "1/4", 
			};
		}

		private void OnBpmTextChanged(object sender, EventArgs e)
		{
			string text = ((TextInputBase)_bpmInput).get_Text();
			string numbersOnly = new string(text.Where(char.IsDigit).ToArray());
			int newBpm;
			if (numbersOnly != text)
			{
				((TextInputBase)_bpmInput).set_Text(numbersOnly);
			}
			else if (!string.IsNullOrEmpty(numbersOnly) && int.TryParse(numbersOnly, out newBpm))
			{
				_bpm = ClampBpm(newBpm);
				_lastValidBpmText = _bpm.ToString();
				UpdateTooltips();
				this.DurationChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		private void OnBpmInputFocusChanged(object sender, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				if (string.IsNullOrEmpty(((TextInputBase)_bpmInput).get_Text()) || !int.TryParse(((TextInputBase)_bpmInput).get_Text(), out var parsedBpm))
				{
					((TextInputBase)_bpmInput).set_Text(_lastValidBpmText);
					return;
				}
				_bpm = ClampBpm(parsedBpm);
				_lastValidBpmText = _bpm.ToString();
				((TextInputBase)_bpmInput).set_Text(_lastValidBpmText);
				UpdateTooltips();
				this.DurationChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		private void UpdateBpmDisplay()
		{
			if (_bpmInput != null && ((TextInputBase)_bpmInput).get_Text() != _bpm.ToString())
			{
				((TextInputBase)_bpmInput).set_Text(_bpm.ToString());
			}
			UpdateTooltips();
		}

		private void UpdateTooltips()
		{
			string dottedSuffix = (_isDotted ? ", dotted" : "");
			for (int i = 0; i < _noteTypes.Length; i++)
			{
				NoteType noteType = _noteTypes[i];
				int baseMs = noteType.GetDurationMs(_bpm);
				int effectiveMs = (_isDotted ? ((int)((double)baseMs * 1.5)) : baseMs);
				string tooltipText = $"{noteType.GetDisplayName()} note ({effectiveMs}ms @ {_bpm} BPM{dottedSuffix})";
				((Control)_noteButtons[i]).set_BasicTooltipText(tooltipText);
				((Control)_noteLabels[i]).set_BasicTooltipText(tooltipText);
			}
			if (_dottedButton != null)
			{
				int dottedMs = CurrentDurationMs;
				((Control)_dottedButton).set_BasicTooltipText($"Dotted note (+50% duration) — current: {dottedMs}ms");
				((Control)_dottedLabel).set_BasicTooltipText(((Control)_dottedButton).get_BasicTooltipText());
			}
		}

		private void UpdateDottedVisual()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			if (_dottedButton != null)
			{
				((Control)_dottedButton).set_BackgroundColor(_isDotted ? _accentColor : MaestroTheme.GhostButtonBackground);
				_dottedLabel.set_TextColor(_isDotted ? MaestroTheme.DarkCharcoal : MaestroTheme.CreamWhite);
				UpdateTooltips();
			}
		}

		private void UpdateNoteTypeSelection()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < _noteTypes.Length; i++)
			{
				bool isSelected = _noteTypes[i] == _selectedNoteType;
				((Control)_noteButtons[i]).set_BackgroundColor(isSelected ? _accentColor : MaestroTheme.GhostButtonBackground);
				_noteLabels[i].set_TextColor(isSelected ? MaestroTheme.DarkCharcoal : MaestroTheme.GhostButtonText);
			}
		}

		protected override void DisposeControl()
		{
			if (_bpmInput != null)
			{
				((TextInputBase)_bpmInput).remove_TextChanged((EventHandler<EventArgs>)OnBpmTextChanged);
				((TextInputBase)_bpmInput).remove_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)OnBpmInputFocusChanged);
			}
			Label bpmLabel = _bpmLabel;
			if (bpmLabel != null)
			{
				((Control)bpmLabel).Dispose();
			}
			TextBox bpmInput = _bpmInput;
			if (bpmInput != null)
			{
				((Control)bpmInput).Dispose();
			}
			Label[] noteLabels = _noteLabels;
			foreach (Label obj in noteLabels)
			{
				if (obj != null)
				{
					((Control)obj).Dispose();
				}
			}
			Panel[] noteButtons = _noteButtons;
			foreach (Panel obj2 in noteButtons)
			{
				if (obj2 != null)
				{
					((Control)obj2).Dispose();
				}
			}
			Label dottedLabel = _dottedLabel;
			if (dottedLabel != null)
			{
				((Control)dottedLabel).Dispose();
			}
			Panel dottedButton = _dottedButton;
			if (dottedButton != null)
			{
				((Control)dottedButton).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
