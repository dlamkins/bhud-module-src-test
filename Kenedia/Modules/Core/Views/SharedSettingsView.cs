using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.GameIntegration.GfxSettings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Views
{
	public class SharedSettingsView : BaseTab
	{
		private NumberBox _topOffsetBox;

		private NumberBox _leftOffsetBox;

		private NumberBox _rightOffsetBox;

		private NumberBox _bottomOffsetBox;

		private Kenedia.Modules.Core.Controls.Image _topLeftImage;

		private Kenedia.Modules.Core.Controls.Image _topRightImage;

		private Kenedia.Modules.Core.Controls.Image _bottomLeftImage;

		private Kenedia.Modules.Core.Controls.Image _bottomRightImage;

		public SharedSettings SharedSettings { get; }

		public ClientWindowService ClientWindowService { get; }

		public SharedSettingsView(SharedSettings sharedSettings, ClientWindowService clientWindowService)
		{
			SharedSettings = sharedSettings;
			ClientWindowService = clientWindowService;
			base.Icon = AsyncTexture2D.FromAssetId(156736);
			base.Name = strings_common.GeneralSettings;
			base.Priority = 0;
		}

		public override void CreateLayout(Container p, int? width = null)
		{
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0546: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0637: Unknown result type (might be due to invalid IL or missing references)
			//IL_0651: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_070a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0724: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07be: Unknown result type (might be due to invalid IL or missing references)
			base.ContentContainer = p;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent(p);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			FlowPanel mcFP = flowPanel;
			TitleHeader obj = new TitleHeader
			{
				SetLocalizedTitle = () => strings_common.WindowBorders,
				SetLocalizedTooltip = () => strings_common.WindowBorder_Tooltip
			};
			((Control)obj).set_Height(25);
			((Control)obj).set_Width(width ?? ((Control)p).get_Width());
			((Control)obj).set_Parent((Container)(object)mcFP);
			FlowPanel flowPanel2 = new FlowPanel();
			((Control)flowPanel2).set_Parent((Container)(object)mcFP);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel2).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel2).set_FlowDirection((ControlFlowDirection)2);
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(3f, 3f));
			((FlowPanel)flowPanel2).set_OuterControlPadding(new Vector2(5f));
			FlowPanel cFP = flowPanel2;
			FlowPanel flowPanel3 = new FlowPanel();
			((Control)flowPanel3).set_Parent((Container)(object)cFP);
			((Container)flowPanel3).set_HeightSizingMode((SizingMode)1);
			((Control)flowPanel3).set_Width((width ?? ((Control)p).get_Width()) - 20 - 225);
			((FlowPanel)flowPanel3).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel3).set_ControlPadding(new Vector2(3f, 3f));
			FlowPanel cP = flowPanel3;
			FlowPanel flowPanel4 = new FlowPanel();
			((Control)flowPanel4).set_Parent((Container)(object)cP);
			((Container)flowPanel4).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel4).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel4).set_FlowDirection((ControlFlowDirection)2);
			FlowPanel pp = flowPanel4;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)pp);
			((Control)label).set_Width(165);
			((Control)label).set_Location(new Point(35, 0));
			((Control)label).set_Height(20);
			label.SetLocalizedText = () => strings_common.TopOffset;
			NumberBox numberBox = new NumberBox();
			((Control)numberBox).set_Parent((Container)(object)pp);
			numberBox.MinValue = -50;
			numberBox.MaxValue = 50;
			numberBox.Value = SharedSettings.WindowOffset.Top;
			numberBox.SetLocalizedTooltip = () => strings_common.TopOffset;
			numberBox.ValueChangedAction = delegate
			{
				UpdateOffset();
			};
			_topOffsetBox = numberBox;
			FlowPanel flowPanel5 = new FlowPanel();
			((Control)flowPanel5).set_Parent((Container)(object)cP);
			((Container)flowPanel5).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel5).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel5).set_FlowDirection((ControlFlowDirection)2);
			pp = flowPanel5;
			Label label2 = new Label();
			((Control)label2).set_Parent((Container)(object)pp);
			((Control)label2).set_Width(165);
			((Control)label2).set_Location(new Point(35, 0));
			((Control)label2).set_Height(20);
			label2.SetLocalizedText = () => strings_common.LeftOffset;
			NumberBox numberBox2 = new NumberBox();
			((Control)numberBox2).set_Parent((Container)(object)pp);
			numberBox2.MinValue = -50;
			numberBox2.MaxValue = 50;
			numberBox2.Value = SharedSettings.WindowOffset.Left;
			numberBox2.SetLocalizedTooltip = () => strings_common.LeftOffset;
			numberBox2.ValueChangedAction = delegate
			{
				UpdateOffset();
			};
			_leftOffsetBox = numberBox2;
			FlowPanel flowPanel6 = new FlowPanel();
			((Control)flowPanel6).set_Parent((Container)(object)cP);
			((Container)flowPanel6).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel6).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel6).set_FlowDirection((ControlFlowDirection)2);
			pp = flowPanel6;
			Label label3 = new Label();
			((Control)label3).set_Parent((Container)(object)pp);
			((Control)label3).set_Width(165);
			((Control)label3).set_Location(new Point(35, 0));
			((Control)label3).set_Height(20);
			label3.SetLocalizedText = () => strings_common.BottomOffset;
			NumberBox numberBox3 = new NumberBox();
			((Control)numberBox3).set_Parent((Container)(object)pp);
			numberBox3.MinValue = -50;
			numberBox3.MaxValue = 50;
			numberBox3.Value = SharedSettings.WindowOffset.Bottom;
			numberBox3.SetLocalizedTooltip = () => strings_common.BottomOffset;
			numberBox3.ValueChangedAction = delegate
			{
				UpdateOffset();
			};
			_bottomOffsetBox = numberBox3;
			FlowPanel flowPanel7 = new FlowPanel();
			((Control)flowPanel7).set_Parent((Container)(object)cP);
			((Container)flowPanel7).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel7).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel7).set_FlowDirection((ControlFlowDirection)2);
			pp = flowPanel7;
			Label label4 = new Label();
			((Control)label4).set_Parent((Container)(object)pp);
			((Control)label4).set_Width(165);
			((Control)label4).set_Location(new Point(35, 0));
			((Control)label4).set_Height(20);
			label4.SetLocalizedText = () => strings_common.RightOffset;
			NumberBox numberBox4 = new NumberBox();
			((Control)numberBox4).set_Parent((Container)(object)pp);
			numberBox4.MinValue = -50;
			numberBox4.MaxValue = 50;
			numberBox4.Value = SharedSettings.WindowOffset.Right;
			numberBox4.SetLocalizedTooltip = () => strings_common.RightOffset;
			numberBox4.ValueChangedAction = delegate
			{
				UpdateOffset();
			};
			_rightOffsetBox = numberBox4;
			FlowPanel flowPanel8 = new FlowPanel();
			((Control)flowPanel8).set_Parent((Container)(object)cFP);
			((Container)flowPanel8).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel8).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel8).set_FlowDirection((ControlFlowDirection)2);
			((FlowPanel)flowPanel8).set_ControlPadding(new Vector2(5f, 5f));
			FlowPanel subCP = flowPanel8;
			FlowPanel flowPanel9 = new FlowPanel();
			((Control)flowPanel9).set_Parent((Container)(object)subCP);
			((Container)flowPanel9).set_HeightSizingMode((SizingMode)1);
			((Control)flowPanel9).set_Width(125);
			((FlowPanel)flowPanel9).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel9).set_ControlPadding(new Vector2(5f, 5f));
			cP = flowPanel9;
			Label label5 = new Label();
			((Control)label5).set_Parent((Container)(object)cP);
			label5.SetLocalizedText = () => strings_common.TopLeftCorner;
			((Label)label5).set_AutoSizeWidth(true);
			((Control)label5).set_Visible(false);
			Kenedia.Modules.Core.Controls.Image image = new Kenedia.Modules.Core.Controls.Image();
			((Control)image).set_Parent((Container)(object)cP);
			((Control)image).set_BackgroundColor(Color.get_White());
			((Control)image).set_Size(new Point(100, ((Control)_rightOffsetBox).get_Height() * 2));
			image.SetLocalizedTooltip = () => strings_common.TopLeftCorner;
			_topLeftImage = image;
			Label label6 = new Label();
			((Control)label6).set_Parent((Container)(object)cP);
			label6.SetLocalizedText = () => strings_common.BottomLeftCorner;
			((Label)label6).set_AutoSizeWidth(true);
			((Control)label6).set_Visible(false);
			Kenedia.Modules.Core.Controls.Image image2 = new Kenedia.Modules.Core.Controls.Image();
			((Control)image2).set_Parent((Container)(object)cP);
			((Control)image2).set_BackgroundColor(Color.get_White());
			((Control)image2).set_Size(new Point(100, ((Control)_rightOffsetBox).get_Height() * 2));
			image2.SetLocalizedTooltip = () => strings_common.BottomLeftCorner;
			_bottomLeftImage = image2;
			FlowPanel flowPanel10 = new FlowPanel();
			((Control)flowPanel10).set_Parent((Container)(object)subCP);
			((Container)flowPanel10).set_HeightSizingMode((SizingMode)1);
			((Control)flowPanel10).set_Width(125);
			((FlowPanel)flowPanel10).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel10).set_ControlPadding(new Vector2(5f, 5f));
			cP = flowPanel10;
			Label label7 = new Label();
			((Control)label7).set_Parent((Container)(object)cP);
			label7.SetLocalizedText = () => strings_common.TopRightCorner;
			((Label)label7).set_AutoSizeWidth(true);
			((Control)label7).set_Visible(false);
			Kenedia.Modules.Core.Controls.Image image3 = new Kenedia.Modules.Core.Controls.Image();
			((Control)image3).set_Parent((Container)(object)cP);
			((Control)image3).set_BackgroundColor(Color.get_White());
			((Control)image3).set_Size(new Point(100, ((Control)_rightOffsetBox).get_Height() * 2));
			image3.SetLocalizedTooltip = () => strings_common.TopRightCorner;
			_topRightImage = image3;
			Label label8 = new Label();
			((Control)label8).set_Parent((Container)(object)cP);
			label8.SetLocalizedText = () => strings_common.BottomRightCorner;
			((Label)label8).set_AutoSizeWidth(true);
			((Control)label8).set_Visible(false);
			Kenedia.Modules.Core.Controls.Image image4 = new Kenedia.Modules.Core.Controls.Image();
			((Control)image4).set_Parent((Container)(object)cP);
			((Control)image4).set_BackgroundColor(Color.get_White());
			((Control)image4).set_Size(new Point(100, ((Control)_rightOffsetBox).get_Height() * 2));
			image4.SetLocalizedTooltip = () => strings_common.BottomRightCorner;
			_bottomRightImage = image4;
		}

		public void UpdateOffset()
		{
			if (_leftOffsetBox != null)
			{
				SharedSettings.WindowOffset = new RectangleDimensions(_leftOffsetBox.Value, _topOffsetBox.Value, _rightOffsetBox.Value, _bottomOffsetBox.Value);
				SetTopLeftImage();
				SetTopRightImage();
				SetBottomLeftImage();
				SetBottomRightImage();
			}
		}

		private void SetTopLeftImage()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
			Point p = (Point)(((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed())) ? new Point(SharedSettings.WindowOffset.Left, SharedSettings.WindowOffset.Top) : Point.get_Zero());
			using Bitmap bitmap = new Bitmap(((Control)_topLeftImage).get_Width(), ((Control)_topLeftImage).get_Height());
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new Point(wndBounds.Left + p.X, wndBounds.Top + p.Y), Point.Empty, new Size(((Control)_topLeftImage).get_Width(), ((Control)_topLeftImage).get_Height()));
			bitmap.Save(s, ImageFormat.Bmp);
			((Image)_topLeftImage).set_Texture(AsyncTexture2D.op_Implicit(s.CreateTexture2D()));
		}

		private void SetBottomLeftImage()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
			Point p = (Point)(((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed())) ? new Point(SharedSettings.WindowOffset.Left, SharedSettings.WindowOffset.Bottom) : Point.get_Zero());
			using Bitmap bitmap = new Bitmap(((Control)_bottomLeftImage).get_Width(), ((Control)_bottomLeftImage).get_Height());
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new Point(wndBounds.Left + p.X, wndBounds.Bottom - ((Control)_bottomLeftImage).get_Height() + p.Y), Point.Empty, new Size(((Control)_bottomLeftImage).get_Width(), ((Control)_bottomLeftImage).get_Height()));
			bitmap.Save(s, ImageFormat.Bmp);
			((Image)_bottomLeftImage).set_Texture(AsyncTexture2D.op_Implicit(s.CreateTexture2D()));
		}

		private void SetTopRightImage()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
			Point p = (Point)(((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed())) ? new Point(SharedSettings.WindowOffset.Right, SharedSettings.WindowOffset.Top) : Point.get_Zero());
			using Bitmap bitmap = new Bitmap(((Control)_topRightImage).get_Width(), ((Control)_topRightImage).get_Height());
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new Point(wndBounds.Right - ((Control)_topRightImage).get_Width() + p.X, wndBounds.Top + p.Y), Point.Empty, new Size(((Control)_topRightImage).get_Width(), ((Control)_topRightImage).get_Height()));
			bitmap.Save(s, ImageFormat.Bmp);
			((Image)_topRightImage).set_Texture(AsyncTexture2D.op_Implicit(s.CreateTexture2D()));
		}

		private void SetBottomRightImage()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
			Point p = (Point)(((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed())) ? new Point(SharedSettings.WindowOffset.Right, SharedSettings.WindowOffset.Bottom) : Point.get_Zero());
			using Bitmap bitmap = new Bitmap(((Control)_bottomLeftImage).get_Width(), ((Control)_bottomLeftImage).get_Height());
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new Point(wndBounds.Right - ((Control)_bottomRightImage).get_Width() + p.X, wndBounds.Bottom - ((Control)_bottomRightImage).get_Height() + p.Y), Point.Empty, new Size(((Control)_bottomRightImage).get_Width(), ((Control)_bottomRightImage).get_Height()));
			bitmap.Save(s, ImageFormat.Bmp);
			((Image)_bottomRightImage).set_Texture(AsyncTexture2D.op_Implicit(s.CreateTexture2D()));
		}
	}
}
