using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using DanceDanceRotationModule.Model;
using DanceDanceRotationModule.Storage;
using DanceDanceRotationModule.Util;
using Microsoft.Xna.Framework;

namespace DanceDanceRotationModule.Views
{
	public class SongListView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<SongListView>();

		private FlowPanel _songsListPanel;

		private List<SongListRow> _rows = new List<SongListRow>();

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			FlowPanel rootPanel = val;
			FlowPanel val2 = new FlowPanel();
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			val2.set_ControlPadding(new Vector2(4f, 0f));
			((Control)val2).set_Parent((Container)(object)rootPanel);
			FlowPanel topPanel = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Add From Clipboard");
			((Control)val3).set_Parent((Container)(object)topPanel);
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Info("AddFromClipboardButton Clicked");
				Logger.Info("Attempting to read in clipboard contents");
				string result = ClipboardUtil.get_WindowsClipboardService().GetTextAsync().Result;
				Song song = DanceDanceRotationModule.SongRepo.AddSong(result, showNotification: true);
				if (song != null)
				{
					DanceDanceRotationModule.SongRepo.SetSelectedSong(song.Id);
				}
			});
			StandardButton val4 = new StandardButton();
			val4.set_Text("Create Song");
			((Control)val4).set_Parent((Container)(object)topPanel);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Info("CreateSongButton Clicked");
				UrlHelper.OpenUrl("https://campbt.github.io/DanceDanceRotationComposer/create.html");
			});
			FlowPanel val5 = new FlowPanel();
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Container)val5).set_HeightSizingMode((SizingMode)2);
			val5.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val5).set_CanScroll(true);
			((Control)val5).set_Parent((Container)(object)rootPanel);
			_songsListPanel = val5;
			_rows = new List<SongListRow>();
			DanceDanceRotationModule.SongRepo.OnSelectedSongChanged += delegate(object sender, SelectedSongInfo info)
			{
				if (_rows.Count == 0)
				{
					BuildSongList();
				}
				else if (info.Song != null)
				{
					foreach (SongListRow row in _rows)
					{
						row.OnSelectedSongInfoChanged(info.Song.Id);
					}
				}
			};
			DanceDanceRotationModule.SongRepo.OnSongsChanged += delegate
			{
				Logger.Trace("OnSongsChanged - Rebuilding Song List");
				BuildSongList();
			};
			DanceDanceRotationModule.Settings.ShowOnlyCharacterClassSongs.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				Logger.Trace("ShowOnlyCharacterClassSongs.SettingsChanged - Rebuilding Song List");
				BuildSongList();
			});
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)delegate
			{
				Logger.Trace("PlayerCharacter.NameChanged - Rebuilding Song List");
				BuildSongList();
			});
			BuildSongList();
		}

		private void BuildSongList()
		{
			((Container)_songsListPanel).ClearChildren();
			_rows.Clear();
			Song.ID selectedSongId = DanceDanceRotationModule.SongRepo.GetSelectedSongId();
			List<Song> songList = DanceDanceRotationModule.SongRepo.GetAllSongs();
			Profession playerCurrentProfession = ProfessionExtensions.CurrentProfessionOfPlayer();
			List<Song> filteredSongs;
			if (DanceDanceRotationModule.Settings.ShowOnlyCharacterClassSongs.get_Value() && playerCurrentProfession != Profession.Unknown)
			{
				filteredSongs = new List<Song>();
				foreach (Song song4 in songList)
				{
					if (song4.Profession == Profession.Common || song4.Profession == Profession.Unknown || song4.Profession == playerCurrentProfession)
					{
						filteredSongs.Add(song4);
					}
				}
			}
			else
			{
				filteredSongs = songList;
			}
			filteredSongs.Sort(delegate(Song song1, Song song2)
			{
				if (song1.Profession != song2.Profession)
				{
					return song1.Profession.CompareTo(song2.Profession);
				}
				return (song1.EliteName != song2.EliteName) ? string.Compare(song1.EliteName, song2.EliteName, StringComparison.Ordinal) : string.Compare(song1.Name, song2.Name, StringComparison.Ordinal);
			});
			Logger.Trace($"BuildSongList | {filteredSongs.Count}/{songList.Count} songs shown");
			foreach (Song song3 in filteredSongs)
			{
				List<SongListRow> rows = _rows;
				SongListRow songListRow = new SongListRow(song3, selectedSongId);
				((Container)songListRow).set_WidthSizingMode((SizingMode)2);
				((Container)songListRow).set_HeightSizingMode((SizingMode)1);
				((Control)songListRow).set_Parent((Container)(object)_songsListPanel);
				rows.Add(songListRow);
			}
		}

		public SongListView()
			: this()
		{
		}
	}
}
