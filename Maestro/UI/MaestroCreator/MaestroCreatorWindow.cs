using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.MaestroCreator
{
	public class MaestroCreatorWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 420;

			public const int WindowHeight = 565;

			public const int ContentWidth = 390;

			public const int ContentHeight = 530;

			public const int RowHeight = 28;

			public const int NoteSequenceHeight = 195;

			public const int ChordBarHeight = 30;

			public const int TitleInputX = 35;

			public const int TitleInputWidth = 95;

			public const int ArtistLabelX = 135;

			public const int ArtistInputX = 175;

			public const int ArtistInputWidth = 70;

			public const int TranscriberLabelX = 250;

			public const int TranscriberInputX = 275;

			public const int TranscriberInputWidth = 115;

			public const int ActionButtonWidth = 80;

			public const int ActionButtonSpacing = 10;

			public const int ChordPreviewMaxLength = 38;

			public const int LabelYOffset = 5;
		}

		private static readonly Logger Logger = Logger.GetLogger<MaestroCreatorWindow>();

		private readonly TextBox _titleInput;

		private readonly TextBox _artistInput;

		private readonly TextBox _transcriberInput;

		private readonly PianoKeyboard _pianoKeyboard;

		private readonly DurationSelector _durationSelector;

		private readonly NoteSequencePanel _noteSequencePanel;

		private readonly StandardButton _previewButton;

		private readonly StandardButton _saveButton;

		private readonly StandardButton _cancelButton;

		private readonly StandardButton _chordModeButton;

		private readonly Label _chordPreviewLabel;

		private readonly StandardButton _addChordButton;

		private bool _isChordMode;

		private readonly List<string> _pendingChordNotes = new List<string>();

		private readonly List<NoteEventArgs> _pendingChordEvents = new List<NoteEventArgs>();

		private InstrumentType _instrument;

		private readonly Panel _confirmationOverlay;

		private readonly Label _confirmationLabel;

		private readonly StandardButton _readyButton;

		private bool _isWaitingForConfirmation;

		private static Texture2D _backgroundTexture;

		public event EventHandler<Song> SongCreated;

		public event EventHandler WindowClosed;

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 565));
		}

		public void SetInstrument(InstrumentType instrument)
		{
			_instrument = instrument;
			((WindowBase2)this).set_Subtitle(instrument.ToString());
			_pianoKeyboard.Configure(instrument);
		}

		public override void Show()
		{
			((WindowBase2)this).Show();
			_pianoKeyboard.Configure(_instrument);
			_isWaitingForConfirmation = true;
			_confirmationLabel.set_Text($"Equip your {_instrument} and click Ready");
			((Control)_confirmationOverlay).set_Visible(true);
			_pianoKeyboard.SetOctaveButtonsEnabled(enabled: false);
		}

		private void OnReadyClicked(object sender, MouseEventArgs e)
		{
			if (_isWaitingForConfirmation)
			{
				_isWaitingForConfirmation = false;
				((Control)_confirmationOverlay).set_Visible(false);
				_chordPreviewLabel.set_Text("Resetting octave...");
				if (_instrument == InstrumentType.Bass)
				{
					ResetToLowOctave();
				}
				else
				{
					Module.Instance.ResetToMiddleOctave();
				}
				_pianoKeyboard.SetOctaveButtonsEnabled(enabled: true);
				_chordPreviewLabel.set_Text("");
			}
		}

		private void ResetToLowOctave()
		{
			Module keyboardService = Module.Instance;
			for (int i = 0; i < 5; i++)
			{
				keyboardService.PlayOctaveChange(up: false);
				Thread.Sleep(100);
			}
		}

		public MaestroCreatorWindow()
			: this(GetBackground(), new Rectangle(0, 0, 420, 565), new Rectangle(15, 30, 390, 530))
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Expected O, but got Unknown
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Expected O, but got Unknown
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Expected O, but got Unknown
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Expected O, but got Unknown
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Expected O, but got Unknown
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Expected O, but got Unknown
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Expected O, but got Unknown
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_041e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_043c: Expected O, but got Unknown
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_0459: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_0483: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0496: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Expected O, but got Unknown
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Expected O, but got Unknown
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_051c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Expected O, but got Unknown
			((WindowBase2)this).set_Title("Maestro Creator");
			((WindowBase2)this).set_Emblem(Module.Instance.ContentsManager.GetTexture("creator-emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("MaestroCreatorWindow_v1");
			((WindowBase2)this).set_CanResize(false);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			int currentY = 2;
			CreateLabel("Title:", 0, currentY);
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(35, currentY));
			((Control)val).set_Width(95);
			((TextInputBase)val).set_PlaceholderText("Song title");
			_titleInput = val;
			CreateLabel("Artist:", 135, currentY);
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(175, currentY));
			((Control)val2).set_Width(70);
			((TextInputBase)val2).set_PlaceholderText("Artist");
			_artistInput = val2;
			CreateLabel("By:", 250, currentY);
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(275, currentY));
			((Control)val3).set_Width(115);
			((TextInputBase)val3).set_PlaceholderText("Your name");
			_transcriberInput = val3;
			currentY += 35;
			PianoKeyboard pianoKeyboard = new PianoKeyboard(390);
			((Control)pianoKeyboard).set_Parent((Container)(object)this);
			((Control)pianoKeyboard).set_Location(new Point(0, currentY));
			((Panel)pianoKeyboard).set_ShowBorder(true);
			_pianoKeyboard = pianoKeyboard;
			_pianoKeyboard.NotePressed += OnNotePressed;
			_pianoKeyboard.OctaveChanged += OnOctaveChanged;
			currentY += PianoKeyboard.Layout.TotalHeight + 7;
			DurationSelector durationSelector = new DurationSelector(390);
			((Control)durationSelector).set_Parent((Container)(object)this);
			((Control)durationSelector).set_Location(new Point(0, currentY));
			_durationSelector = durationSelector;
			currentY += 37;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Chord: OFF");
			((Control)val4).set_Location(new Point(0, currentY));
			((Control)val4).set_Size(new Point(80, 26));
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
			currentY += 37;
			NoteSequencePanel noteSequencePanel = new NoteSequencePanel(390, 195);
			((Control)noteSequencePanel).set_Parent((Container)(object)this);
			((Control)noteSequencePanel).set_Location(new Point(0, currentY));
			((Panel)noteSequencePanel).set_ShowBorder(true);
			_noteSequencePanel = noteSequencePanel;
			currentY += 209;
			int buttonsStartX = 65;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text("Preview");
			((Control)val7).set_Location(new Point(buttonsStartX, currentY));
			((Control)val7).set_Size(new Point(80, 26));
			_previewButton = val7;
			((Control)_previewButton).add_Click((EventHandler<MouseEventArgs>)OnPreviewClicked);
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text("Save");
			((Control)val8).set_Location(new Point(buttonsStartX + 80 + 10, currentY));
			((Control)val8).set_Size(new Point(80, 26));
			_saveButton = val8;
			((Control)_saveButton).add_Click((EventHandler<MouseEventArgs>)OnSaveClicked);
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)this);
			val9.set_Text("Cancel");
			((Control)val9).set_Location(new Point(buttonsStartX + 180, currentY));
			((Control)val9).set_Size(new Point(80, 26));
			_cancelButton = val9;
			((Control)_cancelButton).add_Click((EventHandler<MouseEventArgs>)OnCancelClicked);
			Panel val10 = new Panel();
			((Control)val10).set_Parent((Container)(object)this);
			((Control)val10).set_Location(new Point(0, 37));
			((Control)val10).set_Size(new Point(390, PianoKeyboard.Layout.TotalHeight));
			((Control)val10).set_BackgroundColor(new Color(0, 0, 0, 200));
			((Control)val10).set_ZIndex(100);
			((Control)val10).set_Visible(false);
			_confirmationOverlay = val10;
			Label val11 = new Label();
			((Control)val11).set_Parent((Container)(object)_confirmationOverlay);
			val11.set_Text("Equip your instrument and click Ready");
			((Control)val11).set_Location(new Point(0, PianoKeyboard.Layout.TotalHeight / 2 - 30));
			((Control)val11).set_Size(new Point(390, 30));
			val11.set_Font(GameService.Content.get_DefaultFont16());
			val11.set_TextColor(MaestroTheme.CreamWhite);
			val11.set_HorizontalAlignment((HorizontalAlignment)1);
			_confirmationLabel = val11;
			StandardButton val12 = new StandardButton();
			((Control)val12).set_Parent((Container)(object)_confirmationOverlay);
			val12.set_Text("Ready");
			((Control)val12).set_Location(new Point(145, PianoKeyboard.Layout.TotalHeight / 2 + 5));
			((Control)val12).set_Size(new Point(100, 30));
			_readyButton = val12;
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
			val.set_TextColor(MaestroTheme.CreamWhite);
			return val;
		}

		private void OnChordModeToggle(object sender, MouseEventArgs e)
		{
			_isChordMode = !_isChordMode;
			_chordModeButton.set_Text(_isChordMode ? "Chord: ON" : "Chord: OFF");
			if (!_isChordMode && _pendingChordNotes.Count > 0)
			{
				_pendingChordNotes.Clear();
				_pendingChordEvents.Clear();
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
			foreach (NoteEventArgs noteEvent in _pendingChordEvents)
			{
				PlayNoteSound(noteEvent);
			}
			string chordString = string.Join(" ", _pendingChordNotes);
			_noteSequencePanel.AddNote(chordString);
			_pendingChordNotes.Clear();
			_pendingChordEvents.Clear();
			UpdateChordPreview();
		}

		private void UpdateChordPreview()
		{
			if (_pendingChordNotes.Count == 0)
			{
				_chordPreviewLabel.set_Text(_isChordMode ? "Click keys to build chord..." : "");
				((Control)_chordPreviewLabel).set_BasicTooltipText((string)null);
				((Control)_addChordButton).set_Enabled(false);
				return;
			}
			string chordText = string.Join(" ", _pendingChordNotes);
			string displayText = "Chord: " + chordText;
			if (displayText.Length > 38)
			{
				displayText = displayText.Substring(0, 35) + "...";
			}
			_chordPreviewLabel.set_Text(displayText);
			((Control)_chordPreviewLabel).set_BasicTooltipText(chordText);
			((Control)_addChordButton).set_Enabled(true);
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
				_pendingChordNotes.Add(noteString);
				_pendingChordEvents.Add(e);
				UpdateChordPreview();
			}
			else
			{
				PlayNoteSound(e);
				_noteSequencePanel.AddNote(noteString);
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
				if (_instrument == InstrumentType.Bass)
				{
					if (octave > 0)
					{
						sb.Append("+");
					}
				}
				else if (octave > 0)
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

		private void OnPreviewClicked(object sender, MouseEventArgs e)
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
			Song song = BuildSong("Preview", "Preview", "");
			if (song != null)
			{
				Module.Instance.PreviewSong(song);
			}
		}

		private void OnSaveClicked(object sender, MouseEventArgs e)
		{
			if (_pendingChordNotes.Count > 0)
			{
				AddPendingChord();
			}
			if (ValidateInput())
			{
				Song song = BuildSong(((TextInputBase)_titleInput).get_Text().Trim(), string.IsNullOrWhiteSpace(((TextInputBase)_artistInput).get_Text()) ? "Unknown" : ((TextInputBase)_artistInput).get_Text().Trim(), string.IsNullOrWhiteSpace(((TextInputBase)_transcriberInput).get_Text()) ? "" : ((TextInputBase)_transcriberInput).get_Text().Trim());
				if (song != null)
				{
					this.SongCreated?.Invoke(this, song);
					ScreenNotification.ShowNotification("Song saved: " + song.Name, (NotificationType)0, (Texture2D)null, 4);
					((Control)this).Hide();
					ClearInputs();
				}
			}
		}

		private void OnCancelClicked(object sender, MouseEventArgs e)
		{
			((Control)this).Hide();
			ClearInputs();
		}

		public override void Hide()
		{
			((WindowBase2)this).Hide();
			this.WindowClosed?.Invoke(this, EventArgs.Empty);
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
					IsUserImported = true
				};
				foreach (string note in _noteSequencePanel.Notes)
				{
					song.Notes.Add(note);
				}
				List<SongCommand> commands = NoteParser.Parse(song.Notes);
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
			((TextInputBase)_titleInput).set_Text(string.Empty);
			((TextInputBase)_artistInput).set_Text(string.Empty);
			((TextInputBase)_transcriberInput).set_Text(string.Empty);
			_noteSequencePanel.Clear();
			_pianoKeyboard.CurrentOctave = 0;
			_pendingChordNotes.Clear();
			_pendingChordEvents.Clear();
			_isChordMode = false;
			_chordModeButton.set_Text("Chord: OFF");
			UpdateChordPreview();
		}

		protected override void DisposeControl()
		{
			_pianoKeyboard.NotePressed -= OnNotePressed;
			_pianoKeyboard.OctaveChanged -= OnOctaveChanged;
			((Control)_previewButton).remove_Click((EventHandler<MouseEventArgs>)OnPreviewClicked);
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
			StandardButton previewButton = _previewButton;
			if (previewButton != null)
			{
				((Control)previewButton).Dispose();
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
