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
using Blish_HUD.Input;
using Gw2Sharp.Mumble.Models;
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

		private Microsoft.Xna.Framework.Point _draggingStart;

		private int _characterPotraitSize = 130;

		private int _gap = 13;

		public Action OnImageCaptured { get; set; }

		public Func<string> AccountImagePath { get; set; }

		public Func<string> AccountName { get; set; }

		public PotraitCapture(ClientWindowService clientWindowService, SharedSettings sharedSettings, TextureManager tM)
		{
			_clientWindowService = clientWindowService;
			_sharedSettings = sharedSettings;
			Microsoft.Xna.Framework.Point res = GameService.Graphics.Resolution;
			base.Size = new Microsoft.Xna.Framework.Point(100, 100);
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
			base.Location = new Microsoft.Xna.Framework.Point((res.X - base.Size.X) / 2, res.Y - 125 - base.Size.Y);
			_dragButton = new ImageButton
			{
				Parent = this,
				Texture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Drag_Button),
				HoveredTexture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Drag_Button_Hovered),
				Size = new Microsoft.Xna.Framework.Point(32, 32),
				Location = new Microsoft.Xna.Framework.Point(0, 0),
				SetLocalizedTooltip = () => strings.DragOverCharacter_Instructions
			};
			_dragButton.LeftMouseButtonPressed += DragButton_LeftMouseButtonPressed;
			_dragButton.LeftMouseButtonReleased += DragButton_LeftMouseButtonReleased;
			_captureButton = new ImageButton
			{
				Parent = this,
				Texture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Potrait_Button),
				HoveredTexture = (AsyncTexture2D)tM.GetControlTexture(TextureManager.ControlTextures.Potrait_Button_Hovered),
				Size = new Microsoft.Xna.Framework.Point(32, 32),
				Location = new Microsoft.Xna.Framework.Point(_dragButton.Right + 5, 0),
				SetLocalizedTooltip = () => strings.CapturePotraits,
				ClickAction = delegate
				{
					CapturePotraits();
				}
			};
			_disclaimerBackground = new FramedContainer
			{
				Parent = this,
				Location = new Microsoft.Xna.Framework.Point(_captureButton.Right + 5, 0),
				BorderColor = Microsoft.Xna.Framework.Color.Black,
				BackgroundImage = AsyncTexture2D.FromAssetId(156003),
				TextureRectangle = new Microsoft.Xna.Framework.Rectangle(50, 50, 500, 500),
				WidthSizingMode = SizingMode.AutoSize,
				AutoSizePadding = new Microsoft.Xna.Framework.Point(15, 0),
				Height = 32
			};
			_sizeBox = new NumberBox
			{
				Parent = _disclaimerBackground,
				Location = new Microsoft.Xna.Framework.Point(5, (_disclaimerBackground.Height - 25) / 2),
				Size = new Microsoft.Xna.Framework.Point(100, 25),
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
				Location = new Microsoft.Xna.Framework.Point(_sizeBox.Right + 5, (_disclaimerBackground.Height - 25) / 2),
				Size = new Microsoft.Xna.Framework.Point(100, 25),
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
				Location = new Microsoft.Xna.Framework.Point(_gapBox.Right + 5, 0),
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
				Size = new Microsoft.Xna.Framework.Point(32, 32),
				Location = new Microsoft.Xna.Framework.Point(0, 35),
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
				Size = new Microsoft.Xna.Framework.Point(32, 32),
				Location = new Microsoft.Xna.Framework.Point(0, 70),
				SetLocalizedTooltip = () => string.Format(strings.RemoveItem, strings.PotraitFrame),
				ClickAction = delegate
				{
					RemovePortrait();
				}
			};
			_characterPotraitsBackground = new Dummy
			{
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.8f,
				Parent = Control.Graphics.SpriteScreen,
				ZIndex = 2147483646
			};
			AddPotrait();
			AddPotrait();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			_dragging = _dragging && base.MouseOver;
			if (_dragging)
			{
				base.Location = Control.Input.Mouse.Position.Add(new Microsoft.Xna.Framework.Point(-_draggingStart.X, -_draggingStart.Y));
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
			int index = 0;
			Microsoft.Xna.Framework.Point pos = new Microsoft.Xna.Framework.Point(_captureButton.AbsoluteBounds.X + 5, _captureButton.AbsoluteBounds.Y + 40);
			_characterPotraitsBackground.Location = pos.Add(new Microsoft.Xna.Framework.Point(-5, -5));
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
			_dragging = true;
			_draggingStart = (_dragging ? base.RelativeMousePosition : Microsoft.Xna.Framework.Point.Zero);
		}

		private void CapturePotraits()
		{
			string accountName = AccountName?.Invoke();
			if (accountName == null || string.IsNullOrEmpty(accountName))
			{
				ScreenNotification.ShowNotification("[Characters]: Unable to determine account name.");
				return;
			}
			string path2 = AccountImagePath?.Invoke();
			if (string.IsNullOrEmpty(path2))
			{
				return;
			}
			try
			{
				Directory.CreateDirectory(path2);
			}
			catch (Exception)
			{
				ScreenNotification.ShowNotification("[Characters]: Unable to access the portrait image folder.");
				return;
			}
			Regex regex = new Regex("Image.*[0-9].png");
			List<string> images = (from path in Directory.GetFiles(path2, "*.png", SearchOption.AllDirectories)
				where regex.IsMatch(path)
				select path).ToList();
			IntPtr hWnd = GameService.GameIntegration.Gw2Instance.Gw2WindowHandle;
			User32Dll.POINT pOINT = default(User32Dll.POINT);
			pOINT.X = 0;
			pOINT.Y = 0;
			User32Dll.POINT clientOrigin = pOINT;
			if (hWnd != IntPtr.Zero)
			{
				User32Dll.ClientToScreen(hWnd, ref clientOrigin);
			}
			else
				_ = 0;
			User32Dll.GetWindowRect(hWnd, out var _);
			User32Dll.GetClientRect(hWnd, out var _);
			User32Dll.GetDpiForWindow(hWnd);
			double uiScale = GameService.Graphics.UIScaleMultiplier;
			for (int i = 0; i < _characterPotraitFrames.Count; i++)
			{
				FramedMaskedRegion c = _characterPotraitFrames[i];
				Microsoft.Xna.Framework.Rectangle bounds = new Microsoft.Xna.Framework.Rectangle(c.AbsoluteBounds.X + c.BorderWidth.Horizontal / 2, c.AbsoluteBounds.Y + c.BorderWidth.Vertical / 2, c.AbsoluteBounds.Width - c.BorderWidth.Horizontal, c.AbsoluteBounds.Height - c.BorderWidth.Vertical);
				int x = bounds.X;
				int y = bounds.Y;
				int width = Math.Max(1, bounds.Width);
				int height = Math.Max(1, bounds.Height);
				int scaledX = (int)Math.Round((double)x * uiScale);
				int scaledY = (int)Math.Round((double)y * uiScale);
				int scaledWidth = Math.Max(1, (int)Math.Round((double)width * uiScale));
				int scaledHeight = Math.Max(1, (int)Math.Round((double)height * uiScale));
				int captureX = clientOrigin.X + scaledX;
				int captureY = clientOrigin.Y + scaledY;
				_ = _sharedSettings.WindowOffset.Left;
				_ = _sharedSettings.WindowOffset.Top;
				using Bitmap bitmap = new Bitmap(scaledWidth, scaledHeight);
				using (Graphics g = System.Drawing.Graphics.FromImage(bitmap))
				{
					g.CopyFromScreen(new System.Drawing.Point(captureX, captureY), System.Drawing.Point.Empty, new Size(scaledWidth, scaledHeight));
				}
				bitmap.Save(GetImagePath(images), ImageFormat.Png);
			}
			OnImageCaptured?.Invoke();
			ScreenNotification.ShowNotification(string.Format("[Characters]: " + strings.CapturedXPotraits, _characterPotraitFrames.Count));
			string GetImagePath(List<string> imagePaths)
			{
				for (int j = 1; j < int.MaxValue; j++)
				{
					string imagePath = path2 + "Image " + $"{j:00}" + ".png";
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
			_sizeBox.Value = (_characterPotraitSize = GetPortraitDefaultSize());
			_gapBox.Value = (_gap = GetPortraitDefaultGap());
			ForceOnScreen();
		}

		private double GetScaling()
		{
			return GameService.Gw2Mumble.UI.UISize switch
			{
				UiSize.Small => 0.81f, 
				UiSize.Normal => 0.897f, 
				UiSize.Large => 1f, 
				UiSize.Larger => 1.103f, 
				_ => 1f, 
			};
		}

		private int GetPortraitDefaultSize()
		{
			return (int)(132.0 * GetScaling());
		}

		private int GetPortraitDefaultGap()
		{
			return (int)(12.0 * GetScaling());
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
