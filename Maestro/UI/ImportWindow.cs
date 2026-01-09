using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI
{
	public class ImportWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 420;

			public const int WindowHeight = 380;

			public const int ContentWidth = 390;

			public const int ContentHeight = 380;

			public const int InputX = 90;

			public const int InputWidth = 290;

			public const int RowHeight = 28;

			public const int ScriptAreaHeight = 140;
		}

		private readonly TextBox _titleInput;

		private readonly TextBox _artistInput;

		private readonly TextBox _transcriberInput;

		private readonly Dropdown _instrumentDropdown;

		private readonly Panel _scriptContainer;

		private readonly MultilineTextBox _scriptInput;

		private readonly StandardButton _importButton;

		private readonly StandardButton _cancelButton;

		private static Texture2D _backgroundTexture;

		public event EventHandler<Song> SongImported;

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 380));
		}

		public ImportWindow()
			: base(GetBackground(), new Rectangle(0, 0, 420, 380), new Rectangle(15, 30, 390, 380))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			base.Title = "Import Song";
			base.Subtitle = "AHK v1 Format";
			base.Emblem = Module.Instance.ContentsManager.GetTexture("import.png");
			base.SavesPosition = true;
			base.Id = "ImportWindow_v1";
			base.CanResize = false;
			base.Parent = GameService.Graphics.SpriteScreen;
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
			_instrumentDropdown = new Dropdown
			{
				Parent = this,
				Location = new Point(90, currentY),
				Width = 290
			};
			string[] names = Enum.GetNames(typeof(InstrumentType));
			foreach (string instrument in names)
			{
				_instrumentDropdown.Items.Add(instrument);
			}
			_instrumentDropdown.SelectedItem = "Harp";
			currentY += 35;
			CreateLabel("AHK Script:", 0, currentY);
			currentY += 28;
			_scriptContainer = new Panel
			{
				Parent = this,
				Location = new Point(0, currentY),
				Size = new Point(390, 140),
				CanScroll = true,
				ShowBorder = true
			};
			_scriptInput = new MultilineTextBox
			{
				Parent = _scriptContainer,
				Location = new Point(0, 0),
				Size = new Point(370, 600),
				PlaceholderText = "Paste AHK v1 script here...",
				HideBackground = true
			};
			currentY += 147;
			_importButton = new StandardButton
			{
				Parent = this,
				Text = "Import",
				Location = new Point(200, currentY),
				Size = new Point(90, 26)
			};
			_importButton.Click += OnImportClicked;
			_cancelButton = new StandardButton
			{
				Parent = this,
				Text = "Cancel",
				Location = new Point(300, currentY),
				Size = new Point(90, 26)
			};
			_cancelButton.Click += OnCancelClicked;
		}

		private Label CreateLabel(string text, int x, int y)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			return new Label
			{
				Parent = this,
				Text = text,
				Location = new Point(x, y + 5),
				AutoSizeWidth = true,
				TextColor = MaestroTheme.CreamWhite
			};
		}

		private TextBox CreateTextBox(int x, int y, string placeholder)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			return new TextBox
			{
				Parent = this,
				Location = new Point(x, y),
				Width = 290,
				PlaceholderText = placeholder
			};
		}

		private void OnImportClicked(object sender, MouseEventArgs e)
		{
			if (ValidateInput())
			{
				Song song = ParseSong();
				if (song != null)
				{
					this.SongImported?.Invoke(this, song);
					Hide();
					ClearInputs();
				}
			}
		}

		private void OnCancelClicked(object sender, MouseEventArgs e)
		{
			Hide();
			ClearInputs();
		}

		private bool ValidateInput()
		{
			if (string.IsNullOrWhiteSpace(_titleInput.Text))
			{
				ScreenNotification.ShowNotification("Please enter a song title", ScreenNotification.NotificationType.Error);
				return false;
			}
			if (string.IsNullOrWhiteSpace(_scriptInput.Text))
			{
				ScreenNotification.ShowNotification("Please paste the AHK script", ScreenNotification.NotificationType.Error);
				return false;
			}
			return true;
		}

		private Song ParseSong()
		{
			try
			{
				List<string> notes = AhkParser.ParseToCompact(_scriptInput.Text);
				if (notes.Count == 0)
				{
					ScreenNotification.ShowNotification("No notes found in AHK script", ScreenNotification.NotificationType.Error);
					return null;
				}
				Enum.TryParse<InstrumentType>(_instrumentDropdown.SelectedItem, out var instrument);
				Song song = new Song
				{
					Name = _titleInput.Text.Trim(),
					Artist = (string.IsNullOrWhiteSpace(_artistInput.Text) ? "Unknown" : _artistInput.Text.Trim()),
					Transcriber = (string.IsNullOrWhiteSpace(_transcriberInput.Text) ? null : _transcriberInput.Text.Trim()),
					Instrument = instrument,
					IsUserImported = true
				};
				song.Notes.AddRange(notes);
				List<SongCommand> commands = NoteParser.Parse(notes);
				song.Commands.AddRange(commands);
				ScreenNotification.ShowNotification($"Imported {notes.Count} notes, {song.Commands.Count} commands");
				return song;
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Parse error: " + ex.Message, ScreenNotification.NotificationType.Error);
				return null;
			}
		}

		private void ClearInputs()
		{
			_titleInput.Text = string.Empty;
			_artistInput.Text = string.Empty;
			_transcriberInput.Text = string.Empty;
			_scriptInput.Text = string.Empty;
			_instrumentDropdown.SelectedItem = "Harp";
		}

		protected override void DisposeControl()
		{
			_importButton.Click -= OnImportClicked;
			_cancelButton.Click -= OnCancelClicked;
			_titleInput?.Dispose();
			_artistInput?.Dispose();
			_transcriberInput?.Dispose();
			_instrumentDropdown?.Dispose();
			_scriptInput?.Dispose();
			_scriptContainer?.Dispose();
			_importButton?.Dispose();
			_cancelButton?.Dispose();
			base.DisposeControl();
		}
	}
}
