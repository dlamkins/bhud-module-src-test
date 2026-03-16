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

		public DurationSelector(int width)
			: this()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Expected O, but got Unknown
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Expected O, but got Unknown
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Expected O, but got Unknown
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Expected O, but got Unknown
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
			val.set_TextColor(MaestroTheme.CreamWhite);
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
				((Control)val3).set_BackgroundColor(isSelected ? MaestroTheme.AmberGold : MaestroTheme.ButtonBackground);
				((Control)val3).set_BasicTooltipText(tooltipText);
				Panel button = val3;
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)button);
				val4.set_Text(GetNoteButtonText(noteType));
				((Control)val4).set_Location(new Point(0, 0));
				((Control)val4).set_Size(new Point(40, 26));
				val4.set_Font(GameService.Content.get_DefaultFont12());
				val4.set_TextColor(isSelected ? MaestroTheme.DarkCharcoal : MaestroTheme.CreamWhite);
				val4.set_HorizontalAlignment((HorizontalAlignment)1);
				val4.set_VerticalAlignment((VerticalAlignment)1);
				((Control)val4).set_BasicTooltipText(tooltipText);
				Label label = val4;
				int capturedIndex = i;
				NoteType capturedNoteType = noteType;
				((Control)button).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					//IL_0030: Unknown result type (might be due to invalid IL or missing references)
					bool flag2 = _noteTypes[capturedIndex] == _selectedNoteType;
					((Control)button).set_BackgroundColor(flag2 ? MaestroTheme.DeepAmber : MaestroTheme.ButtonBackgroundHover);
				});
				((Control)button).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					//IL_0030: Unknown result type (might be due to invalid IL or missing references)
					bool flag = _noteTypes[capturedIndex] == _selectedNoteType;
					((Control)button).set_BackgroundColor(flag ? MaestroTheme.AmberGold : MaestroTheme.ButtonBackground);
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
			((Control)val5).set_BackgroundColor(MaestroTheme.ButtonBackground);
			((Control)val5).set_BasicTooltipText("Dotted note (+50% duration)");
			_dottedButton = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)_dottedButton);
			val6.set_Text("Dot");
			((Control)val6).set_Location(new Point(0, 0));
			((Control)val6).set_Size(new Point(42, 26));
			val6.set_Font(GameService.Content.get_DefaultFont12());
			val6.set_TextColor(MaestroTheme.CreamWhite);
			val6.set_HorizontalAlignment((HorizontalAlignment)1);
			val6.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val6).set_BasicTooltipText("Dotted note (+50% duration)");
			_dottedLabel = val6;
			((Control)_dottedButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				((Control)_dottedButton).set_BackgroundColor(_isDotted ? MaestroTheme.DeepAmber : MaestroTheme.ButtonBackgroundHover);
			});
			((Control)_dottedButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				((Control)_dottedButton).set_BackgroundColor(_isDotted ? MaestroTheme.AmberGold : MaestroTheme.ButtonBackground);
			});
			((Control)_dottedButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				IsDotted = !_isDotted;
			});
			UpdateNoteTypeSelection();
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
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (_dottedButton != null)
			{
				((Control)_dottedButton).set_BackgroundColor(_isDotted ? MaestroTheme.AmberGold : MaestroTheme.ButtonBackground);
				_dottedLabel.set_TextColor(_isDotted ? MaestroTheme.DarkCharcoal : MaestroTheme.CreamWhite);
				UpdateTooltips();
			}
		}

		private void UpdateNoteTypeSelection()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < _noteTypes.Length; i++)
			{
				bool isSelected = _noteTypes[i] == _selectedNoteType;
				((Control)_noteButtons[i]).set_BackgroundColor(isSelected ? MaestroTheme.AmberGold : MaestroTheme.ButtonBackground);
				_noteLabels[i].set_TextColor(isSelected ? MaestroTheme.DarkCharcoal : MaestroTheme.CreamWhite);
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
