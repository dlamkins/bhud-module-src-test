using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using DanceDanceRotationModule.Storage;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace DanceDanceRotationModule.Views
{
	public class HelpView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<HelpView>();

		protected override void Build(Container buildPanel)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			Logger.Info("Loading Help View");
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			Panel rootPanel = val;
			CreateHelpSetion((Container)(object)rootPanel, new Point(10, 10), "help/setHotkeys.png", "Set Hotkeys", "in Blish Settings", "");
			((Control)CreateHelpSetion((Container)(object)rootPanel, new Point(280, 10), "help/selectSong.png", "Select Song", "", "Open")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Info("HelpView - ShowSongList selected");
				DanceDanceRotationModule.Instance.ShowSongList();
			});
			StandardButton openNotesContainerButton = CreateHelpSetion((Container)(object)rootPanel, new Point(550, 10), "help/pressPlay.png", "Press Play", "", "Open");
			((Control)openNotesContainerButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Info("HelpView - ShowNotesWindow selected");
				DanceDanceRotationModule.Instance.ShowNotesWindow();
			});
			((Control)openNotesContainerButton).set_Enabled(false);
			DanceDanceRotationModule.SongRepo.OnSelectedSongChanged += SettingsChangedDelegate;
			void SettingsChangedDelegate(object sender, SelectedSongInfo info)
			{
				if (info.Song != null && !DanceDanceRotationModule.Settings.HasShownInitialSongInfo.get_Value())
				{
					Logger.Info("The first ever song was selected (" + info.Song.Id.Name + "). Showing song info and enabling the open notes helper button.");
					((Control)openNotesContainerButton).set_Enabled(info.Song != null);
					DanceDanceRotationModule.Settings.HasShownInitialSongInfo.set_Value(true);
					DanceDanceRotationModule.Instance.ShowSongInfo();
					DanceDanceRotationModule.SongRepo.OnSelectedSongChanged -= SettingsChangedDelegate;
				}
			}
		}

		private StandardButton CreateHelpSetion(Container rootPanel, Point imageLocation, string imageTextureName, string titleString, string subtitleString, string buttonString)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Expected O, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Expected O, but got Unknown
			Color titleTextColor = Colors.ColonialWhite;
			BitmapFont titleFont = GameService.Content.get_DefaultFont32();
			Color subtitleTextColor = Color.get_White();
			BitmapFont subtitleFont = GameService.Content.get_DefaultFont18();
			int centerPoint = imageLocation.X + 125;
			Image val = new Image(AsyncTexture2D.op_Implicit(DanceDanceRotationModule.Instance.ContentsManager.GetTexture(imageTextureName)));
			((Control)val).set_Size(new Point(250, 150));
			((Control)val).set_Location(imageLocation);
			((Control)val).set_Parent(rootPanel);
			int yPosition = ((Control)val).get_Bottom() + 20;
			Label val2 = new Label();
			val2.set_Text(titleString);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Location(new Point(centerPoint, yPosition));
			val2.set_Font(titleFont);
			val2.set_TextColor(titleTextColor);
			((Control)val2).set_Parent(rootPanel);
			Label title = val2;
			((Control)title).set_Left(((Control)title).get_Left() - ((Control)title).get_Width() / 2);
			yPosition = ((Control)title).get_Bottom() + 8;
			if (subtitleString.Length > 0)
			{
				Label val3 = new Label();
				val3.set_Text(subtitleString);
				((Control)val3).set_Height(28);
				val3.set_AutoSizeWidth(true);
				((Control)val3).set_ClipsBounds(true);
				((Control)val3).set_Location(new Point(centerPoint, yPosition));
				val3.set_Font(subtitleFont);
				val3.set_TextColor(subtitleTextColor);
				((Control)val3).set_Parent(rootPanel);
				Label subtitle = val3;
				((Control)subtitle).set_Left(((Control)subtitle).get_Left() - ((Control)subtitle).get_Width() / 2);
			}
			if (buttonString.Length > 0)
			{
				StandardButton val4 = new StandardButton();
				val4.set_Text(buttonString);
				((Control)val4).set_Location(new Point(centerPoint, yPosition));
				((Control)val4).set_Parent(rootPanel);
				StandardButton button = val4;
				((Control)button).set_Left(((Control)button).get_Left() - ((Control)button).get_Width() / 2);
				return button;
			}
			return null;
		}

		public HelpView()
			: this()
		{
		}
	}
}
