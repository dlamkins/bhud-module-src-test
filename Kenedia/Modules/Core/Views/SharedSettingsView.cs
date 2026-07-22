using System.ComponentModel;
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
			SharedSettings.PropertyChanged += new PropertyChangedEventHandler(SharedSettings_PropertyChanged);
		}

		private void SharedSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "WindowOffset")
			{
				ApplyOffsets();
			}
		}

		public override void CreateLayout(Blish_HUD.Controls.Container p, int? width = null)
		{
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
				Location = new Microsoft.Xna.Framework.Point(35, 0),
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
					SetWindowOffset();
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
				Location = new Microsoft.Xna.Framework.Point(35, 0),
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
					SetWindowOffset();
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
				Location = new Microsoft.Xna.Framework.Point(35, 0),
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
					SetWindowOffset();
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
				Location = new Microsoft.Xna.Framework.Point(35, 0),
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
					SetWindowOffset();
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
				BackgroundColor = Microsoft.Xna.Framework.Color.White,
				Size = new Microsoft.Xna.Framework.Point(100, _rightOffsetBox.Height * 2),
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
				BackgroundColor = Microsoft.Xna.Framework.Color.White,
				Size = new Microsoft.Xna.Framework.Point(100, _rightOffsetBox.Height * 2),
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
				BackgroundColor = Microsoft.Xna.Framework.Color.White,
				Size = new Microsoft.Xna.Framework.Point(100, _rightOffsetBox.Height * 2),
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
				BackgroundColor = Microsoft.Xna.Framework.Color.White,
				Size = new Microsoft.Xna.Framework.Point(100, _rightOffsetBox.Height * 2),
				SetLocalizedTooltip = () => strings_common.BottomRightCorner
			};
		}

		private void ApplyOffsets()
		{
			if (_leftOffsetBox != null)
			{
				_leftOffsetBox.Value = SharedSettings.WindowOffset.Left;
				_topOffsetBox.Value = SharedSettings.WindowOffset.Top;
				_rightOffsetBox.Value = SharedSettings.WindowOffset.Right;
				_bottomOffsetBox.Value = SharedSettings.WindowOffset.Bottom;
				SetWindowOffsetImages();
			}
		}

		public void SetWindowOffset()
		{
			if (_leftOffsetBox != null)
			{
				SharedSettings.WindowOffset = new RectangleDimensions(_leftOffsetBox.Value, _topOffsetBox.Value, _rightOffsetBox.Value, _bottomOffsetBox.Value);
				SetWindowOffsetImages();
			}
		}

		public void SetWindowOffsetImages()
		{
			if (_leftOffsetBox != null)
			{
				SetTopLeftImage();
				SetTopRightImage();
				SetBottomLeftImage();
				SetBottomRightImage();
			}
		}

		private void SetTopLeftImage()
		{
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
			Microsoft.Xna.Framework.Point p = (((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Microsoft.Xna.Framework.Point(SharedSettings.WindowOffset.Left, SharedSettings.WindowOffset.Top) : Microsoft.Xna.Framework.Point.Zero);
			using Bitmap bitmap = new Bitmap(_topLeftImage.Width, _topLeftImage.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new System.Drawing.Point(wndBounds.Left + p.X, wndBounds.Top + p.Y), System.Drawing.Point.Empty, new Size(_topLeftImage.Width, _topLeftImage.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			_topLeftImage.Texture = s.CreateTexture2D();
		}

		private void SetBottomLeftImage()
		{
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
			Microsoft.Xna.Framework.Point p = (((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Microsoft.Xna.Framework.Point(SharedSettings.WindowOffset.Left, SharedSettings.WindowOffset.Bottom) : Microsoft.Xna.Framework.Point.Zero);
			using Bitmap bitmap = new Bitmap(_bottomLeftImage.Width, _bottomLeftImage.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new System.Drawing.Point(wndBounds.Left + p.X, wndBounds.Bottom - _bottomLeftImage.Height + p.Y), System.Drawing.Point.Empty, new Size(_bottomLeftImage.Width, _bottomLeftImage.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			_bottomLeftImage.Texture = s.CreateTexture2D();
		}

		private void SetTopRightImage()
		{
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
			Microsoft.Xna.Framework.Point p = (((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Microsoft.Xna.Framework.Point(SharedSettings.WindowOffset.Right, SharedSettings.WindowOffset.Top) : Microsoft.Xna.Framework.Point.Zero);
			using Bitmap bitmap = new Bitmap(_topRightImage.Width, _topRightImage.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new System.Drawing.Point(wndBounds.Right - _topRightImage.Width + p.X, wndBounds.Top + p.Y), System.Drawing.Point.Empty, new Size(_topRightImage.Width, _topRightImage.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			_topRightImage.Texture = s.CreateTexture2D();
		}

		private void SetBottomRightImage()
		{
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
			Microsoft.Xna.Framework.Point p = (((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Microsoft.Xna.Framework.Point(SharedSettings.WindowOffset.Right, SharedSettings.WindowOffset.Bottom) : Microsoft.Xna.Framework.Point.Zero);
			using Bitmap bitmap = new Bitmap(_bottomLeftImage.Width, _bottomLeftImage.Height);
			using Graphics g = Graphics.FromImage(bitmap);
			using MemoryStream s = new MemoryStream();
			g.CopyFromScreen(new System.Drawing.Point(wndBounds.Right - _bottomRightImage.Width + p.X, wndBounds.Bottom - _bottomRightImage.Height + p.Y), System.Drawing.Point.Empty, new Size(_bottomRightImage.Width, _bottomRightImage.Height));
			bitmap.Save(s, ImageFormat.Bmp);
			_bottomRightImage.Texture = s.CreateTexture2D();
		}
	}
}
