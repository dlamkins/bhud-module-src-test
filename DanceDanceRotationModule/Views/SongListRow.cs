using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using DanceDanceRotationModule.Model;
using DanceDanceRotationModule.Util;
using Microsoft.Xna.Framework;

namespace DanceDanceRotationModule.Views
{
	public class SongListRow : Panel
	{
		private static readonly Logger Logger = Logger.GetLogger<SongListRow>();

		internal Song Song { get; }

		private Checkbox Checkbox { get; }

		private Label NameLabel { get; }

		private Label ProfessionLabel { get; }

		private Image DeleteButton { get; }

		public SongListRow(Song song, Song.ID checkedId)
			: this()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Expected O, but got Unknown
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Expected O, but got Unknown
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Expected O, but got Unknown
			Song = song;
			((Control)this).set_Padding(new Thickness(12f, 12f, 12f, 0f));
			((Panel)this).set_CanScroll(false);
			Checkbox val = new Checkbox();
			((Control)val).set_Padding(new Thickness(20f, 20f));
			((Control)val).set_Location(new Point(10, 28));
			((Control)val).set_Parent((Container)(object)this);
			Checkbox = val;
			Checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent checkChangedEvent)
			{
				if (checkChangedEvent.get_Checked())
				{
					DanceDanceRotationModule.SongRepo.SetSelectedSong(song.Id);
				}
			});
			Label val2 = new Label();
			val2.set_Text(song.Name);
			val2.set_Font(GameService.Content.get_DefaultFont18());
			((Control)val2).set_Location(new Point(((Control)Checkbox).get_Width() + 20, 8));
			((Control)val2).set_Parent((Container)(object)this);
			NameLabel = val2;
			Label val3 = new Label();
			val3.set_Text(song.EliteName);
			val3.set_AutoSizeWidth(true);
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(ProfessionExtensions.GetProfessionColor(song.Profession));
			((Control)val3).set_Location(new Point(((Control)NameLabel).get_Left(), ((Control)NameLabel).get_Bottom()));
			((Control)val3).set_Parent((Container)(object)this);
			ProfessionLabel = val3;
			Image val4 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.ButtonDelete));
			((Control)val4).set_Size(ControlExtensions.ImageButtonSmallSize);
			((Control)val4).set_Parent((Container)(object)this);
			DeleteButton = val4;
			ControlExtensions.ConvertToButton((Control)(object)DeleteButton);
			((Control)DeleteButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Info("DeleteButton Clicked: song=" + song.Id.Name);
				DanceDanceRotationModule.SongRepo.DeleteSong(song.Id);
			});
			((Control)this).set_Height(CalculateHeight());
			OnSelectedSongInfoChanged(checkedId);
		}

		public void OnSelectedSongInfoChanged(Song.ID checkedId)
		{
			bool isSelectedSong = Song.Id.Equals(checkedId);
			((Control)Checkbox).set_Enabled(!isSelectedSong);
			Checkbox.set_Checked(isSelectedSong);
		}

		private int CalculateHeight()
		{
			return 16 + ((Control)NameLabel).get_Height() + ((Control)ProfessionLabel).get_Height();
		}

		protected override void OnContentResized(RegionChangedEventArgs e)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).OnContentResized(e);
			if (NameLabel != null)
			{
				int centerY = CalculateHeight() / 2;
				((Control)DeleteButton).set_Location(new Point(((Control)this).get_Width() - ((Control)DeleteButton).get_Width() - 20, centerY - ((Control)DeleteButton).get_Height() / 2));
				((Control)Checkbox).set_Location(new Point(((Control)Checkbox).get_Location().X, centerY - ((Control)Checkbox).get_Height() / 2));
				((Control)NameLabel).set_Location(new Point(((Control)Checkbox).get_Width() + 20, 8));
				((Control)NameLabel).set_Width(((Control)this).get_Width() - 8 - ((Control)NameLabel).get_Left() - ((Control)DeleteButton).get_Width());
				((Control)ProfessionLabel).set_Width(((Control)NameLabel).get_Width());
			}
		}
	}
}
