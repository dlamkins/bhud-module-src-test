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

			public const int WindowHeight = 250;

			public const int ContentWidth = 390;

			public const int InputX = 90;

			public const int InputWidth = 290;

			public const int RowHeight = 28;
		}

		private readonly TextBox _titleInput;

		private readonly TextBox _artistInput;

		private readonly TextBox _transcriberInput;

		private readonly Dropdown _instrumentDropdown;

		private readonly StandardButton _pasteButton;

		private readonly Label _parseStatusLabel;

		private readonly StandardButton _importButton;

		private readonly StandardButton _cancelButton;

		private List<string> _parsedNotes;

		private static Texture2D _backgroundTexture;

		public event EventHandler<Song> SongImported;

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 250));
		}

		public ImportWindow()
			: this(GetBackground(), new Rectangle(0, 0, 420, 250), new Rectangle(15, 20, 390, 250))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Expected O, but got Unknown
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Expected O, but got Unknown
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Expected O, but got Unknown
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Expected O, but got Unknown
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Expected O, but got Unknown
			((WindowBase2)this).set_Title("Import Song");
			((WindowBase2)this).set_Subtitle("AHK v1 Format");
			((WindowBase2)this).set_Emblem(Module.Instance.ContentsManager.GetTexture("import-emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("ImportWindow_v1");
			((WindowBase2)this).set_CanResize(false);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			int currentY = 2;
			CreateLabel("Title:", 0, currentY);
			_titleInput = CreateTextBox(90, currentY, "Song title");
			currentY += 35;
			CreateLabel("Artist:", 0, currentY);
			_artistInput = CreateTextBox(90, currentY, "Artist name");
			currentY += 35;
			CreateLabel("Transcriber:", 0, currentY);
			_transcriberInput = CreateTextBox(90, currentY, "Transcriber name");
			currentY += 35;
			CreateLabel("Instrument:", 0, currentY);
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(90, currentY));
			((Control)val).set_Width(290);
			_instrumentDropdown = val;
			string[] names = Enum.GetNames(typeof(InstrumentType));
			foreach (string instrument in names)
			{
				_instrumentDropdown.get_Items().Add(instrument);
			}
			_instrumentDropdown.set_SelectedItem("Harp");
			currentY += 42;
			CreateLabel("AHK Script:", 0, currentY);
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Paste from Clipboard");
			((Control)val2).set_Location(new Point(90, currentY));
			((Control)val2).set_Size(new Point(290, 26));
			_pasteButton = val2;
			((Control)_pasteButton).add_Click((EventHandler<MouseEventArgs>)OnPasteClicked);
			currentY += 35;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("");
			((Control)val3).set_Location(new Point(90, currentY));
			((Control)val3).set_Width(290);
			val3.set_AutoSizeHeight(true);
			val3.set_TextColor(MaestroTheme.MutedCream);
			_parseStatusLabel = val3;
			currentY += 17;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Import");
			((Control)val4).set_Location(new Point(200, currentY));
			((Control)val4).set_Size(new Point(90, 26));
			_importButton = val4;
			((Control)_importButton).add_Click((EventHandler<MouseEventArgs>)OnImportClicked);
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Cancel");
			((Control)val5).set_Location(new Point(300, currentY));
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

		private TextBox CreateTextBox(int x, int y, string placeholder)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Width(290);
			((TextInputBase)val).set_PlaceholderText(placeholder);
			return val;
		}

		private void OnPasteClicked(object sender, MouseEventArgs e)
		{
			ClipboardUtil.get_WindowsClipboardService().GetTextAsync().ContinueWith(delegate(Task<string> task)
			{
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				if (task.IsFaulted || string.IsNullOrEmpty(task.Result))
				{
					_parsedNotes = null;
					_parseStatusLabel.set_Text("Clipboard is empty");
					_parseStatusLabel.set_TextColor(MaestroTheme.Error);
				}
				else
				{
					ParseScript(task.Result);
				}
			});
		}

		private void ParseScript(string script)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				_parsedNotes = AhkParser.ParseToCompact(script);
				if (_parsedNotes.Count == 0)
				{
					_parseStatusLabel.set_Text("No notes found in script");
					_parseStatusLabel.set_TextColor(MaestroTheme.Error);
				}
				else
				{
					TimeSpan duration = TimeSpan.FromMilliseconds(NoteParser.CalculateDurationMs(_parsedNotes));
					_parseStatusLabel.set_Text($"{_parsedNotes.Count} notes · {duration:m\\:ss}");
					_parseStatusLabel.set_TextColor(MaestroTheme.Playing);
				}
			}
			catch
			{
				_parsedNotes = null;
				_parseStatusLabel.set_Text("Could not parse clipboard content");
				_parseStatusLabel.set_TextColor(MaestroTheme.Error);
			}
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
			Enum.TryParse<InstrumentType>(_instrumentDropdown.get_SelectedItem(), out var instrument);
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
			((TextInputBase)_titleInput).set_Text(string.Empty);
			((TextInputBase)_artistInput).set_Text(string.Empty);
			((TextInputBase)_transcriberInput).set_Text(string.Empty);
			_instrumentDropdown.set_SelectedItem("Harp");
			_parsedNotes = null;
			_parseStatusLabel.set_Text("");
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
