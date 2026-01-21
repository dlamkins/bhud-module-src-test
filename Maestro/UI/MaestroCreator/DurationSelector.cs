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

		public int CurrentDurationMs => _selectedNoteType.GetDurationMs(_bpm);

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
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Expected O, but got Unknown
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Expected O, but got Unknown
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
			for (int i = 0; i < _noteTypes.Length; i++)
			{
				NoteType noteType = _noteTypes[i];
				string tooltipText = $"{noteType.GetDisplayName()} note ({noteType.GetDurationMs(_bpm)}ms @ {_bpm} BPM)";
				((Control)_noteButtons[i]).set_BasicTooltipText(tooltipText);
				((Control)_noteLabels[i]).set_BasicTooltipText(tooltipText);
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
			((Panel)this).DisposeControl();
		}
	}
}
