using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Data;
using Maestro.Services.Playback;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.MaestroCreator
{
	public class MaestroCreatorWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 420;

			public const int WindowHeight = 408;

			public const int ContentWidth = 390;

			public const int ContentLeftInset = 15;

			public const int ContentHeight = 398;

			public const int RowHeight = 28;

			public const int ChordBarHeight = 30;

			public const int TitleInputX = 45;

			public const int TitleInputWidth = 345;

			public const int ArtistInputX = 48;

			public const int ArtistInputWidth = 140;

			public const int ByLabelX = 200;

			public const int ByInputX = 228;

			public const int ByInputWidth = 162;

			public const int ActionButtonWidth = 90;

			public const int ActionButtonHeight = 30;

			public const int ActionButtonSpacing = 12;

			public const int ChordPreviewMaxLength = 38;

			public const int LabelYOffset = 5;

			public const int MaxChordNotes = 7;
		}

		private readonly TextBox _titleInput;

		private readonly TextBox _artistInput;

		private readonly TextBox _transcriberInput;

		private readonly PianoKeyboard _pianoKeyboard;

		private readonly DrumPadPanel _drumPadPanel;

		private readonly DurationSelector _durationSelector;

		private readonly NoteSequencePanel _noteSequencePanel;

		private readonly StandardButton _saveButton;

		private readonly StandardButton _cancelButton;

		private readonly StandardButton _chordModeButton;

		private readonly Label _chordPreviewLabel;

		private readonly StandardButton _addChordButton;

		private bool _isChordMode;

		private readonly List<string> _pendingChordNotes = new List<string>();

		private readonly List<Action> _pendingChordPreviews = new List<Action>();

		private InstrumentType _instrument;

		private Song _editingSong;

		private NoteSequenceWindow _noteSequenceWindow;

		private readonly SongPlayer _creatorPlayer;

		private bool _isPreviewActive;

		private readonly Panel _confirmationOverlay;

		private readonly Label _confirmationLabel;

		private readonly StandardButton _readyButton;

		private bool _isWaitingForConfirmation;

		private static Texture2D _backgroundTexture;

		private bool IsPercussion => InstrumentCatalog.Get(_instrument).IsPercussion;

		public event EventHandler<Song> SongCreated;

		public event EventHandler<Song> SongEdited;

		public event EventHandler WindowClosed;

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateCreatorBackground(420, 408));
		}

		private static string ShortInstrumentName(InstrumentType instrument)
		{
			if (instrument != InstrumentType.Bell && instrument != InstrumentType.BellMagnanimous)
			{
				return InstrumentCatalog.Get(instrument).DisplayName;
			}
			return "Bell";
		}

		private void ConfigureInputPanel()
		{
			if (IsPercussion)
			{
				((Control)_pianoKeyboard).set_Visible(false);
				((Control)_drumPadPanel).set_Visible(true);
				_drumPadPanel.Configure(_instrument);
			}
			else
			{
				((Control)_drumPadPanel).set_Visible(false);
				((Control)_pianoKeyboard).set_Visible(true);
				_pianoKeyboard.Configure(_instrument);
			}
		}

		public void SetInstrument(InstrumentType instrument)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			_instrument = instrument;
			((WindowBase2)this).set_Subtitle(ShortInstrumentName(instrument));
			ConfigureInputPanel();
			_durationSelector.SetAccentColor(instrument);
			if (_isChordMode)
			{
				((Control)_chordModeButton).set_BackgroundColor(MaestroTheme.GetInstrumentAccent(instrument));
			}
		}

		public void LoadSong(Song song)
		{
			_editingSong = song;
			_instrument = song.Instrument;
			SetInstrument(song.Instrument);
			((TextInputBase)_titleInput).set_Text(song.Name ?? string.Empty);
			((TextInputBase)_artistInput).set_Text(song.Artist ?? string.Empty);
			((TextInputBase)_transcriberInput).set_Text(song.Transcriber ?? string.Empty);
			if (song.Bpm.HasValue)
			{
				_durationSelector.Bpm = song.Bpm.Value;
			}
			_noteSequencePanel.Clear();
			foreach (string note in song.Notes)
			{
				_noteSequencePanel.AddNote(note);
			}
		}

		public override void Show()
		{
			((WindowBase2)this).Show();
			OpenNotesWindow();
			ConfigureInputPanel();
			if (_editingSong != null)
			{
				_isWaitingForConfirmation = false;
				((Control)_confirmationOverlay).set_Visible(false);
				if (!IsPercussion)
				{
					_pianoKeyboard.SetOctaveButtonsEnabled(enabled: true);
				}
				return;
			}
			_isWaitingForConfirmation = true;
			_confirmationLabel.set_Text("Equip your " + ShortInstrumentName(_instrument) + " and click Ready");
			((Control)_confirmationOverlay).set_Visible(true);
			if (!IsPercussion)
			{
				_pianoKeyboard.SetOctaveButtonsEnabled(enabled: false);
			}
		}

		private void OpenNotesWindow()
		{
			if (_noteSequenceWindow == null)
			{
				((Control)_noteSequencePanel).set_Parent((Container)null);
				_noteSequenceWindow = new NoteSequenceWindow(_noteSequencePanel);
				((Control)_noteSequenceWindow).Show();
			}
		}

		private void CloseNotesWindow()
		{
			if (_noteSequenceWindow != null)
			{
				_noteSequenceWindow.DetachPanel();
				_noteSequenceWindow.CloseProgrammatic();
				((Control)_noteSequenceWindow).Dispose();
				_noteSequenceWindow = null;
			}
		}

		private void OnReadyClicked(object sender, MouseEventArgs e)
		{
			if (!_isWaitingForConfirmation)
			{
				return;
			}
			_isWaitingForConfirmation = false;
			((Control)_confirmationOverlay).set_Visible(false);
			_chordPreviewLabel.set_Text("Resetting octave...");
			if (!IsPercussion)
			{
				if (InstrumentCatalog.Get(_instrument).MinOctave == 0)
				{
					ResetToLowOctave();
				}
				else
				{
					Module.Instance.ResetToMiddleOctave();
				}
			}
			if (!IsPercussion)
			{
				_pianoKeyboard.SetOctaveButtonsEnabled(enabled: true);
			}
			_chordPreviewLabel.set_Text("");
		}

		private void ResetToLowOctave()
		{
			Module keyboardService = Module.Instance;
			for (int i = 0; i < 5; i++)
			{
				keyboardService.PlayOctaveChange(up: false);
				Thread.Sleep(150);
			}
		}

		public MaestroCreatorWindow()
			: this(GetBackground(), new Rectangle(0, 0, 420, 408), new Rectangle(15, 20, 390, 398))
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Expected O, but got Unknown
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Expected O, but got Unknown
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Expected O, but got Unknown
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Expected O, but got Unknown
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Expected O, but got Unknown
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Expected O, but got Unknown
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Expected O, but got Unknown
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Expected O, but got Unknown
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Unknown result type (might be due to invalid IL or missing references)
			//IL_052d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0538: Unknown result type (might be due to invalid IL or missing references)
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_054b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0555: Unknown result type (might be due to invalid IL or missing references)
			//IL_055d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Expected O, but got Unknown
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			//IL_056f: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0586: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_059c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_05be: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d5: Expected O, but got Unknown
			//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05db: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0601: Unknown result type (might be due to invalid IL or missing references)
			//IL_060b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0610: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Expected O, but got Unknown
			((WindowBase2)this).set_Title("Maestro Creator");
			((WindowBase2)this).set_Emblem(Module.Instance.ContentsManager.GetTexture("creator-emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("MaestroCreatorWindow_v2");
			((WindowBase2)this).set_CanResize(false);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			int currentY = 2;
			CreateLabel("Title:", 0, currentY);
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(45, currentY));
			((Control)val).set_Width(345);
			((TextInputBase)val).set_PlaceholderText("Song title");
			_titleInput = val;
			currentY += 35;
			CreateLabel("Artist:", 0, currentY);
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(48, currentY));
			((Control)val2).set_Width(140);
			((TextInputBase)val2).set_PlaceholderText("Artist");
			_artistInput = val2;
			CreateLabel("By:", 200, currentY);
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(228, currentY));
			((Control)val3).set_Width(162);
			((TextInputBase)val3).set_PlaceholderText("Your name");
			_transcriberInput = val3;
			currentY += 42;
			PianoKeyboard pianoKeyboard = new PianoKeyboard(390);
			((Control)pianoKeyboard).set_Parent((Container)(object)this);
			((Control)pianoKeyboard).set_Location(new Point(0, currentY));
			((Panel)pianoKeyboard).set_ShowBorder(true);
			_pianoKeyboard = pianoKeyboard;
			_pianoKeyboard.NotePressed += OnNotePressed;
			_pianoKeyboard.OctaveChanged += OnOctaveChanged;
			DrumPadPanel drumPadPanel = new DrumPadPanel(390);
			((Control)drumPadPanel).set_Parent((Container)(object)this);
			((Control)drumPadPanel).set_Location(new Point(0, currentY));
			((Control)drumPadPanel).set_Visible(false);
			((Panel)drumPadPanel).set_ShowBorder(true);
			_drumPadPanel = drumPadPanel;
			_drumPadPanel.PadPressed += OnDrumPadPressed;
			_drumPadPanel.RestPressed += OnDrumRestPressed;
			currentY += PianoKeyboard.Layout.TotalHeight + 7;
			DurationSelector durationSelector = new DurationSelector(390);
			((Control)durationSelector).set_Parent((Container)(object)this);
			((Control)durationSelector).set_Location(new Point(0, currentY));
			_durationSelector = durationSelector;
			currentY += 37;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Chord");
			((Control)val4).set_Location(new Point(0, currentY));
			((Control)val4).set_Size(new Point(65, 26));
			((Control)val4).set_BasicTooltipText("Toggle chord mode to add multiple notes at once");
			_chordModeButton = val4;
			((Control)_chordModeButton).add_Click((EventHandler<MouseEventArgs>)OnChordModeToggle);
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("");
			((Control)val5).set_Location(new Point(85, currentY));
			((Control)val5).set_Size(new Point(220, 26));
			val5.set_Font(GameService.Content.get_DefaultFont12());
			val5.set_TextColor(MaestroTheme.AmberGold);
			val5.set_VerticalAlignment((VerticalAlignment)1);
			_chordPreviewLabel = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Add Chord");
			((Control)val6).set_Location(new Point(310, currentY));
			((Control)val6).set_Size(new Point(80, 26));
			((Control)val6).set_Enabled(false);
			((Control)val6).set_BasicTooltipText("Add the current chord to the sequence");
			_addChordButton = val6;
			((Control)_addChordButton).add_Click((EventHandler<MouseEventArgs>)OnAddChordClicked);
			currentY += 44;
			int totalButtonsWidth = 192;
			int buttonsStartX = (390 - totalButtonsWidth) / 2;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text("Save");
			((Control)val7).set_Location(new Point(buttonsStartX, currentY));
			((Control)val7).set_Size(new Point(90, 30));
			_saveButton = val7;
			((Control)_saveButton).add_Click((EventHandler<MouseEventArgs>)OnSaveClicked);
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text("Cancel");
			((Control)val8).set_Location(new Point(buttonsStartX + 90 + 12, currentY));
			((Control)val8).set_Size(new Point(90, 30));
			_cancelButton = val8;
			((Control)_cancelButton).add_Click((EventHandler<MouseEventArgs>)OnCancelClicked);
			_creatorPlayer = new SongPlayer(Module.Instance.KeyboardService);
			_noteSequencePanel = new NoteSequencePanel(670, 400);
			_noteSequencePanel.PreviewAllRequested += OnPreviewClicked;
			_noteSequencePanel.PreviewSelectionRequested += OnPreviewSelectionClicked;
			_noteSequencePanel.PauseRequested += OnPauseClicked;
			_noteSequencePanel.StopRequested += OnStopClicked;
			_noteSequencePanel.InsertModeChanged += OnNoteSequenceStateChanged;
			_noteSequencePanel.ReplaceModeChanged += OnNoteSequenceStateChanged;
			_noteSequencePanel.SelectionChanged += OnNoteSequenceStateChanged;
			Panel val9 = new Panel();
			((Control)val9).set_Parent((Container)(object)this);
			((Control)val9).set_Location(new Point(0, ((Control)_pianoKeyboard).get_Location().Y));
			((Control)val9).set_Size(new Point(390, PianoKeyboard.Layout.TotalHeight));
			((Control)val9).set_BackgroundColor(new Color(0, 0, 0, 200));
			((Control)val9).set_ZIndex(100);
			((Control)val9).set_Visible(false);
			_confirmationOverlay = val9;
			Label val10 = new Label();
			((Control)val10).set_Parent((Container)(object)_confirmationOverlay);
			val10.set_Text("Equip your instrument and click Ready");
			((Control)val10).set_Location(new Point(0, PianoKeyboard.Layout.TotalHeight / 2 - 30));
			((Control)val10).set_Size(new Point(390, 30));
			val10.set_Font(GameService.Content.get_DefaultFont16());
			val10.set_TextColor(MaestroTheme.CreamWhite);
			val10.set_HorizontalAlignment((HorizontalAlignment)1);
			_confirmationLabel = val10;
			StandardButton val11 = new StandardButton();
			((Control)val11).set_Parent((Container)(object)_confirmationOverlay);
			val11.set_Text("Ready");
			((Control)val11).set_Location(new Point(145, PianoKeyboard.Layout.TotalHeight / 2 + 5));
			((Control)val11).set_Size(new Point(100, 30));
			_readyButton = val11;
			((Control)_readyButton).add_Click((EventHandler<MouseEventArgs>)OnReadyClicked);
		}

		private Label CreateLabel(string text, int x, int y)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(text);
			((Control)val).set_Location(new Point(x, y + 5));
			val.set_AutoSizeWidth(true);
			val.set_TextColor(MaestroTheme.InputLabelColor);
			return val;
		}

		private void OnChordModeToggle(object sender, MouseEventArgs e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			_isChordMode = !_isChordMode;
			Color accent = MaestroTheme.GetInstrumentAccent(_instrument);
			((Control)_chordModeButton).set_BackgroundColor(_isChordMode ? accent : Color.get_Transparent());
			if (!_isChordMode && _pendingChordNotes.Count > 0)
			{
				_pendingChordNotes.Clear();
				_pendingChordPreviews.Clear();
			}
			UpdateChordPreview();
		}

		private void OnAddChordClicked(object sender, MouseEventArgs e)
		{
			if (_pendingChordNotes.Count > 0)
			{
				AddPendingChord();
			}
		}

		private void AddPendingChord()
		{
			if (_pendingChordNotes.Count == 0)
			{
				return;
			}
			foreach (Action pendingChordPreview in _pendingChordPreviews)
			{
				pendingChordPreview?.Invoke();
			}
			string chordString = string.Join(" ", _pendingChordNotes);
			if (_noteSequencePanel.IsReplaceMode)
			{
				_noteSequencePanel.ReplaceAt(_noteSequencePanel.ReplaceTargetIndex, chordString);
				_noteSequencePanel.ExitReplaceMode();
			}
			else if (_noteSequencePanel.IsInsertMode && _noteSequencePanel.HasSelection)
			{
				int insertIndex = _noteSequencePanel.GetLastSelectedIndex() + 1;
				_noteSequencePanel.InsertAt(insertIndex, chordString);
				_noteSequencePanel.SelectSingle(insertIndex);
			}
			else
			{
				_noteSequencePanel.AddNote(chordString);
			}
			_pendingChordNotes.Clear();
			_pendingChordPreviews.Clear();
			UpdateChordPreview();
		}

		private void UpdateChordPreview(bool showFullMessage = false)
		{
			if (_pendingChordNotes.Count == 0)
			{
				_chordPreviewLabel.set_Text(_isChordMode ? "Click keys to build chord..." : "");
				((Control)_chordPreviewLabel).set_BasicTooltipText((string)null);
				((Control)_addChordButton).set_Enabled(false);
				return;
			}
			string chordText = string.Join(" ", _pendingChordNotes);
			string displayText = (showFullMessage ? $"Chord full ({7}/{7})" : $"Chord ({_pendingChordNotes.Count}/{7}): {chordText}");
			if (displayText.Length > 38)
			{
				displayText = displayText.Substring(0, 35) + "...";
			}
			_chordPreviewLabel.set_Text(displayText);
			((Control)_chordPreviewLabel).set_BasicTooltipText(chordText);
			((Control)_addChordButton).set_Enabled(true);
		}

		private void OnNoteSequenceStateChanged(object sender, EventArgs e)
		{
			UpdateStatusLabel();
		}

		private void UpdateStatusLabel()
		{
			if (!_isChordMode)
			{
				_chordPreviewLabel.set_Text("");
			}
		}

		private void OnOctaveChanged(object sender, bool up)
		{
			Module.Instance.PauseIfPlaying();
			Module.Instance.PlayOctaveChange(up);
		}

		private void OnNotePressed(object sender, NoteEventArgs e)
		{
			string noteString = BuildNoteString(e);
			if (_isChordMode)
			{
				AddToPendingChord(noteString, delegate
				{
					PlayNoteSound(e);
				});
			}
			else
			{
				CommitToken(noteString, delegate
				{
					PlayNoteSound(e);
				});
			}
		}

		private void OnDrumPadPressed(object sender, DrumSound sound)
		{
			string token = DrumMapping.Get(sound).Code + ":" + _durationSelector.CurrentDurationMs;
			if (_isChordMode)
			{
				AddToPendingChord(token, delegate
				{
					Module.Instance.PlayDrum(sound);
				});
			}
			else
			{
				CommitToken(token, delegate
				{
					Module.Instance.PlayDrum(sound);
				});
			}
		}

		private void OnDrumRestPressed(object sender, EventArgs e)
		{
			string token = "R:" + _durationSelector.CurrentDurationMs;
			CommitToken(token, null);
		}

		private void AddToPendingChord(string token, Action preview)
		{
			if (_pendingChordNotes.Count >= 7)
			{
				UpdateChordPreview(showFullMessage: true);
			}
			else if (!_pendingChordNotes.Contains(token))
			{
				_pendingChordNotes.Add(token);
				_pendingChordPreviews.Add(preview);
				UpdateChordPreview();
			}
		}

		private void CommitToken(string token, Action preview)
		{
			if (_noteSequencePanel.IsReplaceMode)
			{
				preview?.Invoke();
				_noteSequencePanel.ReplaceAt(_noteSequencePanel.ReplaceTargetIndex, token);
				_noteSequencePanel.ExitReplaceMode();
			}
			else if (_noteSequencePanel.IsInsertMode && _noteSequencePanel.HasSelection)
			{
				preview?.Invoke();
				int insertIndex = _noteSequencePanel.GetLastSelectedIndex() + 1;
				_noteSequencePanel.InsertAt(insertIndex, token);
				_noteSequencePanel.SelectSingle(insertIndex);
			}
			else
			{
				preview?.Invoke();
				_noteSequencePanel.AddNote(token);
			}
		}

		private void PlayNoteSound(NoteEventArgs e)
		{
			if (!e.IsRest)
			{
				Module.Instance.PauseIfPlaying();
				Module.Instance.PlayNote(e.Note, e.IsSharp, e.IsHighC);
			}
		}

		private string BuildNoteString(NoteEventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			if (e.IsRest)
			{
				sb.Append("R");
			}
			else
			{
				sb.Append(e.Note);
				if (e.IsHighC)
				{
					sb.Append("^");
				}
				else if (e.IsSharp)
				{
					sb.Append("#");
				}
				int octave = _pianoKeyboard.CurrentOctave;
				if (octave > 0)
				{
					sb.Append("+");
				}
				else if (octave < 0)
				{
					sb.Append("-");
				}
			}
			sb.Append(":");
			sb.Append(_durationSelector.CurrentDurationMs);
			return sb.ToString();
		}

		private void OnPreviewClicked(object sender, EventArgs e)
		{
			if (_pendingChordNotes.Count > 0)
			{
				AddPendingChord();
			}
			if (_noteSequencePanel.NoteCount == 0)
			{
				ScreenNotification.ShowNotification("Add some notes first!", (NotificationType)1, (Texture2D)null, 4);
				return;
			}
			List<string> notes = _noteSequencePanel.Notes.ToList();
			NoteParser.ParseResult parseResult = SongCompiler.ParseWithMapping(notes, _instrument);
			Song song = new Song
			{
				Name = "Preview",
				Artist = "Preview",
				Instrument = _instrument,
				IsCreated = true
			};
			song.Notes.AddRange(notes);
			song.Commands.AddRange(parseResult.Commands);
			Module.Instance.SongPlayer.Stop();
			_creatorPlayer.Play(song);
			StartPreviewHighlight(parseResult.CommandToNoteLineIndex, null);
		}

		private void OnPreviewSelectionClicked(object sender, EventArgs e)
		{
			IReadOnlyList<string> selectedNotes = _noteSequencePanel.GetSelectedNotes();
			if (selectedNotes.Count != 0)
			{
				try
				{
					IReadOnlyList<int> selectedIndices = _noteSequencePanel.GetSelectedIndices();
					NoteParser.ParseResult parseResult = SongCompiler.ParseWithMapping(selectedNotes.ToList(), _instrument);
					Song song = new Song
					{
						Name = "Preview",
						Artist = "Preview",
						Instrument = _instrument,
						IsCreated = true
					};
					song.Notes.AddRange(selectedNotes);
					song.Commands.AddRange(parseResult.Commands);
					Module.Instance.SongPlayer.Stop();
					_creatorPlayer.Play(song);
					StartPreviewHighlight(parseResult.CommandToNoteLineIndex, selectedIndices.ToArray());
				}
				catch (Exception ex)
				{
					ScreenNotification.ShowNotification("Error previewing selection: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
				}
			}
		}

		private void StartPreviewHighlight(int[] mapping, int[] noteIndices)
		{
			if (_isPreviewActive)
			{
				StopPreviewHighlight();
			}
			_isPreviewActive = true;
			_noteSequencePanel.StartPlaybackHighlight(_creatorPlayer, mapping, noteIndices);
			_noteSequencePanel.SetControlsEnabled(enabled: false);
			_creatorPlayer.OnStopped += OnPreviewEnded;
			_creatorPlayer.OnCompleted += OnPreviewEnded;
		}

		private void StopPreviewHighlight()
		{
			_creatorPlayer.OnStopped -= OnPreviewEnded;
			_creatorPlayer.OnCompleted -= OnPreviewEnded;
			_isPreviewActive = false;
			_noteSequencePanel.StopPlaybackHighlight();
			_noteSequencePanel.SetControlsEnabled(enabled: true);
		}

		private void OnPauseClicked(object sender, EventArgs e)
		{
			SongPlayer player = _creatorPlayer;
			if (player.IsPlaying)
			{
				player.TogglePause();
				_noteSequencePanel.SetPlaybackPaused(player.IsPaused);
			}
		}

		private void OnStopClicked(object sender, EventArgs e)
		{
			_creatorPlayer.Stop();
		}

		private void OnPreviewEnded(object sender, EventArgs e)
		{
			StopPreviewHighlight();
		}

		private void OnSaveClicked(object sender, MouseEventArgs e)
		{
			if (_pendingChordNotes.Count > 0)
			{
				AddPendingChord();
			}
			if (!ValidateInput())
			{
				return;
			}
			Song song = BuildSong(((TextInputBase)_titleInput).get_Text().Trim(), string.IsNullOrWhiteSpace(((TextInputBase)_artistInput).get_Text()) ? "Unknown" : ((TextInputBase)_artistInput).get_Text().Trim(), string.IsNullOrWhiteSpace(((TextInputBase)_transcriberInput).get_Text()) ? "" : ((TextInputBase)_transcriberInput).get_Text().Trim());
			if (song != null)
			{
				if (_editingSong != null)
				{
					song.IsCreated = _editingSong.IsCreated;
					song.IsUserImported = _editingSong.IsUserImported;
					song.CommunityId = _editingSong.CommunityId;
					song.IsUploaded = _editingSong.IsUploaded && !HasSongChanged(_editingSong, song);
					this.SongEdited?.Invoke(this, song);
					ScreenNotification.ShowNotification("Song updated: " + song.Name, (NotificationType)0, (Texture2D)null, 4);
				}
				else
				{
					this.SongCreated?.Invoke(this, song);
					ScreenNotification.ShowNotification("Song saved: " + song.Name, (NotificationType)0, (Texture2D)null, 4);
				}
				((Control)this).Hide();
				ClearInputs();
			}
		}

		private void OnCancelClicked(object sender, MouseEventArgs e)
		{
			((Control)this).Hide();
			ClearInputs();
		}

		public override void Hide()
		{
			if (_isPreviewActive)
			{
				StopPreviewHighlight();
			}
			_creatorPlayer.Stop();
			CloseNotesWindow();
			((WindowBase2)this).Hide();
			this.WindowClosed?.Invoke(this, EventArgs.Empty);
			ClearInputs();
		}

		private bool HasSongChanged(Song original, Song edited)
		{
			if (original.Name != edited.Name)
			{
				return true;
			}
			if (original.Artist != edited.Artist)
			{
				return true;
			}
			if (original.Transcriber != edited.Transcriber)
			{
				return true;
			}
			if (original.Instrument != edited.Instrument)
			{
				return true;
			}
			if (!original.Notes.SequenceEqual(edited.Notes))
			{
				return true;
			}
			return false;
		}

		private bool ValidateInput()
		{
			if (string.IsNullOrWhiteSpace(((TextInputBase)_titleInput).get_Text()))
			{
				ScreenNotification.ShowNotification("Please enter a song title", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			if (_noteSequencePanel.NoteCount == 0)
			{
				ScreenNotification.ShowNotification("Please add some notes", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			return true;
		}

		private Song BuildSong(string title, string artist, string transcriber)
		{
			try
			{
				Song song = new Song
				{
					Name = title,
					Artist = artist,
					Transcriber = transcriber,
					Instrument = _instrument,
					IsCreated = true,
					Bpm = _durationSelector.Bpm
				};
				foreach (string note in _noteSequencePanel.Notes)
				{
					song.Notes.Add(note);
				}
				List<SongCommand> commands = SongCompiler.Parse(song.Notes, _instrument);
				song.Commands.AddRange(commands);
				return song;
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Error building song: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
				return null;
			}
		}

		private void ClearInputs()
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			_editingSong = null;
			((TextInputBase)_titleInput).set_Text(string.Empty);
			((TextInputBase)_artistInput).set_Text(string.Empty);
			((TextInputBase)_transcriberInput).set_Text(string.Empty);
			_noteSequencePanel.ResetModes();
			_noteSequencePanel.ClearSelection();
			_noteSequencePanel.Clear();
			_noteSequencePanel.ClearUndoStack();
			_pianoKeyboard.CurrentOctave = 0;
			_pendingChordNotes.Clear();
			_pendingChordPreviews.Clear();
			_isChordMode = false;
			((Control)_chordModeButton).set_BackgroundColor(Color.get_Transparent());
			UpdateChordPreview();
		}

		protected override void DisposeControl()
		{
			_creatorPlayer.Stop();
			CloseNotesWindow();
			_noteSequencePanel.PreviewSelectionRequested -= OnPreviewSelectionClicked;
			_noteSequencePanel.InsertModeChanged -= OnNoteSequenceStateChanged;
			_noteSequencePanel.ReplaceModeChanged -= OnNoteSequenceStateChanged;
			_noteSequencePanel.SelectionChanged -= OnNoteSequenceStateChanged;
			_pianoKeyboard.NotePressed -= OnNotePressed;
			_pianoKeyboard.OctaveChanged -= OnOctaveChanged;
			_drumPadPanel.PadPressed -= OnDrumPadPressed;
			_drumPadPanel.RestPressed -= OnDrumRestPressed;
			_noteSequencePanel.PreviewAllRequested -= OnPreviewClicked;
			_noteSequencePanel.PauseRequested -= OnPauseClicked;
			_noteSequencePanel.StopRequested -= OnStopClicked;
			((Control)_saveButton).remove_Click((EventHandler<MouseEventArgs>)OnSaveClicked);
			((Control)_cancelButton).remove_Click((EventHandler<MouseEventArgs>)OnCancelClicked);
			((Control)_chordModeButton).remove_Click((EventHandler<MouseEventArgs>)OnChordModeToggle);
			((Control)_addChordButton).remove_Click((EventHandler<MouseEventArgs>)OnAddChordClicked);
			((Control)_readyButton).remove_Click((EventHandler<MouseEventArgs>)OnReadyClicked);
			TextBox titleInput = _titleInput;
			if (titleInput != null)
			{
				((Control)titleInput).Dispose();
			}
			TextBox artistInput = _artistInput;
			if (artistInput != null)
			{
				((Control)artistInput).Dispose();
			}
			TextBox transcriberInput = _transcriberInput;
			if (transcriberInput != null)
			{
				((Control)transcriberInput).Dispose();
			}
			PianoKeyboard pianoKeyboard = _pianoKeyboard;
			if (pianoKeyboard != null)
			{
				((Control)pianoKeyboard).Dispose();
			}
			DrumPadPanel drumPadPanel = _drumPadPanel;
			if (drumPadPanel != null)
			{
				((Control)drumPadPanel).Dispose();
			}
			DurationSelector durationSelector = _durationSelector;
			if (durationSelector != null)
			{
				((Control)durationSelector).Dispose();
			}
			NoteSequencePanel noteSequencePanel = _noteSequencePanel;
			if (noteSequencePanel != null)
			{
				((Control)noteSequencePanel).Dispose();
			}
			StandardButton saveButton = _saveButton;
			if (saveButton != null)
			{
				((Control)saveButton).Dispose();
			}
			StandardButton cancelButton = _cancelButton;
			if (cancelButton != null)
			{
				((Control)cancelButton).Dispose();
			}
			StandardButton chordModeButton = _chordModeButton;
			if (chordModeButton != null)
			{
				((Control)chordModeButton).Dispose();
			}
			Label chordPreviewLabel = _chordPreviewLabel;
			if (chordPreviewLabel != null)
			{
				((Control)chordPreviewLabel).Dispose();
			}
			StandardButton addChordButton = _addChordButton;
			if (addChordButton != null)
			{
				((Control)addChordButton).Dispose();
			}
			StandardButton readyButton = _readyButton;
			if (readyButton != null)
			{
				((Control)readyButton).Dispose();
			}
			Label confirmationLabel = _confirmationLabel;
			if (confirmationLabel != null)
			{
				((Control)confirmationLabel).Dispose();
			}
			Panel confirmationOverlay = _confirmationOverlay;
			if (confirmationOverlay != null)
			{
				((Control)confirmationOverlay).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
