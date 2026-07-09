using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Models;
using rp.spark.Services;
using rp.spark.UI.Controls;

namespace rp.spark.UI.Views
{
	public class ProfileNotesView : View
	{
		private const int MaxNoteLength = 8000;

		private static readonly Logger Logger = Logger.GetLogger<ProfileNotesView>();

		private readonly ProfileNotes _notesRepository;

		private CharacterProfile _profile;

		private PlayerPresence _presence;

		private Label _title;

		private SparkMultiline _notesBox;

		private Label _status;

		private bool _isLoading;

		private bool _isDirty;

		public ProfileNotesView(ProfileNotes notesRepository, CharacterProfile profile, PlayerPresence presence)
			: this()
		{
			_notesRepository = notesRepository;
			RememberProfile(profile, presence);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(GetTitleText());
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_TextColor(Color.get_White());
			val.set_StrokeText(true);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(760, 30));
			((Control)val).set_Parent(buildPanel);
			_title = val;
			SparkMultiline sparkMultiline = new SparkMultiline();
			((TextInputBase)sparkMultiline).set_PlaceholderText("Private notes for this profile.");
			((TextInputBase)sparkMultiline).set_MaxLength(8000);
			((Control)sparkMultiline).set_Location(new Point(0, 40));
			((Control)sparkMultiline).set_Size(new Point(760, 430));
			((Control)sparkMultiline).set_Parent(buildPanel);
			_notesBox = sparkMultiline;
			_notesBox.AttachWheelSource(buildPanel);
			((TextInputBase)_notesBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (!_isLoading)
				{
					_isDirty = true;
					SetStatusText(GetDraftStatusText());
				}
			});
			StandardButton val2 = new StandardButton();
			val2.set_Text("Save Notes");
			((Control)val2).set_Location(new Point(0, 490));
			((Control)val2).set_Size(new Point(120, 35));
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SaveNotes();
			});
			StandardButton val3 = new StandardButton();
			val3.set_Text("Clear");
			((Control)val3).set_Location(new Point(130, 490));
			((Control)val3).set_Size(new Point(90, 35));
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((TextInputBase)_notesBox).set_Text(string.Empty);
				SaveNotes();
			});
			Label val4 = new Label();
			val4.set_Text(string.Empty);
			val4.set_Font(GameService.Content.get_DefaultFont12());
			val4.set_TextColor(new Color(220, 220, 220));
			((Control)val4).set_Location(new Point(0, 535));
			((Control)val4).set_Size(new Point(760, 24));
			((Control)val4).set_Parent(buildPanel);
			_status = val4;
			LoadNotes();
		}

		public void SetProfile(CharacterProfile profile, PlayerPresence presence)
		{
			SaveIfDirty();
			RememberProfile(profile, presence);
			if (_title != null)
			{
				_title.set_Text(GetTitleText());
			}
			if (_notesBox != null)
			{
				LoadNotes();
			}
		}

		private void RememberProfile(CharacterProfile profile, PlayerPresence presence)
		{
			_profile = profile ?? new CharacterProfile();
			_presence = presence ?? new PlayerPresence();
		}

		private void LoadNotes()
		{
			_isLoading = true;
			try
			{
				ProfileNote note = _notesRepository?.Load(_profile, _presence);
				if (_notesBox != null)
				{
					((TextInputBase)_notesBox).set_Text(note?.Text ?? string.Empty);
				}
				_isDirty = false;
				SetStatusText(GetLoadedStatusText(note));
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load SPARK private notes for this profile.");
				SetStatusText("Couldn't load notes for this profile.");
			}
			finally
			{
				_isLoading = false;
			}
		}

		private void SaveNotes()
		{
			if (_notesBox != null)
			{
				try
				{
					ProfileNote note = _notesRepository?.Save(_profile, _presence, ((TextInputBase)_notesBox).get_Text() ?? string.Empty);
					_isDirty = false;
					SetStatusText(GetSavedStatusText(note));
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to save SPARK private notes for this profile.");
					SetStatusText("Couldn't save notes for this profile.");
				}
			}
		}

		private void SaveIfDirty()
		{
			if (_isDirty)
			{
				SaveNotes();
			}
		}

		private string GetTitleText()
		{
			string characterName = _presence.VisibleName();
			if (string.IsNullOrWhiteSpace(characterName))
			{
				characterName = _profile.DisplayName;
			}
			if (string.IsNullOrWhiteSpace(characterName))
			{
				characterName = _profile.CharacterName;
			}
			if (!string.IsNullOrWhiteSpace(characterName))
			{
				return "Private Notes: " + characterName.Trim();
			}
			return "Private Notes";
		}

		private string GetLoadedStatusText(ProfileNote note)
		{
			if (string.IsNullOrWhiteSpace(ProfileNotes.GetNoteKey(_profile, _presence)))
			{
				return "Notes unavailable for this profile.";
			}
			if (note == null || note.UpdatedAt == default(DateTime) || string.IsNullOrEmpty(note.Text))
			{
				return "No notes saved yet.";
			}
			return "Last saved " + ProfileText.FormatShortTime(note.UpdatedAt) + ".";
		}

		private string GetSavedStatusText(ProfileNote note)
		{
			if (note != null)
			{
				return "Notes saved " + ProfileText.FormatShortTime(note.UpdatedAt) + ".";
			}
			return "Notes saved.";
		}

		private string GetDraftStatusText()
		{
			SparkMultiline notesBox = _notesBox;
			int length = Math.Min((((notesBox != null) ? ((TextInputBase)notesBox).get_Text() : null) ?? string.Empty).Length, 8000);
			return $"Unsaved notes. {length}/{8000}";
		}

		private void SetStatusText(string text)
		{
			if (_status != null)
			{
				_status.set_Text(text ?? string.Empty);
			}
		}

		protected override void Unload()
		{
			SaveIfDirty();
			_title = null;
			_notesBox = null;
			_status = null;
			_isDirty = false;
		}
	}
}
