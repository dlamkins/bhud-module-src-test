using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using DanceDanceRotationModule.Model;
using DanceDanceRotationModule.Storage;
using DanceDanceRotationModule.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DanceDanceRotationModule.Views
{
	public class SongInfoView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<SongInfoView>();

		private static readonly Point UtilityIconSize = new Point(72, 72);

		private static readonly Point UtilityRemapIconSize = new Point(48, 48);

		private Song _song;

		private SongData _songData;

		private Label _nameLabel;

		private Label _professionLabel;

		private Label _descriptionLabel;

		private TextBox _buildUrlTextBox;

		private Image _openBuildUrlBuildTemplateButton;

		private Label _buildTemplateLabel;

		private TextBox _buildTemplateTextBox;

		private Image _copyBuildTemplateButton;

		private Label _remapInstructionsText;

		private Image _remapUtilityImage1;

		private Image _remapUtilityImage2;

		private Image _remapUtilityImage3;

		private Image _rotateRemapButton;

		private Label _playbackRateLabel;

		private TrackBar _playbackRateTrackbar;

		private Label _startAtLabel;

		private TrackBar _startAtTrackbar;

		private Label _notePaceLabel;

		private TrackBar _notePaceTrackBar;

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Expected O, but got Unknown
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Expected O, but got Unknown
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Expected O, but got Unknown
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Expected O, but got Unknown
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Expected O, but got Unknown
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Expected O, but got Unknown
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Expected O, but got Unknown
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Expected O, but got Unknown
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Expected O, but got Unknown
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Expected O, but got Unknown
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Expected O, but got Unknown
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_046f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0489: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Expected O, but got Unknown
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d2: Expected O, but got Unknown
			//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0537: Unknown result type (might be due to invalid IL or missing references)
			//IL_053e: Unknown result type (might be due to invalid IL or missing references)
			//IL_054e: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Expected O, but got Unknown
			//IL_0567: Unknown result type (might be due to invalid IL or missing references)
			//IL_056c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0573: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_059b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Expected O, but got Unknown
			//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e3: Expected O, but got Unknown
			//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0606: Unknown result type (might be due to invalid IL or missing references)
			//IL_060d: Unknown result type (might be due to invalid IL or missing references)
			//IL_061d: Unknown result type (might be due to invalid IL or missing references)
			//IL_061e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0630: Unknown result type (might be due to invalid IL or missing references)
			//IL_0635: Unknown result type (might be due to invalid IL or missing references)
			//IL_0640: Unknown result type (might be due to invalid IL or missing references)
			//IL_0648: Unknown result type (might be due to invalid IL or missing references)
			//IL_064f: Unknown result type (might be due to invalid IL or missing references)
			//IL_065f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0660: Unknown result type (might be due to invalid IL or missing references)
			//IL_066a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0677: Expected O, but got Unknown
			//IL_0678: Unknown result type (might be due to invalid IL or missing references)
			//IL_067d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0684: Unknown result type (might be due to invalid IL or missing references)
			//IL_068f: Unknown result type (might be due to invalid IL or missing references)
			//IL_069a: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b9: Expected O, but got Unknown
			//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f4: Expected O, but got Unknown
			//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0704: Unknown result type (might be due to invalid IL or missing references)
			//IL_070f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0717: Unknown result type (might be due to invalid IL or missing references)
			//IL_071e: Unknown result type (might be due to invalid IL or missing references)
			//IL_072e: Unknown result type (might be due to invalid IL or missing references)
			//IL_072f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0741: Unknown result type (might be due to invalid IL or missing references)
			//IL_0746: Unknown result type (might be due to invalid IL or missing references)
			//IL_0751: Unknown result type (might be due to invalid IL or missing references)
			//IL_0759: Unknown result type (might be due to invalid IL or missing references)
			//IL_0760: Unknown result type (might be due to invalid IL or missing references)
			//IL_0770: Unknown result type (might be due to invalid IL or missing references)
			//IL_0771: Unknown result type (might be due to invalid IL or missing references)
			//IL_077b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0788: Expected O, but got Unknown
			//IL_0789: Unknown result type (might be due to invalid IL or missing references)
			//IL_078e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0795: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ca: Expected O, but got Unknown
			//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0803: Unknown result type (might be due to invalid IL or missing references)
			//IL_080a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0811: Unknown result type (might be due to invalid IL or missing references)
			//IL_081c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0826: Unknown result type (might be due to invalid IL or missing references)
			//IL_082b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0835: Unknown result type (might be due to invalid IL or missing references)
			//IL_0840: Unknown result type (might be due to invalid IL or missing references)
			//IL_084a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0855: Unknown result type (might be due to invalid IL or missing references)
			//IL_085c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0865: Expected O, but got Unknown
			//IL_0866: Unknown result type (might be due to invalid IL or missing references)
			//IL_086b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0876: Unknown result type (might be due to invalid IL or missing references)
			//IL_0881: Unknown result type (might be due to invalid IL or missing references)
			//IL_0888: Unknown result type (might be due to invalid IL or missing references)
			//IL_088f: Unknown result type (might be due to invalid IL or missing references)
			//IL_089f: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_08aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b7: Expected O, but got Unknown
			//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e2: Expected O, but got Unknown
			//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0906: Unknown result type (might be due to invalid IL or missing references)
			//IL_0911: Unknown result type (might be due to invalid IL or missing references)
			//IL_091e: Expected O, but got Unknown
			//IL_092e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0933: Unknown result type (might be due to invalid IL or missing references)
			//IL_0934: Unknown result type (might be due to invalid IL or missing references)
			//IL_093e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0949: Unknown result type (might be due to invalid IL or missing references)
			//IL_0956: Expected O, but got Unknown
			//IL_0966: Unknown result type (might be due to invalid IL or missing references)
			//IL_096b: Unknown result type (might be due to invalid IL or missing references)
			//IL_096c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0976: Unknown result type (might be due to invalid IL or missing references)
			//IL_0981: Unknown result type (might be due to invalid IL or missing references)
			//IL_098e: Expected O, but got Unknown
			//IL_098e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0993: Unknown result type (might be due to invalid IL or missing references)
			//IL_0994: Unknown result type (might be due to invalid IL or missing references)
			//IL_099e: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a8: Expected O, but got Unknown
			//IL_09b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_09bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_09be: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a19: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_OuterControlPadding(new Vector2(0f, 0f));
			((Container)val).set_AutoSizePadding(new Point(0, 0));
			((Control)val).set_Parent(buildPanel);
			FlowPanel rootPanel = val;
			int childSectionWidth = ((Control)buildPanel).get_Width();
			FlowPanel val2 = new FlowPanel();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_OuterControlPadding(new Vector2(10f, 10f));
			((Container)val2).set_AutoSizePadding(new Point(10, 10));
			val2.set_ControlPadding(new Vector2(0f, 2f));
			((Control)val2).set_Parent((Container)(object)rootPanel);
			FlowPanel namePanel = val2;
			Label val3 = new Label();
			val3.set_Text("--");
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			val3.set_Font(GameService.Content.get_DefaultFont18());
			((Control)val3).set_Parent((Container)(object)namePanel);
			_nameLabel = val3;
			Label val4 = new Label();
			val4.set_Text("");
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
			val4.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val4).set_Parent((Container)(object)namePanel);
			_professionLabel = val4;
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Width(childSectionWidth);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			val5.set_FlowDirection((ControlFlowDirection)3);
			val5.set_OuterControlPadding(new Vector2(10f, 10f));
			((Container)val5).set_AutoSizePadding(new Point(10, 10));
			val5.set_ControlPadding(new Vector2(0f, 8f));
			((Panel)val5).set_Title("Description");
			((Panel)val5).set_CanCollapse(true);
			((Control)val5).set_Parent((Container)(object)rootPanel);
			FlowPanel descriptionPanel = val5;
			Label val6 = new Label();
			val6.set_Text("--");
			((Control)val6).set_Width(childSectionWidth);
			val6.set_AutoSizeHeight(true);
			val6.set_WrapText(true);
			val6.set_Font(GameService.Content.get_DefaultFont14());
			val6.set_TextColor(Color.get_LightGray());
			((Control)val6).set_Parent((Container)(object)descriptionPanel);
			_descriptionLabel = val6;
			Label val7 = new Label();
			val7.set_Text("");
			((Control)val7).set_Height(4);
			((Control)val7).set_Parent((Container)(object)descriptionPanel);
			Label val8 = new Label();
			val8.set_Text("Build URL");
			val8.set_AutoSizeWidth(true);
			val8.set_AutoSizeHeight(true);
			val8.set_Font(GameService.Content.get_DefaultFont12());
			val8.set_TextColor(Color.get_LightGray());
			((Control)val8).set_Parent((Container)(object)descriptionPanel);
			FlowPanel val9 = new FlowPanel();
			((Container)val9).set_WidthSizingMode((SizingMode)2);
			((Container)val9).set_HeightSizingMode((SizingMode)1);
			val9.set_FlowDirection((ControlFlowDirection)2);
			val9.set_ControlPadding(new Vector2(8f, 0f));
			((Panel)val9).set_CanScroll(false);
			((Control)val9).set_Parent((Container)(object)descriptionPanel);
			FlowPanel buildUrlPanel = val9;
			TextBox val10 = new TextBox();
			((TextInputBase)val10).set_Text("--");
			((Control)val10).set_Enabled(false);
			((TextInputBase)val10).set_Font(GameService.Content.get_DefaultFont12());
			((Control)val10).set_Parent((Container)(object)buildUrlPanel);
			_buildUrlTextBox = val10;
			((TextInputBase)_buildUrlTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				((TextInputBase)_buildUrlTextBox).set_Text(_song?.BuildUrl ?? "");
			});
			Image val11 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.ButtonOpenUrl));
			((Control)val11).set_Size(ControlExtensions.ImageButtonSmallSize);
			((Control)val11).set_Parent((Container)(object)buildUrlPanel);
			_openBuildUrlBuildTemplateButton = val11;
			ControlExtensions.ConvertToButton((Control)(object)_openBuildUrlBuildTemplateButton);
			((Control)_openBuildUrlBuildTemplateButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Info("OpenBuildUrl Clicked");
				if (_song != null && _song.BuildUrl != null)
				{
					UrlHelper.OpenUrl(_song.BuildUrl);
				}
				else
				{
					ScreenNotification.ShowNotification("Failed to open link.", (NotificationType)0, (Texture2D)null, 4);
				}
			});
			Label val12 = new Label();
			val12.set_Text("Build Template");
			val12.set_AutoSizeWidth(true);
			val12.set_AutoSizeHeight(true);
			val12.set_Font(GameService.Content.get_DefaultFont12());
			val12.set_TextColor(Color.get_LightGray());
			((Control)val12).set_Parent((Container)(object)descriptionPanel);
			_buildTemplateLabel = val12;
			FlowPanel val13 = new FlowPanel();
			((Container)val13).set_WidthSizingMode((SizingMode)2);
			((Container)val13).set_HeightSizingMode((SizingMode)1);
			val13.set_FlowDirection((ControlFlowDirection)2);
			val13.set_ControlPadding(new Vector2(8f, 0f));
			((Panel)val13).set_CanScroll(false);
			((Control)val13).set_Parent((Container)(object)descriptionPanel);
			FlowPanel buildLink = val13;
			TextBox val14 = new TextBox();
			((TextInputBase)val14).set_Text("--");
			((Control)val14).set_Enabled(false);
			((TextInputBase)val14).set_Font(GameService.Content.get_DefaultFont12());
			((Control)val14).set_Parent((Container)(object)buildLink);
			_buildTemplateTextBox = val14;
			((TextInputBase)_buildTemplateTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				((TextInputBase)_buildTemplateTextBox).set_Text(_song?.BuildTemplateCode ?? "");
			});
			Image val15 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.ButtonCopy));
			((Control)val15).set_Size(ControlExtensions.ImageButtonSmallSize);
			((Control)val15).set_Parent((Container)(object)buildLink);
			_copyBuildTemplateButton = val15;
			ControlExtensions.ConvertToButton((Control)(object)_copyBuildTemplateButton);
			((Control)_copyBuildTemplateButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Info("CopyBuild Clicked");
				if (_song != null && _song.BuildTemplateCode != null)
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_song.BuildTemplateCode);
					ScreenNotification.ShowNotification("Copied Build to Clipboard", (NotificationType)0, (Texture2D)null, 4);
				}
				else
				{
					ScreenNotification.ShowNotification("No Build Template to Copy", (NotificationType)0, (Texture2D)null, 4);
				}
			});
			FlowPanel val16 = new FlowPanel();
			((Control)val16).set_Width(childSectionWidth);
			((Container)val16).set_HeightSizingMode((SizingMode)1);
			val16.set_OuterControlPadding(new Vector2(10f, 10f));
			((Container)val16).set_AutoSizePadding(new Point(10, 10));
			val16.set_ControlPadding(new Vector2(0f, 10f));
			((Panel)val16).set_Title("Practice Settings");
			((Panel)val16).set_CanCollapse(true);
			((Control)val16).set_Parent((Container)(object)rootPanel);
			FlowPanel practiceSettingsSection = val16;
			FlowPanel val17 = new FlowPanel();
			((Container)val17).set_HeightSizingMode((SizingMode)1);
			((Container)val17).set_WidthSizingMode((SizingMode)2);
			val17.set_FlowDirection((ControlFlowDirection)2);
			((Control)val17).set_Parent((Container)(object)practiceSettingsSection);
			FlowPanel playbackRateSection = val17;
			Label val18 = new Label();
			val18.set_Text("Note Speed");
			((Control)val18).set_BasicTooltipText("Setting under 100% slows down the song, adding more time in between notes.");
			((Control)val18).set_Width(100);
			val18.set_AutoSizeHeight(true);
			val18.set_Font(GameService.Content.get_DefaultFont14());
			val18.set_TextColor(Color.get_LightGray());
			((Control)val18).set_Parent((Container)(object)playbackRateSection);
			Label val19 = new Label();
			val19.set_Text("100%");
			((Control)val19).set_Width(100);
			val19.set_AutoSizeHeight(true);
			val19.set_Font(GameService.Content.get_DefaultFont18());
			val19.set_TextColor(Color.get_White());
			((Control)val19).set_Parent((Container)(object)playbackRateSection);
			_playbackRateLabel = val19;
			TrackBar val20 = new TrackBar();
			((Control)val20).set_Enabled(true);
			val20.set_MinValue(10f);
			val20.set_MaxValue(100f);
			val20.set_Value(100f);
			val20.set_SmallStep(false);
			((Control)val20).set_Parent((Container)(object)practiceSettingsSection);
			_playbackRateTrackbar = val20;
			_playbackRateTrackbar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object sender, ValueEventArgs<float> args)
			{
				if (_song != null)
				{
					DanceDanceRotationModule.SongRepo.UpdateData(_song.Id, delegate(SongData songData)
					{
						songData.PlaybackRate = args.get_Value() / 100f;
						return songData;
					});
				}
				_playbackRateLabel.set_Text($"{args.get_Value()}%");
			});
			FlowPanel val21 = new FlowPanel();
			((Container)val21).set_HeightSizingMode((SizingMode)1);
			((Container)val21).set_WidthSizingMode((SizingMode)2);
			val21.set_FlowDirection((ControlFlowDirection)2);
			((Control)val21).set_Parent((Container)(object)practiceSettingsSection);
			FlowPanel notePaceSection = val21;
			Label val22 = new Label();
			val22.set_Text("Note Pace");
			((Control)val22).set_BasicTooltipText("Sets how fast notes move. This does not affect how fast you have to press buttons.");
			((Control)val22).set_Width(100);
			val22.set_AutoSizeHeight(true);
			val22.set_Font(GameService.Content.get_DefaultFont14());
			val22.set_TextColor(Color.get_LightGray());
			((Control)val22).set_Parent((Container)(object)notePaceSection);
			Label val23 = new Label();
			val23.set_Text("100%");
			((Control)val23).set_Width(100);
			val23.set_AutoSizeHeight(true);
			val23.set_Font(GameService.Content.get_DefaultFont18());
			val23.set_TextColor(Color.get_White());
			((Control)val23).set_Parent((Container)(object)notePaceSection);
			_notePaceLabel = val23;
			TrackBar val24 = new TrackBar();
			((Control)val24).set_Enabled(true);
			val24.set_MinValue(75f);
			val24.set_MaxValue(600f);
			val24.set_SmallStep(false);
			val24.set_Value(300f);
			((Control)val24).set_Parent((Container)(object)practiceSettingsSection);
			_notePaceTrackBar = val24;
			_notePaceTrackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object sender, ValueEventArgs<float> args)
			{
				if (_song != null)
				{
					DanceDanceRotationModule.SongRepo.UpdateData(_song.Id, delegate(SongData songData)
					{
						songData.NotePositionChangePerSecond = (int)args.get_Value();
						return songData;
					});
				}
				int num3 = (int)(args.get_Value() * 100f / 300f);
				_notePaceLabel.set_Text(num3 + "%");
			});
			FlowPanel val25 = new FlowPanel();
			((Container)val25).set_HeightSizingMode((SizingMode)1);
			((Container)val25).set_WidthSizingMode((SizingMode)2);
			val25.set_FlowDirection((ControlFlowDirection)2);
			((Control)val25).set_Parent((Container)(object)practiceSettingsSection);
			FlowPanel startAtSection = val25;
			Label val26 = new Label();
			val26.set_Text("Start At");
			((Control)val26).set_BasicTooltipText("Sets what time the notes start");
			((Control)val26).set_Width(100);
			val26.set_AutoSizeHeight(true);
			val26.set_Font(GameService.Content.get_DefaultFont14());
			val26.set_TextColor(Color.get_LightGray());
			((Control)val26).set_Parent((Container)(object)startAtSection);
			Label val27 = new Label();
			val27.set_Text("0\u2009:\u200900");
			((Control)val27).set_Width(100);
			val27.set_AutoSizeHeight(true);
			val27.set_Font(GameService.Content.get_DefaultFont18());
			val27.set_TextColor(Color.get_White());
			((Control)val27).set_Parent((Container)(object)startAtSection);
			_startAtLabel = val27;
			TrackBar val28 = new TrackBar();
			((Control)val28).set_Enabled(true);
			val28.set_MinValue(0f);
			val28.set_MaxValue(100f);
			val28.set_SmallStep(false);
			val28.set_Value(0f);
			((Control)val28).set_Parent((Container)(object)practiceSettingsSection);
			_startAtTrackbar = val28;
			_startAtTrackbar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object sender, ValueEventArgs<float> args)
			{
				if (_song != null)
				{
					DanceDanceRotationModule.SongRepo.UpdateData(_song.Id, delegate(SongData songData)
					{
						songData.StartAtSecond = (int)args.get_Value();
						return songData;
					});
				}
				int num = (int)args.get_Value() / 60;
				int num2 = (int)args.get_Value() % 60;
				_startAtLabel.set_Text($"{num}\u2009:\u2009{num2:00}");
			});
			Label val29 = new Label();
			val29.set_Text("");
			((Control)val29).set_Height(4);
			((Control)val29).set_Parent((Container)(object)descriptionPanel);
			FlowPanel val30 = new FlowPanel();
			((Control)val30).set_Width(childSectionWidth);
			((Container)val30).set_HeightSizingMode((SizingMode)1);
			val30.set_OuterControlPadding(new Vector2(10f, 10f));
			((Container)val30).set_AutoSizePadding(new Point(10, 10));
			val30.set_ControlPadding(new Vector2(0f, 10f));
			((Panel)val30).set_Title("Remap Utility Skills");
			((Panel)val30).set_CanCollapse(true);
			((Control)val30).set_Parent((Container)(object)rootPanel);
			FlowPanel utilityRemap = val30;
			Label val31 = new Label();
			val31.set_Text("Set the utility icon positions you use if you prefer utility icons in different positions than the song's build template");
			((Control)val31).set_Width(280);
			val31.set_AutoSizeHeight(true);
			val31.set_WrapText(true);
			val31.set_Font(GameService.Content.get_DefaultFont14());
			val31.set_TextColor(Color.get_LightGray());
			((Control)val31).set_Parent((Container)(object)utilityRemap);
			_remapInstructionsText = val31;
			FlowPanel val32 = new FlowPanel();
			((Container)val32).set_WidthSizingMode((SizingMode)2);
			((Container)val32).set_HeightSizingMode((SizingMode)1);
			val32.set_FlowDirection((ControlFlowDirection)2);
			((Panel)val32).set_CanScroll(false);
			((Control)val32).set_Parent((Container)(object)utilityRemap);
			FlowPanel remapIcons = val32;
			Image val33 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.UnknownAbilityIcon));
			((Control)val33).set_Size(new Point(72, 72));
			((Control)val33).set_BasicTooltipText("This should match in-game utility slot 1");
			((Control)val33).set_Parent((Container)(object)remapIcons);
			_remapUtilityImage1 = val33;
			Image val34 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.UnknownAbilityIcon));
			((Control)val34).set_Size(UtilityIconSize);
			((Control)val34).set_BasicTooltipText("This should match in-game utility slot 2");
			((Control)val34).set_Parent((Container)(object)remapIcons);
			_remapUtilityImage2 = val34;
			Image val35 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.UnknownAbilityIcon));
			((Control)val35).set_Size(UtilityIconSize);
			((Control)val35).set_BasicTooltipText("This should match in-game utility slot 3");
			((Control)val35).set_Parent((Container)(object)remapIcons);
			_remapUtilityImage3 = val35;
			Panel val36 = new Panel();
			((Control)val36).set_Size(UtilityIconSize);
			((Control)val36).set_Parent((Container)(object)remapIcons);
			Panel rotateRemapButtonPanel = val36;
			Image val37 = new Image(AsyncTexture2D.op_Implicit(Resources.Instance.ButtonReload));
			((Control)val37).set_Size(UtilityRemapIconSize);
			((Control)val37).set_Location(new Point((UtilityIconSize.X - UtilityRemapIconSize.X) / 2, (UtilityIconSize.Y - UtilityRemapIconSize.Y) / 2));
			((Control)val37).set_BasicTooltipText("Change the remapping ordering.");
			((Control)val37).set_Parent((Container)(object)rotateRemapButtonPanel);
			_rotateRemapButton = val37;
			((Control)_rotateRemapButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Info("RotateRemapping Clicked");
				if (_song != null)
				{
					DanceDanceRotationModule.SongRepo.UpdateData(_song.Id, delegate(SongData songData)
					{
						switch (songData.Utility1Mapping)
						{
						case SongData.UtilitySkillMapping.One:
							switch (songData.Utility2Mapping)
							{
							case SongData.UtilitySkillMapping.Two:
								songData.Utility1Mapping = SongData.UtilitySkillMapping.One;
								songData.Utility2Mapping = SongData.UtilitySkillMapping.Three;
								songData.Utility3Mapping = SongData.UtilitySkillMapping.Two;
								break;
							case SongData.UtilitySkillMapping.Three:
								songData.Utility1Mapping = SongData.UtilitySkillMapping.Two;
								songData.Utility2Mapping = SongData.UtilitySkillMapping.One;
								songData.Utility3Mapping = SongData.UtilitySkillMapping.Three;
								break;
							}
							break;
						case SongData.UtilitySkillMapping.Two:
							switch (songData.Utility2Mapping)
							{
							case SongData.UtilitySkillMapping.One:
								songData.Utility1Mapping = SongData.UtilitySkillMapping.Two;
								songData.Utility2Mapping = SongData.UtilitySkillMapping.Three;
								songData.Utility3Mapping = SongData.UtilitySkillMapping.One;
								break;
							case SongData.UtilitySkillMapping.Three:
								songData.Utility1Mapping = SongData.UtilitySkillMapping.Three;
								songData.Utility2Mapping = SongData.UtilitySkillMapping.One;
								songData.Utility3Mapping = SongData.UtilitySkillMapping.Two;
								break;
							}
							break;
						case SongData.UtilitySkillMapping.Three:
							switch (songData.Utility2Mapping)
							{
							case SongData.UtilitySkillMapping.One:
								songData.Utility1Mapping = SongData.UtilitySkillMapping.Three;
								songData.Utility2Mapping = SongData.UtilitySkillMapping.Two;
								songData.Utility3Mapping = SongData.UtilitySkillMapping.One;
								break;
							case SongData.UtilitySkillMapping.Two:
								songData.Utility1Mapping = SongData.UtilitySkillMapping.One;
								songData.Utility2Mapping = SongData.UtilitySkillMapping.Two;
								songData.Utility3Mapping = SongData.UtilitySkillMapping.Three;
								break;
							}
							break;
						}
						return songData;
					});
				}
			});
			DanceDanceRotationModule.SongRepo.OnSelectedSongChanged += delegate(object sender, SelectedSongInfo songInfo)
			{
				OnSelectedSongChanged(songInfo);
			};
		}

		private void OnSelectedSongChanged(SelectedSongInfo songInfo)
		{
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			Logger.Trace("OnSelectedSongChanged: " + songInfo.Song?.Id.Name);
			_song = songInfo.Song;
			_songData = songInfo.Data;
			if (_song == null)
			{
				_nameLabel.set_Text("--");
				_descriptionLabel.set_Text("--");
				_professionLabel.set_Text("");
				((TextInputBase)_buildUrlTextBox).set_Text("--");
				((TextInputBase)_buildTemplateTextBox).set_Text("--");
				((Control)_copyBuildTemplateButton).set_Visible(false);
				((Control)_openBuildUrlBuildTemplateButton).set_Visible(false);
				_remapUtilityImage1.set_Texture(AsyncTexture2D.op_Implicit(Resources.Instance.UnknownAbilityIcon));
				_remapUtilityImage2.set_Texture(AsyncTexture2D.op_Implicit(Resources.Instance.UnknownAbilityIcon));
				_remapUtilityImage3.set_Texture(AsyncTexture2D.op_Implicit(Resources.Instance.UnknownAbilityIcon));
				SetRemappingEnabled();
				_playbackRateTrackbar.set_Value(100f);
				_startAtTrackbar.set_Value(0f);
				_notePaceTrackBar.set_Value(300f);
				return;
			}
			_nameLabel.set_Text(_song.Name ?? "<no name>");
			_descriptionLabel.set_Text(_song.Description ?? "");
			_professionLabel.set_Text(_song.EliteName);
			_professionLabel.set_TextColor(ProfessionExtensions.GetProfessionColor(_song.Profession));
			((TextInputBase)_buildUrlTextBox).set_Text(_song.BuildUrl ?? "--");
			((Control)_openBuildUrlBuildTemplateButton).set_Visible(!string.IsNullOrEmpty(_song.BuildUrl));
			((Control)_buildTemplateLabel).set_Visible(!string.IsNullOrEmpty(_song.BuildTemplateCode));
			((Control)_buildTemplateTextBox).set_Visible(!string.IsNullOrEmpty(_song.BuildTemplateCode));
			((TextInputBase)_buildTemplateTextBox).set_Text(_song.BuildTemplateCode ?? "--");
			((Control)_copyBuildTemplateButton).set_Visible(!string.IsNullOrEmpty(_song.BuildTemplateCode));
			switch (_song.Profession)
			{
			case Profession.Common:
				DisableRemapping("Utility remapping is disabled.", Color.get_LightGray());
				break;
			case Profession.Revenant:
				DisableRemapping("Remapping utilities is currently disabled for Revenant. Utility skills on both Legends must match the song's build.", Color.get_Tomato());
				break;
			default:
				SetRemappingEnabled();
				break;
			}
			GetRemappedAbilityTexture(_song.Utility1, _songData.Utility1Mapping);
			GetRemappedAbilityTexture(_song.Utility2, _songData.Utility2Mapping);
			GetRemappedAbilityTexture(_song.Utility3, _songData.Utility3Mapping);
			_playbackRateTrackbar.set_Value((float)(int)Math.Round(_songData.PlaybackRate * 100f));
			_startAtTrackbar.set_Value((float)_songData.StartAtSecond);
			_startAtTrackbar.set_MaxValue((float)(int)_song.Notes.LastOrDefault().TimeInRotation.TotalSeconds);
			_notePaceTrackBar.set_Value((float)_songData.NotePositionChangePerSecond);
		}

		private void SetRemappingEnabled()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			((Control)_rotateRemapButton).set_Visible(true);
			_remapInstructionsText.set_Text("Set the utility icon positions you use if you prefer utility icons in different positions than the song's build template.");
			_remapInstructionsText.set_TextColor(Color.get_LightGray());
			((Control)_remapUtilityImage1).set_Visible(true);
			((Control)_remapUtilityImage2).set_Visible(true);
			((Control)_remapUtilityImage3).set_Visible(true);
		}

		private void DisableRemapping(string reason, Color textColor)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			((Control)_rotateRemapButton).set_Visible(false);
			_remapInstructionsText.set_Text(reason);
			_remapInstructionsText.set_TextColor(textColor);
			((Control)_remapUtilityImage1).set_Visible(false);
			((Control)_remapUtilityImage2).set_Visible(false);
			((Control)_remapUtilityImage3).set_Visible(false);
		}

		private void GetRemappedAbilityTexture(PaletteId paletteId, SongData.UtilitySkillMapping skillMapping)
		{
			Image image;
			switch (skillMapping)
			{
			default:
				return;
			case SongData.UtilitySkillMapping.One:
				image = _remapUtilityImage1;
				break;
			case SongData.UtilitySkillMapping.Two:
				image = _remapUtilityImage2;
				break;
			case SongData.UtilitySkillMapping.Three:
				image = _remapUtilityImage3;
				break;
			}
			image.set_Texture(Resources.Instance.GetAbilityIcon(paletteId));
		}

		public SongInfoView()
			: this()
		{
		}
	}
}
