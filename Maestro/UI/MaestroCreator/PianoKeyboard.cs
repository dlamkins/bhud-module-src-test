using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

		private class RoundedKeyPanel : Panel
		{
			private Color _fillColor;

			public Color FillColor
			{
				get
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					return _fillColor;
				}
				set
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					_fillColor = value;
					((Control)this).Invalidate();
				}
			}

			public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0004: Unknown result type (might be due to invalid IL or missing references)
				MaestroTheme.DrawBottomRoundedRect(spriteBatch, (Control)(object)this, bounds, _fillColor);
			}

			public RoundedKeyPanel()
				: this()
			{
			}
		}

		private InstrumentType _instrument;

		private int _minOctave = -1;

		private int _maxOctave = 1;

		private bool _sharpsEnabled = true;

		private int _currentOctave;

		private readonly Label _octaveLabel;

		private readonly Panel _octaveDownButton;

		private readonly Label _octaveDownLabel;

		private readonly Panel _octaveUpButton;

		private readonly Label _octaveUpLabel;

		private readonly Panel _restButton;

		private readonly Label _restLabel;

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

		private Color _accentColor = MaestroTheme.AmberGold;

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
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Expected O, but got Unknown
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Expected O, but got Unknown
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Expected O, but got Unknown
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Expected O, but got Unknown
			//IL_0412: Unknown result type (might be due to invalid IL or missing references)
			//IL_0417: Unknown result type (might be due to invalid IL or missing references)
			//IL_041e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_0432: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Expected O, but got Unknown
			//IL_0452: Unknown result type (might be due to invalid IL or missing references)
			//IL_0457: Unknown result type (might be due to invalid IL or missing references)
			//IL_0463: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0472: Unknown result type (might be due to invalid IL or missing references)
			//IL_047c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Expected O, but got Unknown
			((Control)this).set_Size(new Point(containerWidth, Layout.TotalHeight));
			((Control)this).set_BackgroundColor(new Color(0, 0, 0, 65));
			_whiteKeyPanels = (Panel[])(object)new Panel[8];
			_blackKeyPanels = (Panel[])(object)new Panel[5];
			_keysOffsetX = (containerWidth - Layout.KeysWidth) / 2;
			int centerX = containerWidth / 2;
			int octaveY = 5;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(centerX - 110, octaveY));
			((Control)val).set_Size(new Point(30, 26));
			((Control)val).set_BackgroundColor(MaestroTheme.GhostButtonBackground);
			_octaveDownButton = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_octaveDownButton);
			val2.set_Text("-");
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Size(new Point(30, 26));
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(_accentColor);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			_octaveDownLabel = val2;
			((Control)_octaveDownButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)_octaveDownButton).set_BackgroundColor(MaestroTheme.GhostButtonHover);
			});
			((Control)_octaveDownButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)_octaveDownButton).set_BackgroundColor(MaestroTheme.GhostButtonBackground);
			});
			((Control)_octaveDownButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				if (_octaveButtonsEnabled && _currentOctave > _minOctave)
				{
					CurrentOctave--;
					this.OctaveChanged?.Invoke(this, e: false);
				}
			});
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Octave: Middle");
			((Control)val3).set_Location(new Point(centerX - 75, octaveY));
			((Control)val3).set_Size(new Point(150, 30));
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(MaestroTheme.OctaveLabelColor);
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			_octaveLabel = val3;
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(centerX + 110 - 30, octaveY));
			((Control)val4).set_Size(new Point(30, 26));
			((Control)val4).set_BackgroundColor(MaestroTheme.GhostButtonBackground);
			_octaveUpButton = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)_octaveUpButton);
			val5.set_Text("+");
			((Control)val5).set_Location(new Point(0, 0));
			((Control)val5).set_Size(new Point(30, 26));
			val5.set_Font(GameService.Content.get_DefaultFont16());
			val5.set_TextColor(_accentColor);
			val5.set_HorizontalAlignment((HorizontalAlignment)1);
			val5.set_VerticalAlignment((VerticalAlignment)1);
			_octaveUpLabel = val5;
			((Control)_octaveUpButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)_octaveUpButton).set_BackgroundColor(MaestroTheme.GhostButtonHover);
			});
			((Control)_octaveUpButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)_octaveUpButton).set_BackgroundColor(MaestroTheme.GhostButtonBackground);
			});
			((Control)_octaveUpButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				if (_octaveButtonsEnabled && _currentOctave < _maxOctave)
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
			Panel val6 = new Panel();
			((Control)val6).set_Parent((Container)(object)this);
			((Control)val6).set_Location(new Point(_keysOffsetX + 5, restY));
			((Control)val6).set_Size(new Point(60, 26));
			((Control)val6).set_BackgroundColor(MaestroTheme.GhostButtonBackground);
			_restButton = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)_restButton);
			val7.set_Text("REST");
			((Control)val7).set_Location(new Point(0, -2));
			((Control)val7).set_Size(new Point(60, 26));
			val7.set_Font(GameService.Content.get_DefaultFont12());
			val7.set_TextColor(MaestroTheme.GhostButtonText);
			val7.set_HorizontalAlignment((HorizontalAlignment)1);
			val7.set_VerticalAlignment((VerticalAlignment)1);
			_restLabel = val7;
			((Control)_restButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)_restButton).set_BackgroundColor(MaestroTheme.GhostButtonHover);
			});
			((Control)_restButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)_restButton).set_BackgroundColor(MaestroTheme.GhostButtonBackground);
			});
			((Control)_restButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				this.NotePressed?.Invoke(this, new NoteEventArgs("R", isSharp: false, isHighC: false, isRest: true));
			});
		}

		private Panel CreateWhiteKey(int index, string note, bool isHighC)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			RoundedKeyPanel roundedKeyPanel = new RoundedKeyPanel();
			((Control)roundedKeyPanel).set_Parent((Container)(object)this);
			((Control)roundedKeyPanel).set_Location(new Point(_keysOffsetX + 5 + index * 45, _keysY));
			((Control)roundedKeyPanel).set_Size(new Point(43, 90));
			((Control)roundedKeyPanel).set_BackgroundColor(Color.get_Transparent());
			roundedKeyPanel.FillColor = MaestroTheme.PianoWhiteKey;
			((Control)roundedKeyPanel).set_ZIndex(0);
			RoundedKeyPanel keyPanel = roundedKeyPanel;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)keyPanel);
			val.set_Text(isHighC ? "C^" : note);
			((Control)val).set_Location(new Point(0, 65));
			((Control)val).set_Size(new Point(43, 20));
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.DarkCharcoal);
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)keyPanel).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				keyPanel.FillColor = MaestroTheme.PianoWhiteKeyHover;
			});
			((Control)keyPanel).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				keyPanel.FillColor = MaestroTheme.PianoWhiteKey;
			});
			((Control)keyPanel).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				keyPanel.FillColor = MaestroTheme.PianoWhiteKeyPressed;
			});
			((Control)keyPanel).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				keyPanel.FillColor = MaestroTheme.PianoWhiteKeyHover;
				this.NotePressed?.Invoke(this, new NoteEventArgs(note.Replace("^", ""), isSharp: false, isHighC));
			});
			return (Panel)(object)keyPanel;
		}

		private Panel CreateBlackKey(int index, string note, int afterWhiteKey)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			int xPos = _keysOffsetX + 5 + (afterWhiteKey + 1) * 45 - 15 - 1;
			RoundedKeyPanel roundedKeyPanel = new RoundedKeyPanel();
			((Control)roundedKeyPanel).set_Parent((Container)(object)this);
			((Control)roundedKeyPanel).set_Location(new Point(xPos, _keysY));
			((Control)roundedKeyPanel).set_Size(new Point(30, 55));
			((Control)roundedKeyPanel).set_BackgroundColor(Color.get_Transparent());
			roundedKeyPanel.FillColor = MaestroTheme.PianoBlackKey;
			((Control)roundedKeyPanel).set_ZIndex(10);
			RoundedKeyPanel keyPanel = roundedKeyPanel;
			string baseNote = note.Replace("#", "");
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)keyPanel);
			val.set_Text(note);
			((Control)val).set_Location(new Point(0, 35));
			((Control)val).set_Size(new Point(30, 18));
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.CreamWhite);
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)keyPanel).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				keyPanel.FillColor = MaestroTheme.PianoBlackKeyHover;
			});
			((Control)keyPanel).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				keyPanel.FillColor = MaestroTheme.PianoBlackKey;
			});
			((Control)keyPanel).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				keyPanel.FillColor = MaestroTheme.PianoBlackKeyPressed;
			});
			((Control)keyPanel).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				keyPanel.FillColor = MaestroTheme.PianoBlackKeyHover;
				this.NotePressed?.Invoke(this, new NoteEventArgs(baseNote, isSharp: true));
			});
			return (Panel)(object)keyPanel;
		}

		public void SetOctaveButtonsEnabled(bool enabled)
		{
			_octaveButtonsEnabled = enabled;
			UpdateOctaveDisplay();
		}

		public void Configure(InstrumentType instrument)
		{
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
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
			_accentColor = MaestroTheme.GetInstrumentAccent(instrument);
			UpdateOctaveDisplay();
		}

		private void UpdateOctaveDisplay()
		{
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
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
				bool downEnabled = _octaveButtonsEnabled && _currentOctave > _minOctave;
				((Control)_octaveDownButton).set_Opacity(downEnabled ? 1f : 0.3f);
				_octaveDownLabel.set_TextColor(downEnabled ? _accentColor : MaestroTheme.Disabled);
			}
			if (_octaveUpButton != null)
			{
				bool upEnabled = _octaveButtonsEnabled && _currentOctave < _maxOctave;
				((Control)_octaveUpButton).set_Opacity(upEnabled ? 1f : 0.3f);
				_octaveUpLabel.set_TextColor(upEnabled ? _accentColor : MaestroTheme.Disabled);
			}
		}

		protected override void DisposeControl()
		{
			Label octaveLabel = _octaveLabel;
			if (octaveLabel != null)
			{
				((Control)octaveLabel).Dispose();
			}
			Label octaveDownLabel = _octaveDownLabel;
			if (octaveDownLabel != null)
			{
				((Control)octaveDownLabel).Dispose();
			}
			Panel octaveDownButton = _octaveDownButton;
			if (octaveDownButton != null)
			{
				((Control)octaveDownButton).Dispose();
			}
			Label octaveUpLabel = _octaveUpLabel;
			if (octaveUpLabel != null)
			{
				((Control)octaveUpLabel).Dispose();
			}
			Panel octaveUpButton = _octaveUpButton;
			if (octaveUpButton != null)
			{
				((Control)octaveUpButton).Dispose();
			}
			Label restLabel = _restLabel;
			if (restLabel != null)
			{
				((Control)restLabel).Dispose();
			}
			Panel restButton = _restButton;
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
