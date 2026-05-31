using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Import
{
	public class ImportWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 420;

			public const int WindowHeight = 272;

			public const int ContentWidth = 390;

			public const int PasteBarHeight = 36;

			public const int ChipHeight = 18;

			public const int InputX = 90;

			public const int InputWidth = 290;

			public const int RowHeight = 28;

			public const int FooterButtonWidth = 90;

			public const int FooterButtonGap = 10;
		}

		private readonly StandardButton _pasteButton;

		private readonly Label _statusChip;

		private readonly TextBox _titleInput;

		private readonly TextBox _artistInput;

		private readonly TextBox _transcriberInput;

		private readonly Dropdown _instrumentDropdown;

		private readonly StandardButton _importButton;

		private readonly StandardButton _cancelButton;

		private List<string> _parsedNotes;

		private static Texture2D _backgroundTexture;

		public event EventHandler<Song> SongImported;

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 272));
		}

		public ImportWindow()
			: this(GetBackground(), new Rectangle(0, 0, 420, 272), new Rectangle(15, 20, 390, 272))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Expected O, but got Unknown
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Expected O, but got Unknown
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Expected O, but got Unknown
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Expected O, but got Unknown
			((WindowBase2)this).set_Title("Import Song");
			((WindowBase2)this).set_Subtitle("AHK v1 Format");
			((WindowBase2)this).set_Emblem(Module.Instance.ContentsManager.GetTexture("import-emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("ImportWindow_v1");
			((WindowBase2)this).set_CanResize(false);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			int currentY = 2;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Paste AHK Script");
			((Control)val).set_Location(new Point(0, currentY));
			((Control)val).set_Size(new Point(390, 36));
			_pasteButton = val;
			((Control)_pasteButton).add_Click((EventHandler<MouseEventArgs>)OnPasteClicked);
			currentY += 43;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(0, currentY));
			((Control)val2).set_Width(390);
			((Control)val2).set_Height(18);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_Font(GameService.Content.get_DefaultFont12());
			val2.set_Text("Paste your AHK v1 script to begin");
			val2.set_TextColor(MaestroTheme.HintTextColor);
			_statusChip = val2;
			currentY += 25;
			CreateLabel("Title:", 0, currentY);
			_titleInput = CreateTextBox(currentY, "Song title");
			((TextInputBase)_titleInput).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				UpdateImportEnabled();
			});
			currentY += 35;
			CreateLabel("Artist:", 0, currentY);
			_artistInput = CreateTextBox(currentY, "Artist name");
			currentY += 35;
			CreateLabel("Transcriber:", 0, currentY);
			_transcriberInput = CreateTextBox(currentY, "Transcriber name");
			currentY += 35;
			CreateLabel("Instrument:", 0, currentY);
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(90, currentY));
			((Control)val3).set_Width(290);
			_instrumentDropdown = val3;
			foreach (InstrumentInfo info in InstrumentCatalog.Pickable)
			{
				_instrumentDropdown.get_Items().Add(info.DisplayName);
			}
			_instrumentDropdown.set_SelectedItem("Harp");
			currentY += 42;
			int cancelX = 300;
			int importX = cancelX - 10 - 90;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Import");
			((Control)val4).set_Location(new Point(importX, currentY));
			((Control)val4).set_Size(new Point(90, 26));
			((Control)val4).set_Enabled(false);
			_importButton = val4;
			((Control)_importButton).add_Click((EventHandler<MouseEventArgs>)OnImportClicked);
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Cancel");
			((Control)val5).set_Location(new Point(cancelX, currentY));
			((Control)val5).set_Size(new Point(90, 26));
			_cancelButton = val5;
			((Control)_cancelButton).add_Click((EventHandler<MouseEventArgs>)OnCancelClicked);
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

		private TextBox CreateTextBox(int y, string placeholder)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(90, y));
			((Control)val).set_Width(290);
			((TextInputBase)val).set_PlaceholderText(placeholder);
			return val;
		}

		private void OnPasteClicked(object sender, MouseEventArgs e)
		{
			ClipboardUtil.get_WindowsClipboardService().GetTextAsync().ContinueWith(delegate(Task<string> task)
			{
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				if (task.IsFaulted || string.IsNullOrEmpty(task.Result))
				{
					_parsedNotes = null;
					SetStatus("Clipboard is empty", MaestroTheme.Error);
					UpdateImportEnabled();
				}
				else
				{
					ParseScript(task.Result);
				}
			});
		}

		private void ParseScript(string script)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				_parsedNotes = AhkParser.ParseToCompact(script);
				if (_parsedNotes.Count == 0)
				{
					SetStatus("No notes found in script", MaestroTheme.Error);
				}
				else
				{
					TimeSpan duration = TimeSpan.FromMilliseconds(NoteParser.CalculateDurationMs(_parsedNotes));
					SetStatus($"✓ {_parsedNotes.Count} notes · {duration:m\\:ss}", MaestroTheme.Playing);
				}
			}
			catch
			{
				_parsedNotes = null;
				SetStatus("Could not parse clipboard content", MaestroTheme.Error);
			}
			UpdateImportEnabled();
		}

		private void SetStatus(string text, Color color)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			_statusChip.set_Text(text);
			_statusChip.set_TextColor(color);
		}

		private void UpdateImportEnabled()
		{
			((Control)_importButton).set_Enabled(_parsedNotes != null && _parsedNotes.Count > 0 && !string.IsNullOrWhiteSpace(((TextInputBase)_titleInput).get_Text()));
		}

		private void OnImportClicked(object sender, MouseEventArgs e)
		{
			if (ValidateInput())
			{
				Song song = BuildSong();
				if (song != null)
				{
					this.SongImported?.Invoke(this, song);
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

		private bool ValidateInput()
		{
			if (string.IsNullOrWhiteSpace(((TextInputBase)_titleInput).get_Text()))
			{
				ScreenNotification.ShowNotification("Please enter a song title", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			if (_parsedNotes == null || _parsedNotes.Count == 0)
			{
				ScreenNotification.ShowNotification("Please paste a valid AHK script", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			return true;
		}

		private Song BuildSong()
		{
			InstrumentCatalog.TryFromDisplayName(_instrumentDropdown.get_SelectedItem(), out var instrument);
			Song song = new Song
			{
				Name = ((TextInputBase)_titleInput).get_Text().Trim(),
				Artist = (string.IsNullOrWhiteSpace(((TextInputBase)_artistInput).get_Text()) ? "Unknown" : ((TextInputBase)_artistInput).get_Text().Trim()),
				Transcriber = (string.IsNullOrWhiteSpace(((TextInputBase)_transcriberInput).get_Text()) ? null : ((TextInputBase)_transcriberInput).get_Text().Trim()),
				Instrument = instrument,
				IsUserImported = true
			};
			song.Notes.AddRange(_parsedNotes);
			List<SongCommand> commands = NoteParser.Parse(_parsedNotes);
			song.Commands.AddRange(commands);
			ScreenNotification.ShowNotification($"Imported \"{song.Name}\" ({_parsedNotes.Count} notes)", (NotificationType)0, (Texture2D)null, 4);
			return song;
		}

		private void ClearInputs()
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			((TextInputBase)_titleInput).set_Text(string.Empty);
			((TextInputBase)_artistInput).set_Text(string.Empty);
			((TextInputBase)_transcriberInput).set_Text(string.Empty);
			_instrumentDropdown.set_SelectedItem("Harp");
			_parsedNotes = null;
			SetStatus("Paste your AHK v1 script to begin", MaestroTheme.HintTextColor);
			UpdateImportEnabled();
		}

		protected override void DisposeControl()
		{
			((Control)_importButton).remove_Click((EventHandler<MouseEventArgs>)OnImportClicked);
			((Control)_cancelButton).remove_Click((EventHandler<MouseEventArgs>)OnCancelClicked);
			((Control)_pasteButton).remove_Click((EventHandler<MouseEventArgs>)OnPasteClicked);
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
			Dropdown instrumentDropdown = _instrumentDropdown;
			if (instrumentDropdown != null)
			{
				((Control)instrumentDropdown).Dispose();
			}
			Label statusChip = _statusChip;
			if (statusChip != null)
			{
				((Control)statusChip).Dispose();
			}
			StandardButton pasteButton = _pasteButton;
			if (pasteButton != null)
			{
				((Control)pasteButton).Dispose();
			}
			StandardButton importButton = _importButton;
			if (importButton != null)
			{
				((Control)importButton).Dispose();
			}
			StandardButton cancelButton = _cancelButton;
			if (cancelButton != null)
			{
				((Control)cancelButton).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
