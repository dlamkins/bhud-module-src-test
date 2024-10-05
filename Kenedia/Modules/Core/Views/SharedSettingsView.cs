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
			Kenedia.Modules.Core.Controls.FlowPanel mcFP = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = p,
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom
			};
			new TitleHeader
			{
				SetLocalizedTitle = () => strings_common.WindowBorders,
				SetLocalizedTooltip = () => strings_common.WindowBorder_Tooltip,
				Height = 25,
				Width = (width ?? p.Width),
				Parent = mcFP
			};
			Kenedia.Modules.Core.Controls.FlowPanel cFP = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = mcFP,
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				ControlPadding = new Vector2(3f, 3f),
				OuterControlPadding = new Vector2(5f)
			};
			Kenedia.Modules.Core.Controls.FlowPanel cP = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = cFP,
				HeightSizingMode = SizingMode.AutoSize,
				Width = (width ?? p.Width) - 20 - 225,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(3f, 3f)
			};
			Kenedia.Modules.Core.Controls.FlowPanel pp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleLeftToRight
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = pp,
				Width = 165,
				Location = new Point(35, 0),
				Height = 20,
				SetLocalizedText = () => strings_common.TopOffset
			};
			_topOffsetBox = new NumberBox
			{
				Parent = pp,
				MinValue = -50,
				MaxValue = 50,
				Value = SharedSettings.WindowOffset.Top,
				SetLocalizedTooltip = () => strings_common.TopOffset,
				ValueChangedAction = delegate
				{
					UpdateOffset();
				}
			};
			pp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleLeftToRight
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = pp,
				Width = 165,
				Location = new Point(35, 0),
				Height = 20,
				SetLocalizedText = () => strings_common.LeftOffset
			};
			_leftOffsetBox = new NumberBox
			{
				Parent = pp,
				MinValue = -50,
				MaxValue = 50,
				Value = SharedSettings.WindowOffset.Left,
				SetLocalizedTooltip = () => strings_common.LeftOffset,
				ValueChangedAction = delegate
				{
					UpdateOffset();
				}
			};
			pp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleLeftToRight
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = pp,
				Width = 165,
				Location = new Point(35, 0),
				Height = 20,
				SetLocalizedText = () => strings_common.BottomOffset
			};
			_bottomOffsetBox = new NumberBox
			{
				Parent = pp,
				MinValue = -50,
				MaxValue = 50,
				Value = SharedSettings.WindowOffset.Bottom,
				SetLocalizedTooltip = () => strings_common.BottomOffset,
				ValueChangedAction = delegate
				{
					UpdateOffset();
				}
			};
			pp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleLeftToRight
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = pp,
				Width = 165,
				Location = new Point(35, 0),
				Height = 20,
				SetLocalizedText = () => strings_common.RightOffset
			};
			_rightOffsetBox = new NumberBox
			{
				Parent = pp,
				MinValue = -50,
				MaxValue = 50,
				Value = SharedSettings.WindowOffset.Right,
				SetLocalizedTooltip = () => strings_common.RightOffset,
				ValueChangedAction = delegate
				{
					UpdateOffset();
				}
			};
			Kenedia.Modules.Core.Controls.FlowPanel subCP = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = cFP,
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				ControlPadding = new Vector2(5f, 5f)
			};
			cP = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = subCP,
				HeightSizingMode = SizingMode.AutoSize,
				Width = 125,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(5f, 5f)
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = cP,
				SetLocalizedText = () => strings_common.TopLeftCorner,
				AutoSizeWidth = true,
				Visible = false
			};
			_topLeftImage = new Kenedia.Modules.Core.Controls.Image
			{
				Parent = cP,
				BackgroundColor = Color.get_White(),
				Size = new Point(100, _rightOffsetBox.Height * 2),
				SetLocalizedTooltip = () => strings_common.TopLeftCorner
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = cP,
				SetLocalizedText = () => strings_common.BottomLeftCorner,
				AutoSizeWidth = true,
				Visible = false
			};
			_bottomLeftImage = new Kenedia.Modules.Core.Controls.Image
			{
				Parent = cP,
				BackgroundColor = Color.get_White(),
				Size = new Point(100, _rightOffsetBox.Height * 2),
				SetLocalizedTooltip = () => strings_common.BottomLeftCorner
			};
			cP = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = subCP,
				HeightSizingMode = SizingMode.AutoSize,
				Width = 125,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(5f, 5f)
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = cP,
				SetLocalizedText = () => strings_common.TopRightCorner,
				AutoSizeWidth = true,
				Visible = false
			};
			_topRightImage = new Kenedia.Modules.Core.Controls.Image
			{
				Parent = cP,
				BackgroundColor = Color.get_White(),
				Size = new Point(100, _rightOffsetBox.Height * 2),
				SetLocalizedTooltip = () => strings_common.TopRightCorner
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = cP,
				SetLocalizedText = () => strings_common.BottomRightCorner,
				AutoSizeWidth = true,
				Visible = false
			};
			_bottomRightImage = new Kenedia.Modules.Core.Controls.Image
			{
				Parent = cP,
				BackgroundColor = Color.get_White(),
				Size = new Point(100, _rightOffsetBox.Height * 2),
				SetLocalizedTooltip = () => strings_common.BottomRightCorner
			};
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
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
			Point p = (Point)(((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Point(SharedSettings.WindowOffset.Left, SharedSettings.WindowOffset.Top) : Point.get_Zero());
			using Bitmap bitmap = new Bitmap(_topLeftImage.Width, _topLeftImage.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new Point(wndBounds.Left + p.X, wndBounds.Top + p.Y), Point.Empty, new Size(_topLeftImage.Width, _topLeftImage.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			_topLeftImage.Texture = s.CreateTexture2D();
		}

		private void SetBottomLeftImage()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
			Point p = (Point)(((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Point(SharedSettings.WindowOffset.Left, SharedSettings.WindowOffset.Bottom) : Point.get_Zero());
			using Bitmap bitmap = new Bitmap(_bottomLeftImage.Width, _bottomLeftImage.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new Point(wndBounds.Left + p.X, wndBounds.Bottom - _bottomLeftImage.Height + p.Y), Point.Empty, new Size(_bottomLeftImage.Width, _bottomLeftImage.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			_bottomLeftImage.Texture = s.CreateTexture2D();
		}

		private void SetTopRightImage()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
			Point p = (Point)(((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Point(SharedSettings.WindowOffset.Right, SharedSettings.WindowOffset.Top) : Point.get_Zero());
			using Bitmap bitmap = new Bitmap(_topRightImage.Width, _topRightImage.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new Point(wndBounds.Right - _topRightImage.Width + p.X, wndBounds.Top + p.Y), Point.Empty, new Size(_topRightImage.Width, _topRightImage.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			_topRightImage.Texture = s.CreateTexture2D();
		}

		private void SetBottomRightImage()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
			Point p = (Point)(((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Point(SharedSettings.WindowOffset.Right, SharedSettings.WindowOffset.Bottom) : Point.get_Zero());
			using Bitmap bitmap = new Bitmap(_bottomLeftImage.Width, _bottomLeftImage.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new Point(wndBounds.Right - _bottomRightImage.Width + p.X, wndBounds.Bottom - _bottomRightImage.Height + p.Y), Point.Empty, new Size(_bottomRightImage.Width, _bottomRightImage.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			_bottomRightImage.Texture = s.CreateTexture2D();
		}
	}
}
