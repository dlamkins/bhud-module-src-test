using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Microsoft.Xna.Framework;

namespace Maestro.UI.MaestroCreator
{
	public class PianoKeyboard : Panel
	{
		public static class Layout
		{
			public const int WhiteKeyWidth = 45;

			public const int WhiteKeyHeight = 90;

			public const int BlackKeyWidth = 30;

			public const int BlackKeyHeight = 55;

			public const int OctaveControlHeight = 30;

			public const int RestButtonHeight = 26;

			public const int Spacing = 5;

			public const int RestSpacing = 12;

			public const int BottomPadding = 16;

			public const int OctaveButtonWidth = 30;

			public const int OctaveButtonOffset = 110;

			public const int OctaveLabelWidth = 150;

			public const int OctaveLabelOffset = 75;

			public const int WhiteKeyGap = 2;

			public static int KeysWidth => 370;

			public static int TotalHeight => 179;
		}

		private InstrumentType _instrument;

		private int _minOctave = -1;

		private int _maxOctave = 1;

		private bool _sharpsEnabled = true;

		private int _currentOctave;

		private readonly Label _octaveLabel;

		private readonly StandardButton _octaveDownButton;

		private readonly StandardButton _octaveUpButton;

		private readonly StandardButton _restButton;

		private readonly string[] _whiteNotes = new string[8] { "C", "D", "E", "F", "G", "A", "B", "C^" };

		private readonly (string note, int afterWhiteKey)[] _blackKeys = new(string, int)[5]
		{
			("C#", 0),
			("D#", 1),
			("F#", 3),
			("G#", 4),
			("A#", 5)
		};

		private readonly Panel[] _whiteKeyPanels;

		private readonly Panel[] _blackKeyPanels;

		private readonly int _keysY;

		private readonly int _keysOffsetX;

		private bool _octaveButtonsEnabled = true;

		public int CurrentOctave
		{
			get
			{
				return _currentOctave;
			}
			set
			{
				_currentOctave = ((value < _minOctave) ? _minOctave : ((value > _maxOctave) ? _maxOctave : value));
				UpdateOctaveDisplay();
			}
		}

		public event EventHandler<NoteEventArgs> NotePressed;

		public event EventHandler<bool> OctaveChanged;

		public PianoKeyboard(int containerWidth)
			: this()
		{
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Expected O, but got Unknown
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Expected O, but got Unknown
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Expected O, but got Unknown
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Expected O, but got Unknown
			((Control)this).set_Size(new Point(containerWidth, Layout.TotalHeight));
			((Control)this).set_BackgroundColor(MaestroTheme.PanelBackground);
			_whiteKeyPanels = (Panel[])(object)new Panel[8];
			_blackKeyPanels = (Panel[])(object)new Panel[5];
			_keysOffsetX = (containerWidth - Layout.KeysWidth) / 2;
			int centerX = containerWidth / 2;
			int octaveY = 5;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("-");
			((Control)val).set_Location(new Point(centerX - 110, octaveY));
			((Control)val).set_Size(new Point(30, 26));
			_octaveDownButton = val;
			((Control)_octaveDownButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_currentOctave > _minOctave)
				{
					CurrentOctave--;
					this.OctaveChanged?.Invoke(this, e: false);
				}
			});
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Octave: Middle");
			((Control)val2).set_Location(new Point(centerX - 75, octaveY));
			((Control)val2).set_Size(new Point(150, 30));
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(MaestroTheme.CreamWhite);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			_octaveLabel = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("+");
			((Control)val3).set_Location(new Point(centerX + 110 - 30, octaveY));
			((Control)val3).set_Size(new Point(30, 26));
			_octaveUpButton = val3;
			((Control)_octaveUpButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_currentOctave < _maxOctave)
				{
					CurrentOctave++;
					this.OctaveChanged?.Invoke(this, e: true);
				}
			});
			_keysY = octaveY + 30 + 5;
			for (int j = 0; j < _whiteNotes.Length; j++)
			{
				string note = _whiteNotes[j];
				bool isHighC = note == "C^";
				Panel keyPanel = CreateWhiteKey(j, note, isHighC);
				_whiteKeyPanels[j] = keyPanel;
			}
			for (int i = 0; i < _blackKeys.Length; i++)
			{
				(string note, int afterWhiteKey) tuple = _blackKeys[i];
				string note2 = tuple.note;
				int afterWhiteKey = tuple.afterWhiteKey;
				Panel keyPanel2 = CreateBlackKey(i, note2, afterWhiteKey);
				_blackKeyPanels[i] = keyPanel2;
			}
			int restY = _keysY + 90 + 12;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("REST");
			((Control)val4).set_Location(new Point(_keysOffsetX + 5, restY));
			((Control)val4).set_Size(new Point(60, 26));
			_restButton = val4;
			((Control)_restButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.NotePressed?.Invoke(this, new NoteEventArgs("R", isSharp: false, isHighC: false, isRest: true));
			});
		}

		private Panel CreateWhiteKey(int index, string note, bool isHighC)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(_keysOffsetX + 5 + index * 45, _keysY));
			((Control)val).set_Size(new Point(43, 90));
			((Control)val).set_BackgroundColor(MaestroTheme.PianoWhiteKey);
			((Control)val).set_ZIndex(0);
			Panel keyPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)keyPanel);
			val2.set_Text(isHighC ? "C^" : note);
			((Control)val2).set_Location(new Point(0, 65));
			((Control)val2).set_Size(new Point(43, 20));
			val2.set_Font(GameService.Content.get_DefaultFont12());
			val2.set_TextColor(MaestroTheme.DarkCharcoal);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)keyPanel).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)keyPanel).set_BackgroundColor(MaestroTheme.PianoWhiteKeyHover);
			});
			((Control)keyPanel).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)keyPanel).set_BackgroundColor(MaestroTheme.PianoWhiteKey);
			});
			((Control)keyPanel).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)keyPanel).set_BackgroundColor(MaestroTheme.PianoWhiteKeyPressed);
			});
			((Control)keyPanel).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)keyPanel).set_BackgroundColor(MaestroTheme.PianoWhiteKeyHover);
				this.NotePressed?.Invoke(this, new NoteEventArgs(note.Replace("^", ""), isSharp: false, isHighC));
			});
			return keyPanel;
		}

		private Panel CreateBlackKey(int index, string note, int afterWhiteKey)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			int xPos = _keysOffsetX + 5 + (afterWhiteKey + 1) * 45 - 15 - 1;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(xPos, _keysY));
			((Control)val).set_Size(new Point(30, 55));
			((Control)val).set_BackgroundColor(MaestroTheme.PianoBlackKey);
			((Control)val).set_ZIndex(10);
			Panel keyPanel = val;
			string baseNote = note.Replace("#", "");
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)keyPanel);
			val2.set_Text(note);
			((Control)val2).set_Location(new Point(0, 35));
			((Control)val2).set_Size(new Point(30, 18));
			val2.set_Font(GameService.Content.get_DefaultFont12());
			val2.set_TextColor(MaestroTheme.CreamWhite);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)keyPanel).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)keyPanel).set_BackgroundColor(MaestroTheme.PianoBlackKeyHover);
			});
			((Control)keyPanel).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)keyPanel).set_BackgroundColor(MaestroTheme.PianoBlackKey);
			});
			((Control)keyPanel).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)keyPanel).set_BackgroundColor(MaestroTheme.PianoBlackKeyPressed);
			});
			((Control)keyPanel).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)keyPanel).set_BackgroundColor(MaestroTheme.PianoBlackKeyHover);
				this.NotePressed?.Invoke(this, new NoteEventArgs(baseNote, isSharp: true));
			});
			return keyPanel;
		}

		public void SetOctaveButtonsEnabled(bool enabled)
		{
			_octaveButtonsEnabled = enabled;
			UpdateOctaveDisplay();
		}

		public void Configure(InstrumentType instrument)
		{
			_instrument = instrument;
			switch (instrument)
			{
			case InstrumentType.Piano:
				_sharpsEnabled = true;
				_minOctave = -1;
				_maxOctave = 1;
				break;
			case InstrumentType.Harp:
			case InstrumentType.Lute:
				_sharpsEnabled = false;
				_minOctave = -1;
				_maxOctave = 1;
				break;
			case InstrumentType.Bass:
				_sharpsEnabled = false;
				_minOctave = 0;
				_maxOctave = 1;
				break;
			default:
				_sharpsEnabled = true;
				_minOctave = -1;
				_maxOctave = 1;
				break;
			}
			Panel[] blackKeyPanels = _blackKeyPanels;
			foreach (Panel key in blackKeyPanels)
			{
				if (key != null)
				{
					((Control)key).set_Visible(_sharpsEnabled);
				}
			}
			_currentOctave = ((_instrument == InstrumentType.Bass) ? 0 : 0);
			UpdateOctaveDisplay();
		}

		private void UpdateOctaveDisplay()
		{
			if (_octaveLabel != null)
			{
				string octaveName = ((_instrument == InstrumentType.Bass) ? ((_currentOctave == 0) ? "Low" : "High") : (_currentOctave switch
				{
					-1 => "Lower (-)", 
					1 => "Upper (+)", 
					_ => "Middle", 
				}));
				_octaveLabel.set_Text("Octave: " + octaveName);
			}
			if (_octaveDownButton != null)
			{
				((Control)_octaveDownButton).set_Enabled(_octaveButtonsEnabled && _currentOctave > _minOctave);
			}
			if (_octaveUpButton != null)
			{
				((Control)_octaveUpButton).set_Enabled(_octaveButtonsEnabled && _currentOctave < _maxOctave);
			}
		}

		protected override void DisposeControl()
		{
			Label octaveLabel = _octaveLabel;
			if (octaveLabel != null)
			{
				((Control)octaveLabel).Dispose();
			}
			StandardButton octaveDownButton = _octaveDownButton;
			if (octaveDownButton != null)
			{
				((Control)octaveDownButton).Dispose();
			}
			StandardButton octaveUpButton = _octaveUpButton;
			if (octaveUpButton != null)
			{
				((Control)octaveUpButton).Dispose();
			}
			StandardButton restButton = _restButton;
			if (restButton != null)
			{
				((Control)restButton).Dispose();
			}
			Panel[] whiteKeyPanels = _whiteKeyPanels;
			foreach (Panel obj in whiteKeyPanels)
			{
				if (obj != null)
				{
					((Control)obj).Dispose();
				}
			}
			whiteKeyPanels = _blackKeyPanels;
			foreach (Panel obj2 in whiteKeyPanels)
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
