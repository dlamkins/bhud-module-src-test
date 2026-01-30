using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Community;
using Maestro.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Community
{
	public class UploadWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 420;

			public const int WindowHeight = 355;

			public const int ContentWidth = 390;

			public const int LabelWidth = 90;

			public const int ValueWidth = 280;

			public const int RowHeight = 24;
		}

		private readonly CommunityUploadService _uploadService;

		private readonly List<Song> _allSongs;

		private List<Song> _uploadableSongs;

		private CustomDropdown _songSelector;

		private Label _nameLabel;

		private Label _artistLabel;

		private Label _transcriberLabel;

		private Label _instrumentLabel;

		private Label _noteCountLabel;

		private Label _remainingUploadsLabel;

		private Panel _validationPanel;

		private Label _nameValidation;

		private Label _transcriberValidation;

		private Label _instrumentValidation;

		private Label _notesValidation;

		private Label _duplicateValidation;

		private Label _rateLimitValidation;

		private StandardButton _uploadButton;

		private StandardButton _cancelButton;

		private LoadingSpinner _loadingSpinner;

		private Label _statusIcon;

		private Label _statusLabel;

		private Song _selectedSong;

		private bool _isUploading;

		private static Texture2D _backgroundTexture;

		public event EventHandler<UploadResponse> UploadCompleted;

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 355));
		}

		public UploadWindow(CommunityUploadService uploadService, List<Song> songs)
			: this(GetBackground(), new Rectangle(0, 0, 420, 355), new Rectangle(15, 20, 390, 355))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			_uploadService = uploadService;
			_allSongs = songs;
			_uploadableSongs = FilterUploadableSongs();
			((WindowBase2)this).set_Title("Upload to Community");
			((WindowBase2)this).set_Emblem(Module.Instance.ContentsManager.GetTexture("upload-emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("UploadWindow_v1");
			((WindowBase2)this).set_CanResize(false);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			BuildUi();
			SubscribeToEvents();
		}

		private void BuildUi()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Expected O, but got Unknown
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Expected O, but got Unknown
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Expected O, but got Unknown
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Expected O, but got Unknown
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Expected O, but got Unknown
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Expected O, but got Unknown
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Expected O, but got Unknown
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0414: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Expected O, but got Unknown
			int currentY = 2;
			CreateLabel("Select Song:", 0, currentY);
			CustomDropdown customDropdown = new CustomDropdown();
			((Control)customDropdown).set_Parent((Container)(object)this);
			((Control)customDropdown).set_Location(new Point(90, currentY));
			((Control)customDropdown).set_Size(new Point(280, 27));
			_songSelector = customDropdown;
			if (_uploadableSongs.Count == 0)
			{
				_songSelector.AddItem("No songs available");
				((Control)_songSelector).set_Enabled(false);
			}
			else
			{
				foreach (Song song in _uploadableSongs)
				{
					string fullText = song.Name + " - " + (song.Transcriber ?? "Unknown");
					_songSelector.AddItem(fullText, fullText, song);
				}
			}
			_songSelector.ValueChanged += OnSongSelected;
			currentY += 36;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, currentY));
			((Control)val).set_Size(new Point(390, 95));
			val.set_ShowBorder(true);
			val.set_CanScroll(true);
			Panel detailsPanel = val;
			int detailY = 8;
			_nameLabel = CreateDetailRow(detailsPanel, "Name:", ref detailY);
			_artistLabel = CreateDetailRow(detailsPanel, "Artist:", ref detailY);
			_transcriberLabel = CreateDetailRow(detailsPanel, "Transcriber:", ref detailY);
			_instrumentLabel = CreateDetailRow(detailsPanel, "Instrument:", ref detailY);
			_noteCountLabel = CreateDetailRow(detailsPanel, "Notes:", ref detailY);
			currentY += 102;
			CreateLabel("Validation:", 0, currentY);
			currentY += 24;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(0, currentY));
			((Control)val2).set_Size(new Point(390, 95));
			val2.set_ShowBorder(true);
			val2.set_CanScroll(true);
			_validationPanel = val2;
			int valY = 5;
			_nameValidation = CreateValidationRow(_validationPanel, "Name (min 3 chars)", ref valY);
			_transcriberValidation = CreateValidationRow(_validationPanel, "Transcriber (min 2 chars)", ref valY);
			_instrumentValidation = CreateValidationRow(_validationPanel, "Instrument selected", ref valY);
			_notesValidation = CreateValidationRow(_validationPanel, "At least 10 notes", ref valY);
			_duplicateValidation = CreateValidationRow(_validationPanel, "Not a duplicate", ref valY);
			_rateLimitValidation = CreateValidationRow(_validationPanel, "Upload limit OK", ref valY);
			currentY += 102;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(0, currentY));
			((Control)val3).set_Width(390);
			val3.set_Text($"Uploads remaining today: {_uploadService.GetRemainingUploads()}/3");
			val3.set_TextColor(MaestroTheme.MutedCream);
			_remainingUploadsLabel = val3;
			currentY += 31;
			LoadingSpinner val4 = new LoadingSpinner();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(0, currentY));
			((Control)val4).set_Size(new Point(20, 20));
			((Control)val4).set_Visible(false);
			_loadingSpinner = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Location(new Point(2, currentY));
			val5.set_AutoSizeWidth(true);
			val5.set_Text("");
			((Control)val5).set_Visible(false);
			_statusIcon = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			((Control)val6).set_Location(new Point(25, currentY));
			((Control)val6).set_Width(365);
			val6.set_Text("");
			val6.set_TextColor(MaestroTheme.LightGray);
			_statusLabel = val6;
			currentY += 7;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text("Upload");
			((Control)val7).set_Location(new Point(200, currentY));
			((Control)val7).set_Size(new Point(90, 26));
			((Control)val7).set_Enabled(false);
			_uploadButton = val7;
			((Control)_uploadButton).add_Click((EventHandler<MouseEventArgs>)OnUploadClicked);
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text("Cancel");
			((Control)val8).set_Location(new Point(300, currentY));
			((Control)val8).set_Size(new Point(90, 26));
			_cancelButton = val8;
			((Control)_cancelButton).add_Click((EventHandler<MouseEventArgs>)OnCancelClicked);
			if (_uploadableSongs.Count > 0)
			{
				_selectedSong = _uploadableSongs[0];
				UpdateSongDetails();
				UpdateValidation();
			}
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
			((Control)val).set_Location(new Point(x, y + 3));
			val.set_AutoSizeWidth(true);
			val.set_TextColor(MaestroTheme.CreamWhite);
			return val;
		}

		private Label CreateDetailRow(Panel parent, string labelText, ref int y)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Text(labelText);
			((Control)val).set_Location(new Point(10, y));
			((Control)val).set_Width(80);
			val.set_TextColor(MaestroTheme.LightGray);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)parent);
			val2.set_Text("-");
			((Control)val2).set_Location(new Point(90, y));
			((Control)val2).set_Width(290);
			val2.set_TextColor(MaestroTheme.CreamWhite);
			y += 17;
			return val2;
		}

		private Label CreateValidationRow(Panel parent, string text, ref int y)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Text("[ ] " + text);
			((Control)val).set_Location(new Point(10, y));
			((Control)val).set_Width(370);
			val.set_TextColor(MaestroTheme.LightGray);
			y += 15;
			return val;
		}

		private void SubscribeToEvents()
		{
			_uploadService.UploadProgressChanged += OnUploadProgressChanged;
		}

		private void OnSongSelected(object sender, ValueChangedEventArgs e)
		{
			Song song = _songSelector.SelectedItem?.Value as Song;
			if (song != null)
			{
				_selectedSong = song;
				UpdateSongDetails();
				UpdateValidation();
			}
		}

		private void UpdateSongDetails()
		{
			if (_selectedSong == null)
			{
				_nameLabel.set_Text("-");
				_artistLabel.set_Text("-");
				_transcriberLabel.set_Text("-");
				_instrumentLabel.set_Text("-");
				_noteCountLabel.set_Text("-");
			}
			else
			{
				_nameLabel.set_Text(_selectedSong.Name ?? "-");
				_artistLabel.set_Text(_selectedSong.Artist ?? "-");
				_transcriberLabel.set_Text(_selectedSong.Transcriber ?? "(not set)");
				_instrumentLabel.set_Text(_selectedSong.Instrument.ToString());
				_noteCountLabel.set_Text(GetNoteCount(_selectedSong).ToString());
			}
		}

		private void UpdateValidation()
		{
			_remainingUploadsLabel.set_Text($"Uploads remaining today: {_uploadService.GetRemainingUploads()}/3");
			if (_selectedSong == null)
			{
				((Control)_uploadButton).set_Enabled(false);
				return;
			}
			UploadValidationResult validation = _uploadService.ValidateSong(_selectedSong);
			UpdateValidationLabel(_nameValidation, "Name (min 3 chars)", validation.NameValid, validation.NameError);
			UpdateValidationLabel(_transcriberValidation, "Transcriber (min 2 chars)", validation.TranscriberValid, validation.TranscriberError);
			UpdateValidationLabel(_instrumentValidation, "Instrument selected", validation.InstrumentValid, validation.InstrumentError);
			UpdateValidationLabel(_notesValidation, "At least 10 notes", validation.NotesValid, validation.NotesError);
			UpdateValidationLabel(_duplicateValidation, "Not a duplicate", !validation.IsDuplicate, validation.DuplicateError);
			UpdateValidationLabel(_rateLimitValidation, "Upload limit OK", !validation.RateLimitExceeded, validation.RateLimitError);
			((Control)_uploadButton).set_Enabled(validation.IsValid && !_isUploading);
		}

		private void UpdateValidationLabel(Label label, string text, bool isValid, string error)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			string checkmark = (isValid ? "[X]" : "[ ]");
			label.set_Text(checkmark + " " + text);
			label.set_TextColor(isValid ? MaestroTheme.Playing : MaestroTheme.Error);
			if (!isValid && !string.IsNullOrEmpty(error))
			{
				((Control)label).set_BasicTooltipText(error);
			}
			else
			{
				((Control)label).set_BasicTooltipText((string)null);
			}
		}

		private int GetNoteCount(Song song)
		{
			if (song.Notes != null && song.Notes.Count > 0)
			{
				return song.Notes.Count((string n) => !n.StartsWith("R:"));
			}
			return song.Commands.Count((SongCommand c) => c.Type != CommandType.Wait);
		}

		private async void OnUploadClicked(object sender, MouseEventArgs e)
		{
			if (_selectedSong == null || _isUploading)
			{
				return;
			}
			_isUploading = true;
			((Control)_uploadButton).set_Enabled(false);
			((Control)_songSelector).set_Enabled(false);
			((Control)_loadingSpinner).set_Visible(true);
			((Control)_statusIcon).set_Visible(false);
			try
			{
				UploadResponse response = await _uploadService.UploadSongAsync(_selectedSong);
				if (response.Success)
				{
					((Control)_loadingSpinner).set_Visible(false);
					_statusIcon.set_Text("[X]");
					_statusIcon.set_TextColor(MaestroTheme.Playing);
					((Control)_statusIcon).set_Visible(true);
					ScreenNotification.ShowNotification("Song submitted successfully!", (NotificationType)0, (Texture2D)null, 4);
					this.UploadCompleted?.Invoke(this, response);
					RefreshSongList();
				}
				else
				{
					((Control)_loadingSpinner).set_Visible(false);
					_statusIcon.set_Text("[X]");
					_statusIcon.set_TextColor(MaestroTheme.Error);
					((Control)_statusIcon).set_Visible(true);
					_statusLabel.set_Text(response.Error ?? "Upload failed");
					_statusLabel.set_TextColor(MaestroTheme.Error);
					ScreenNotification.ShowNotification("Upload failed: " + response.Error, (NotificationType)2, (Texture2D)null, 4);
				}
			}
			finally
			{
				_isUploading = false;
				((Control)_loadingSpinner).set_Visible(false);
				((Control)_songSelector).set_Enabled(true);
				UpdateValidation();
			}
		}

		private void OnUploadProgressChanged(object sender, UploadProgressEventArgs e)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			_statusLabel.set_Text(e.Message);
			switch (e.State)
			{
			case UploadState.Completed:
				_statusLabel.set_TextColor(MaestroTheme.Playing);
				((Control)_loadingSpinner).set_Visible(false);
				_statusIcon.set_Text("[X]");
				_statusIcon.set_TextColor(MaestroTheme.Playing);
				((Control)_statusIcon).set_Visible(true);
				break;
			case UploadState.Failed:
			case UploadState.Cancelled:
				_statusLabel.set_TextColor(MaestroTheme.Error);
				((Control)_loadingSpinner).set_Visible(false);
				((Control)_statusIcon).set_Visible(false);
				break;
			default:
				_statusLabel.set_TextColor(MaestroTheme.LightGray);
				break;
			}
		}

		private void OnCancelClicked(object sender, MouseEventArgs e)
		{
			((Control)this).Hide();
		}

		public override void Show()
		{
			RefreshSongList();
			ClearStatus();
			((WindowBase2)this).Show();
		}

		private List<Song> FilterUploadableSongs()
		{
			return _allSongs.Where((Song s) => (s.IsUserImported || s.IsCreated) && !s.IsCommunityDownloaded && !s.IsUploaded).ToList();
		}

		internal void RefreshSongList()
		{
			_uploadableSongs = FilterUploadableSongs();
			_songSelector.ValueChanged -= OnSongSelected;
			_songSelector.ClearItems();
			if (_uploadableSongs.Count == 0)
			{
				_songSelector.AddItem("No songs available");
				((Control)_songSelector).set_Enabled(false);
				_selectedSong = null;
			}
			else
			{
				foreach (Song song in _uploadableSongs)
				{
					string fullText = song.Name + " - " + (song.Transcriber ?? "Unknown");
					_songSelector.AddItem(fullText, fullText, song);
				}
				((Control)_songSelector).set_Enabled(true);
				_selectedSong = _uploadableSongs[0];
			}
			_songSelector.ValueChanged += OnSongSelected;
			UpdateSongDetails();
			UpdateValidation();
		}

		private void ClearStatus()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			_statusLabel.set_Text("");
			_statusLabel.set_TextColor(MaestroTheme.LightGray);
			((Control)_statusIcon).set_Visible(false);
			((Control)_loadingSpinner).set_Visible(false);
		}

		protected override void DisposeControl()
		{
			_uploadService.UploadProgressChanged -= OnUploadProgressChanged;
			_songSelector.ValueChanged -= OnSongSelected;
			((Control)_uploadButton).remove_Click((EventHandler<MouseEventArgs>)OnUploadClicked);
			((Control)_cancelButton).remove_Click((EventHandler<MouseEventArgs>)OnCancelClicked);
			CustomDropdown songSelector = _songSelector;
			if (songSelector != null)
			{
				((Control)songSelector).Dispose();
			}
			Panel validationPanel = _validationPanel;
			if (validationPanel != null)
			{
				((Control)validationPanel).Dispose();
			}
			StandardButton uploadButton = _uploadButton;
			if (uploadButton != null)
			{
				((Control)uploadButton).Dispose();
			}
			StandardButton cancelButton = _cancelButton;
			if (cancelButton != null)
			{
				((Control)cancelButton).Dispose();
			}
			LoadingSpinner loadingSpinner = _loadingSpinner;
			if (loadingSpinner != null)
			{
				((Control)loadingSpinner).Dispose();
			}
			Label statusIcon = _statusIcon;
			if (statusIcon != null)
			{
				((Control)statusIcon).Dispose();
			}
			Label statusLabel = _statusLabel;
			if (statusLabel != null)
			{
				((Control)statusLabel).Dispose();
			}
			Label remainingUploadsLabel = _remainingUploadsLabel;
			if (remainingUploadsLabel != null)
			{
				((Control)remainingUploadsLabel).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
