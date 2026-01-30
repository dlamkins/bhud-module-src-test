using System;
using System.Collections.Generic;
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

			public const int WindowHeight = 370;

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
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 370));
		}

		public ImportWindow()
			: this(GetBackground(), new Rectangle(0, 0, 420, 370), new Rectangle(15, 20, 390, 380))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Expected O, but got Unknown
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Expected O, but got Unknown
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Expected O, but got Unknown
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Expected O, but got Unknown
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Expected O, but got Unknown
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
			currentY += 35;
			CreateLabel("AHK Script:", 0, currentY);
			currentY += 28;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(0, currentY));
			((Control)val2).set_Size(new Point(390, 140));
			val2.set_CanScroll(true);
			val2.set_ShowBorder(true);
			_scriptContainer = val2;
			MultilineTextBox val3 = new MultilineTextBox();
			((Control)val3).set_Parent((Container)(object)_scriptContainer);
			((Control)val3).set_Location(new Point(0, 0));
			((Control)val3).set_Size(new Point(370, 600));
			((TextInputBase)val3).set_PlaceholderText("Paste AHK v1 script here...");
			val3.set_HideBackground(true);
			_scriptInput = val3;
			currentY += 147;
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

		private void OnImportClicked(object sender, MouseEventArgs e)
		{
			if (ValidateInput())
			{
				Song song = ParseSong();
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
			if (string.IsNullOrWhiteSpace(((TextInputBase)_scriptInput).get_Text()))
			{
				ScreenNotification.ShowNotification("Please paste the AHK script", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			return true;
		}

		private Song ParseSong()
		{
			try
			{
				List<string> notes = AhkParser.ParseToCompact(((TextInputBase)_scriptInput).get_Text());
				if (notes.Count == 0)
				{
					ScreenNotification.ShowNotification("No notes found in AHK script", (NotificationType)2, (Texture2D)null, 4);
					return null;
				}
				Enum.TryParse<InstrumentType>(_instrumentDropdown.get_SelectedItem(), out var instrument);
				Song song = new Song
				{
					Name = ((TextInputBase)_titleInput).get_Text().Trim(),
					Artist = (string.IsNullOrWhiteSpace(((TextInputBase)_artistInput).get_Text()) ? "Unknown" : ((TextInputBase)_artistInput).get_Text().Trim()),
					Transcriber = (string.IsNullOrWhiteSpace(((TextInputBase)_transcriberInput).get_Text()) ? null : ((TextInputBase)_transcriberInput).get_Text().Trim()),
					Instrument = instrument,
					IsUserImported = true
				};
				song.Notes.AddRange(notes);
				List<SongCommand> commands = NoteParser.Parse(notes);
				song.Commands.AddRange(commands);
				ScreenNotification.ShowNotification($"Imported {notes.Count} notes, {song.Commands.Count} commands", (NotificationType)0, (Texture2D)null, 4);
				return song;
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Parse error: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
				return null;
			}
		}

		private void ClearInputs()
		{
			((TextInputBase)_titleInput).set_Text(string.Empty);
			((TextInputBase)_artistInput).set_Text(string.Empty);
			((TextInputBase)_transcriberInput).set_Text(string.Empty);
			((TextInputBase)_scriptInput).set_Text(string.Empty);
			_instrumentDropdown.set_SelectedItem("Harp");
		}

		protected override void DisposeControl()
		{
			((Control)_importButton).remove_Click((EventHandler<MouseEventArgs>)OnImportClicked);
			((Control)_cancelButton).remove_Click((EventHandler<MouseEventArgs>)OnCancelClicked);
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
			MultilineTextBox scriptInput = _scriptInput;
			if (scriptInput != null)
			{
				((Control)scriptInput).Dispose();
			}
			Panel scriptContainer = _scriptContainer;
			if (scriptContainer != null)
			{
				((Control)scriptContainer).Dispose();
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
