using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.GameIntegration.GfxSettings;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls
{
	public class PotraitCapture : Container
	{
		private readonly List<FramedMaskedRegion> _characterPotraitFrames = new List<FramedMaskedRegion>();

		private readonly ClientWindowService _clientWindowService;

		private readonly SharedSettings _sharedSettings;

		private readonly ImageButton _captureButton;

		private readonly ImageButton _addButton;

		private readonly ImageButton _removeButton;

		private readonly Dummy _characterPotraitsBackground;

		private readonly Kenedia.Modules.Core.Controls.Label _disclaimer;

		private readonly FramedContainer _disclaimerBackground;

		private readonly ImageButton _dragButton;

		private readonly NumberBox _sizeBox;

		private readonly NumberBox _gapBox;

		private bool _dragging;

		private Point _draggingStart;

		private int _characterPotraitSize = 130;

		private int _gap = 13;

		public Action OnImageCaptured { get; set; }

		public Func<string> AccountImagePath { get; set; }

		public PotraitCapture(ClientWindowService clientWindowService, SharedSettings sharedSettings, TextureManager tM)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_0432: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			_clientWindowService = clientWindowService;
			_sharedSettings = sharedSettings;
			Point res = GameService.Graphics.Resolution;
			base.Size = new Point(100, 100);
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
			base.Location = new Point((res.X - base.Size.X) / 2, res.Y - 125 - base.Size.Y);
			_dragButton = new ImageButton
			{
				Parent = this,
				Texture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Drag_Button),
				HoveredTexture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Drag_Button_Hovered),
				Size = new Point(32, 32),
				Location = new Point(0, 0),
				SetLocalizedTooltip = () => strings.DragOverCharacter_Instructions
			};
			_dragButton.LeftMouseButtonPressed += DragButton_LeftMouseButtonPressed;
			_dragButton.LeftMouseButtonReleased += DragButton_LeftMouseButtonReleased;
			_captureButton = new ImageButton
			{
				Parent = this,
				Texture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Potrait_Button),
				HoveredTexture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Potrait_Button_Hovered),
				Size = new Point(32, 32),
				Location = new Point(_dragButton.Right + 5, 0),
				SetLocalizedTooltip = () => strings.CapturePotraits,
				ClickAction = delegate
				{
					CapturePotraits();
				}
			};
			_disclaimerBackground = new FramedContainer
			{
				Parent = this,
				Location = new Point(_captureButton.Right + 5, 0),
				BorderColor = Color.get_Black(),
				BackgroundImage = AsyncTexture2D.FromAssetId(156003),
				TextureRectangle = new Rectangle(50, 50, 500, 500),
				WidthSizingMode = SizingMode.AutoSize,
				AutoSizePadding = new Point(15, 0),
				Height = 32
			};
			_sizeBox = new NumberBox
			{
				Parent = _disclaimerBackground,
				Location = new Point(5, (_disclaimerBackground.Height - 25) / 2),
				Size = new Point(100, 25),
				Value = _characterPotraitSize,
				SetLocalizedTooltip = () => strings.PotraitSize,
				ValueChangedAction = delegate(int num)
				{
					_characterPotraitSize = num;
					RepositionPotraitFrames();
				}
			};
			_gapBox = new NumberBox
			{
				Parent = _disclaimerBackground,
				Location = new Point(_sizeBox.Right + 5, (_disclaimerBackground.Height - 25) / 2),
				Size = new Point(100, 25),
				Value = _gap,
				SetLocalizedTooltip = () => strings.PotraitGap,
				ValueChangedAction = delegate(int value)
				{
					_gap = value;
					RepositionPotraitFrames();
				}
			};
			_disclaimer = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = _disclaimerBackground,
				Location = new Point(_gapBox.Right + 5, 0),
				TextColor = ContentService.Colors.ColonialWhite,
				AutoSizeWidth = true,
				Height = 32,
				Font = GameService.Content.DefaultFont16,
				SetLocalizedText = () => strings.BestResultLargerDisclaimer,
				Padding = new Thickness(0f, 0f)
			};
			_addButton = new ImageButton
			{
				Parent = this,
				Texture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Plus_Button),
				HoveredTexture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Plus_Button_Hovered),
				Size = new Point(32, 32),
				Location = new Point(0, 35),
				SetLocalizedTooltip = () => string.Format(strings.AddItem, strings.PotraitFrame),
				ClickAction = delegate
				{
					AddPotrait();
				}
			};
			_removeButton = new ImageButton
			{
				Parent = this,
				Texture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Minus_Button),
				HoveredTexture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Minus_Button_Hovered),
				Size = new Point(32, 32),
				Location = new Point(0, 70),
				SetLocalizedTooltip = () => string.Format(strings.RemoveItem, strings.PotraitFrame),
				ClickAction = delegate
				{
					RemovePortrait();
				}
			};
			_characterPotraitsBackground = new Dummy
			{
				BackgroundColor = Color.get_Black() * 0.8f,
				Parent = Control.Graphics.SpriteScreen,
				ZIndex = 2147483646
			};
			AddPotrait();
			AddPotrait();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateContainer(gameTime);
			_dragging = _dragging && base.MouseOver;
			if (_dragging)
			{
				base.Location = Control.Input.Mouse.Position.Add(new Point(-_draggingStart.X, -_draggingStart.Y));
			}
			ForceOnScreen();
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_captureButton?.Dispose();
			_addButton?.Dispose();
			_removeButton?.Dispose();
			_disclaimer?.Dispose();
			_disclaimerBackground?.Dispose();
			_dragButton?.Dispose();
			_sizeBox?.Dispose();
			_gapBox?.Dispose();
			_characterPotraitsBackground?.Dispose();
			foreach (FramedMaskedRegion characterPotraitFrame in _characterPotraitFrames)
			{
				characterPotraitFrame.Dispose();
			}
		}

		private void RemovePortrait()
		{
			if (_characterPotraitFrames.Count > 1)
			{
				FramedMaskedRegion frame = _characterPotraitFrames.Last();
				frame.Dispose();
				_characterPotraitFrames.Remove(frame);
				RepositionPotraitFrames();
			}
		}

		private void AddPotrait()
		{
			_characterPotraitFrames.Add(new FramedMaskedRegion
			{
				Parent = Control.Graphics.SpriteScreen,
				ZIndex = int.MaxValue,
				Visible = base.Visible
			});
			RepositionPotraitFrames();
		}

		private void RepositionPotraitFrames()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			int index = 0;
			Point pos = default(Point);
			((Point)(ref pos))._002Ector(_captureButton.AbsoluteBounds.X + 5, _captureButton.AbsoluteBounds.Y + 40);
			_characterPotraitsBackground.Location = pos.Add(new Point(-5, -5));
			foreach (FramedMaskedRegion characterPotraitFrame in _characterPotraitFrames)
			{
				characterPotraitFrame.Width = _characterPotraitSize;
				characterPotraitFrame.Height = _characterPotraitSize;
				characterPotraitFrame.Location = pos;
				pos.X += _characterPotraitSize + _gap;
				index++;
			}
			_characterPotraitsBackground.Width = pos.X - _characterPotraitsBackground.Location.X - _gap + 5;
			_characterPotraitsBackground.Height = _characterPotraitSize + 10;
		}

		private void DragButton_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			_dragging = false;
		}

		private void DragButton_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			_dragging = true;
			_draggingStart = (_dragging ? base.RelativeMousePosition : Point.get_Zero());
		}

		private void CapturePotraits()
		{
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			string path2 = AccountImagePath?.Invoke();
			if (string.IsNullOrEmpty(path2))
			{
				return;
			}
			Regex regex = new Regex("Image.*[0-9].png");
			List<string> images = (from path in Directory.GetFiles(path2, "*.png", SearchOption.AllDirectories)
				where regex.IsMatch(path)
				select path).ToList();
			User32Dll.RECT wndBounds = _clientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
			Point p = (Point)(((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Point(_sharedSettings.WindowOffset.Left, _sharedSettings.WindowOffset.Top) : Point.get_Zero());
			double factor = GameService.Graphics.UIScaleMultiplier;
			Size size = new Size(_characterPotraitSize, _characterPotraitSize);
			foreach (FramedMaskedRegion characterPotraitFrame in _characterPotraitFrames)
			{
				Rectangle bounds = characterPotraitFrame.MaskedRegion;
				using Bitmap bitmap = new Bitmap((int)((double)bounds.Width * factor), (int)((double)bounds.Height * factor));
				using (Graphics g = System.Drawing.Graphics.FromImage(bitmap))
				{
					int x = (int)((double)bounds.X * factor);
					int y = (int)((double)bounds.Y * factor);
					g.CopyFromScreen(new Point(wndBounds.Left + p.X + x, wndBounds.Top + p.Y + y), Point.Empty, size);
				}
				bitmap.Save(GetImagePath(images), ImageFormat.Png);
			}
			OnImageCaptured?.Invoke();
			ScreenNotification.ShowNotification(string.Format("[Characters]: " + strings.CapturedXPotraits, _characterPotraitFrames.Count));
			string GetImagePath(List<string> imagePaths)
			{
				for (int i = 1; i < int.MaxValue; i++)
				{
					string imagePath = path2 + "Image " + $"{i:00}" + ".png";
					if (!imagePaths.Contains(imagePath))
					{
						imagePaths.Add(imagePath);
						return imagePath;
					}
				}
				return path2 + "Last Image.png";
			}
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			base.OnMoved(e);
			if (_characterPotraitFrames.Count > 0)
			{
				RepositionPotraitFrames();
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			_characterPotraitsBackground.Show();
			foreach (FramedMaskedRegion characterPotraitFrame in _characterPotraitFrames)
			{
				characterPotraitFrame.Show();
			}
			ForceOnScreen();
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
			_characterPotraitsBackground.Hide();
			foreach (FramedMaskedRegion characterPotraitFrame in _characterPotraitFrames)
			{
				characterPotraitFrame.Hide();
			}
		}

		private void ForceOnScreen()
		{
			Screen screen = Control.Graphics.SpriteScreen;
			if (base.Bottom > screen.Bottom)
			{
				base.Bottom = screen.Bottom;
			}
			if (base.Top < screen.Top + base.Height)
			{
				base.Top = screen.Top + base.Height;
			}
			if (base.Left < screen.Left)
			{
				base.Left = screen.Left;
			}
			if (base.Right > screen.Right)
			{
				base.Left = screen.Right - base.Width;
			}
		}
	}
}
