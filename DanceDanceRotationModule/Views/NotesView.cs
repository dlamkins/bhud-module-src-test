using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using DanceDanceRotationModule.Storage;
using DanceDanceRotationModule.Util;
using Microsoft.Xna.Framework;

namespace DanceDanceRotationModule.Views
{
	public class NotesView : View
	{
		private NotesContainer _notesContainer;

		private FlowPanel _topPanel;

		protected override void Build(Container buildPanel)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Expected O, but got Unknown
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Expected O, but got Unknown
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Expected O, but got Unknown
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Expected O, but got Unknown
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Expected O, but got Unknown
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Expected O, but got Unknown
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Expected O, but got Unknown
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_CanScroll(false);
			((Control)val).set_Parent(buildPanel);
			Panel rootPanel = val;
			Panel val2 = new Panel();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)2);
			val2.set_CanScroll(false);
			val2.set_BackgroundTexture(AsyncTexture2D.op_Implicit(Resources.Instance.NotesBg));
			((Control)val2).set_ZIndex(-2);
			((Control)val2).set_Parent(buildPanel);
			Panel backgroundPanel = val2;
			Panel val3 = new Panel();
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			val3.set_CanScroll(false);
			val3.set_BackgroundTexture(AsyncTexture2D.op_Implicit(Resources.Instance.NotesControlsBg));
			((Control)val3).set_ZIndex(-1);
			((Control)val3).set_Parent(buildPanel);
			Panel topPanelBackground = val3;
			DanceDanceRotationModule.Settings.BackgroundOpacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)delegate(object sender, ValueChangedEventArgs<float> args)
			{
				((Control)backgroundPanel).set_Opacity(args.get_NewValue());
				((Control)topPanelBackground).set_Opacity(1f - args.get_NewValue());
			});
			((Control)backgroundPanel).set_Opacity(DanceDanceRotationModule.Settings.BackgroundOpacity.get_Value());
			((Control)topPanelBackground).set_Opacity(1f - ((Control)backgroundPanel).get_Opacity());
			FlowPanel val4 = new FlowPanel();
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Container)val4).set_HeightSizingMode((SizingMode)2);
			val4.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val4).set_CanScroll(false);
			((Control)val4).set_ZIndex(5);
			((Control)val4).set_Parent((Container)(object)rootPanel);
			FlowPanel flowPanel = val4;
			FlowPanel val5 = new FlowPanel();
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			val5.set_FlowDirection((ControlFlowDirection)2);
			val5.set_OuterControlPadding(new Vector2(10f, 10f));
			((Container)val5).set_AutoSizePadding(new Point(10, 10));
			val5.set_ControlPadding(new Vector2(4f, 0f));
			((Panel)val5).set_CanScroll(false);
			((Control)val5).set_Parent((Container)(object)flowPanel);
			_topPanel = val5;
			((Control)_topPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				((Control)topPanelBackground).set_Location(((Control)_topPanel).get_Location());
				((Control)topPanelBackground).set_Height(((Control)_topPanel).get_Height());
			});
			Image val6 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.ButtonList));
			((Control)val6).set_Size(ControlExtensions.ImageButtonSmallSize);
			((Control)val6).set_Parent((Container)(object)_topPanel);
			ControlExtensions.ConvertToButton((Control)val6);
			((Control)val6).set_BasicTooltipText("Song List");
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				DanceDanceRotationModule.Instance.ToggleSongList();
			});
			Image val7 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.ButtonDetails));
			((Control)val7).set_Size(ControlExtensions.ImageButtonSmallSize);
			((Control)val7).set_Parent((Container)(object)_topPanel);
			ControlExtensions.ConvertToButton((Control)val7);
			((Control)val7).set_BasicTooltipText("Song Info");
			((Control)val7).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				DanceDanceRotationModule.Instance.ToggleSongInfo();
			});
			Image val8 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.ButtonStop));
			((Control)val8).set_Size(ControlExtensions.ImageButtonSmallSize);
			((Control)val8).set_Parent((Container)(object)_topPanel);
			ControlExtensions.ConvertToButton((Control)val8);
			((Control)val8).set_BasicTooltipText("Reset");
			((Control)val8).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_notesContainer.Stop();
			});
			Image val9 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.ButtonPlay));
			((Control)val9).set_Size(ControlExtensions.ImageButtonSmallSize);
			((Control)val9).set_Parent((Container)(object)_topPanel);
			Image playPauseButton = val9;
			ControlExtensions.ConvertToButton((Control)(object)playPauseButton);
			((Control)playPauseButton).set_BasicTooltipText("Play");
			((Control)playPauseButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!_notesContainer.IsStarted() || _notesContainer.IsPaused())
				{
					_notesContainer.Play();
					playPauseButton.set_Texture(AsyncTexture2D.op_Implicit(Resources.Instance.ButtonPause));
					((Control)playPauseButton).set_BasicTooltipText("Pause");
				}
				else
				{
					_notesContainer.Pause();
					playPauseButton.set_Texture(AsyncTexture2D.op_Implicit(Resources.Instance.ButtonPlay));
					((Control)playPauseButton).set_BasicTooltipText("Play");
				}
			});
			Label val10 = new Label();
			val10.set_Text("");
			((Control)val10).set_Width(1000);
			val10.set_AutoSizeHeight(true);
			val10.set_Font(GameService.Content.get_DefaultFont18());
			((Control)val10).set_Parent((Container)(object)_topPanel);
			Label activeSongName = val10;
			ControlExtensions.ConvertToButton((Control)(object)activeSongName);
			((Control)activeSongName).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				DanceDanceRotationModule.Instance.ToggleSongInfo();
			});
			DanceDanceRotationModule.SongRepo.OnSelectedSongChanged += delegate(object sender, SelectedSongInfo songInfo)
			{
				activeSongName.set_Text((songInfo.Song != null) ? songInfo.Song.Name : "--");
			};
			NotesContainer notesContainer = new NotesContainer();
			((Container)notesContainer).set_WidthSizingMode((SizingMode)2);
			((Container)notesContainer).set_HeightSizingMode((SizingMode)2);
			((Container)notesContainer).set_AutoSizePadding(new Point(12, 12));
			((Control)notesContainer).set_Parent((Container)(object)flowPanel);
			_notesContainer = notesContainer;
			_notesContainer.OnStartStop += delegate(object sender, bool isStarted)
			{
				if (!isStarted)
				{
					playPauseButton.set_Texture(AsyncTexture2D.op_Implicit(Resources.Instance.ButtonPlay));
				}
			};
		}

		public void Update(GameTime gameTime)
		{
			_notesContainer?.UpdateNotes(gameTime);
		}

		public NotesContainer GetNotesContainer()
		{
			return _notesContainer;
		}

		protected override void Unload()
		{
			((View<IPresenter>)this).Unload();
			((Control)_notesContainer).Dispose();
			((Control)_topPanel).Dispose();
		}

		public NotesView()
			: this()
		{
		}
	}
}
