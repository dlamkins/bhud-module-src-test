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
using Microsoft.Xna.Framework.Graphics;

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

		private readonly Label _disclaimer;

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
			: this()
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
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0432: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			_clientWindowService = clientWindowService;
			_sharedSettings = sharedSettings;
			Point res = GameService.Graphics.get_Resolution();
			((Control)this).set_Size(new Point(100, 100));
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Control)this).set_Location(new Point((res.X - ((Control)this).get_Size().X) / 2, res.Y - 125 - ((Control)this).get_Size().Y));
			ImageButton imageButton = new ImageButton();
			((Control)imageButton).set_Parent((Container)(object)this);
			imageButton.Texture = AsyncTexture2D.op_Implicit(tM.GetControlTexture(TextureManager.ControlTextures.Drag_Button));
			imageButton.HoveredTexture = AsyncTexture2D.op_Implicit(tM.GetControlTexture(TextureManager.ControlTextures.Drag_Button_Hovered));
			((Control)imageButton).set_Size(new Point(32, 32));
			((Control)imageButton).set_Location(new Point(0, 0));
			imageButton.SetLocalizedTooltip = () => strings.DragOverCharacter_Instructions;
			_dragButton = imageButton;
			((Control)_dragButton).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)DragButton_LeftMouseButtonPressed);
			((Control)_dragButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)DragButton_LeftMouseButtonReleased);
			ImageButton imageButton2 = new ImageButton();
			((Control)imageButton2).set_Parent((Container)(object)this);
			imageButton2.Texture = AsyncTexture2D.op_Implicit(tM.GetControlTexture(TextureManager.ControlTextures.Potrait_Button));
			imageButton2.HoveredTexture = AsyncTexture2D.op_Implicit(tM.GetControlTexture(TextureManager.ControlTextures.Potrait_Button_Hovered));
			((Control)imageButton2).set_Size(new Point(32, 32));
			((Control)imageButton2).set_Location(new Point(((Control)_dragButton).get_Right() + 5, 0));
			imageButton2.SetLocalizedTooltip = () => strings.CapturePotraits;
			imageButton2.ClickAction = delegate
			{
				CapturePotraits();
			};
			_captureButton = imageButton2;
			FramedContainer framedContainer = new FramedContainer();
			((Control)framedContainer).set_Parent((Container)(object)this);
			((Control)framedContainer).set_Location(new Point(((Control)_captureButton).get_Right() + 5, 0));
			framedContainer.BorderColor = Color.get_Black();
			framedContainer.BackgroundImage = AsyncTexture2D.FromAssetId(156003);
			framedContainer.TextureRectangle = new Rectangle(50, 50, 500, 500);
			((Container)framedContainer).set_WidthSizingMode((SizingMode)1);
			((Container)framedContainer).set_AutoSizePadding(new Point(15, 0));
			((Control)framedContainer).set_Height(32);
			_disclaimerBackground = framedContainer;
			NumberBox numberBox = new NumberBox();
			((Control)numberBox).set_Parent((Container)(object)_disclaimerBackground);
			((Control)numberBox).set_Location(new Point(5, (((Control)_disclaimerBackground).get_Height() - 25) / 2));
			((Control)numberBox).set_Size(new Point(100, 25));
			numberBox.Value = _characterPotraitSize;
			numberBox.SetLocalizedTooltip = () => strings.PotraitSize;
			numberBox.ValueChangedAction = delegate(int num)
			{
				_characterPotraitSize = num;
				RepositionPotraitFrames();
			};
			_sizeBox = numberBox;
			NumberBox numberBox2 = new NumberBox();
			((Control)numberBox2).set_Parent((Container)(object)_disclaimerBackground);
			((Control)numberBox2).set_Location(new Point(((Control)_sizeBox).get_Right() + 5, (((Control)_disclaimerBackground).get_Height() - 25) / 2));
			((Control)numberBox2).set_Size(new Point(100, 25));
			numberBox2.Value = _gap;
			numberBox2.SetLocalizedTooltip = () => strings.PotraitGap;
			numberBox2.ValueChangedAction = delegate(int value)
			{
				_gap = value;
				RepositionPotraitFrames();
			};
			_gapBox = numberBox2;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)_disclaimerBackground);
			((Control)label).set_Location(new Point(((Control)_gapBox).get_Right() + 5, 0));
			((Label)label).set_TextColor(Colors.ColonialWhite);
			((Label)label).set_AutoSizeWidth(true);
			((Control)label).set_Height(32);
			((Label)label).set_Font(GameService.Content.get_DefaultFont16());
			label.SetLocalizedText = () => strings.BestResultLargerDisclaimer;
			((Control)label).set_Padding(new Thickness(0f, 0f));
			_disclaimer = label;
			ImageButton imageButton3 = new ImageButton();
			((Control)imageButton3).set_Parent((Container)(object)this);
			imageButton3.Texture = AsyncTexture2D.op_Implicit(tM.GetControlTexture(TextureManager.ControlTextures.Plus_Button));
			imageButton3.HoveredTexture = AsyncTexture2D.op_Implicit(tM.GetControlTexture(TextureManager.ControlTextures.Plus_Button_Hovered));
			((Control)imageButton3).set_Size(new Point(32, 32));
			((Control)imageButton3).set_Location(new Point(0, 35));
			imageButton3.SetLocalizedTooltip = () => string.Format(strings.AddItem, strings.PotraitFrame);
			imageButton3.ClickAction = delegate
			{
				AddPotrait();
			};
			_addButton = imageButton3;
			ImageButton imageButton4 = new ImageButton();
			((Control)imageButton4).set_Parent((Container)(object)this);
			imageButton4.Texture = AsyncTexture2D.op_Implicit(tM.GetControlTexture(TextureManager.ControlTextures.Minus_Button));
			imageButton4.HoveredTexture = AsyncTexture2D.op_Implicit(tM.GetControlTexture(TextureManager.ControlTextures.Minus_Button_Hovered));
			((Control)imageButton4).set_Size(new Point(32, 32));
			((Control)imageButton4).set_Location(new Point(0, 70));
			imageButton4.SetLocalizedTooltip = () => string.Format(strings.RemoveItem, strings.PotraitFrame);
			imageButton4.ClickAction = delegate
			{
				RemovePortrait();
			};
			_removeButton = imageButton4;
			Dummy dummy = new Dummy();
			((Control)dummy).set_BackgroundColor(Color.get_Black() * 0.8f);
			((Control)dummy).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((Control)dummy).set_ZIndex(2147483646);
			_characterPotraitsBackground = dummy;
			AddPotrait();
			AddPotrait();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			_dragging = _dragging && ((Control)this).get_MouseOver();
			if (_dragging)
			{
				((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
					.Add(new Point(-_draggingStart.X, -_draggingStart.Y)));
			}
			ForceOnScreen();
		}

		protected override void DisposeControl()
		{
			((Container)this).DisposeControl();
			ImageButton captureButton = _captureButton;
			if (captureButton != null)
			{
				((Control)captureButton).Dispose();
			}
			ImageButton addButton = _addButton;
			if (addButton != null)
			{
				((Control)addButton).Dispose();
			}
			ImageButton removeButton = _removeButton;
			if (removeButton != null)
			{
				((Control)removeButton).Dispose();
			}
			Label disclaimer = _disclaimer;
			if (disclaimer != null)
			{
				((Control)disclaimer).Dispose();
			}
			FramedContainer disclaimerBackground = _disclaimerBackground;
			if (disclaimerBackground != null)
			{
				((Control)disclaimerBackground).Dispose();
			}
			ImageButton dragButton = _dragButton;
			if (dragButton != null)
			{
				((Control)dragButton).Dispose();
			}
			NumberBox sizeBox = _sizeBox;
			if (sizeBox != null)
			{
				((Control)sizeBox).Dispose();
			}
			NumberBox gapBox = _gapBox;
			if (gapBox != null)
			{
				((Control)gapBox).Dispose();
			}
			Dummy characterPotraitsBackground = _characterPotraitsBackground;
			if (characterPotraitsBackground != null)
			{
				((Control)characterPotraitsBackground).Dispose();
			}
			foreach (FramedMaskedRegion characterPotraitFrame in _characterPotraitFrames)
			{
				((Control)characterPotraitFrame).Dispose();
			}
		}

		private void RemovePortrait()
		{
			if (_characterPotraitFrames.Count > 1)
			{
				FramedMaskedRegion frame = _characterPotraitFrames.Last();
				((Control)frame).Dispose();
				_characterPotraitFrames.Remove(frame);
				RepositionPotraitFrames();
			}
		}

		private void AddPotrait()
		{
			List<FramedMaskedRegion> characterPotraitFrames = _characterPotraitFrames;
			FramedMaskedRegion framedMaskedRegion = new FramedMaskedRegion();
			((Control)framedMaskedRegion).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((Control)framedMaskedRegion).set_ZIndex(int.MaxValue);
			((Control)framedMaskedRegion).set_Visible(((Control)this).get_Visible());
			characterPotraitFrames.Add(framedMaskedRegion);
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
			((Point)(ref pos))._002Ector(((Control)_captureButton).get_AbsoluteBounds().X + 5, ((Control)_captureButton).get_AbsoluteBounds().Y + 40);
			((Control)_characterPotraitsBackground).set_Location(pos.Add(new Point(-5, -5)));
			foreach (FramedMaskedRegion characterPotraitFrame in _characterPotraitFrames)
			{
				((Control)characterPotraitFrame).set_Width(_characterPotraitSize);
				((Control)characterPotraitFrame).set_Height(_characterPotraitSize);
				((Control)characterPotraitFrame).set_Location(pos);
				pos.X += _characterPotraitSize + _gap;
				index++;
			}
			((Control)_characterPotraitsBackground).set_Width(pos.X - ((Control)_characterPotraitsBackground).get_Location().X - _gap + 5);
			((Control)_characterPotraitsBackground).set_Height(_characterPotraitSize + 10);
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
			_draggingStart = (_dragging ? ((Control)this).get_RelativeMousePosition() : Point.get_Zero());
		}

		private void CapturePotraits()
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
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
			ScreenModeSetting? screenMode = GameService.GameIntegration.get_GfxSettings().get_ScreenMode();
			Point p = (Point)(((screenMode.HasValue ? ScreenModeSetting.op_Implicit(screenMode.GetValueOrDefault()) : null) == ScreenModeSetting.op_Implicit(ScreenModeSetting.get_Windowed())) ? new Point(_sharedSettings.WindowOffset.Left, _sharedSettings.WindowOffset.Top) : Point.get_Zero());
			double factor = GameService.Graphics.get_UIScaleMultiplier();
			Size size = new Size(_characterPotraitSize, _characterPotraitSize);
			foreach (FramedMaskedRegion characterPotraitFrame in _characterPotraitFrames)
			{
				Rectangle bounds = characterPotraitFrame.MaskedRegion;
				using Bitmap bitmap = new Bitmap((int)((double)bounds.Width * factor), (int)((double)bounds.Height * factor));
				using (Graphics g = Graphics.FromImage(bitmap))
				{
					int x = (int)((double)bounds.X * factor);
					int y = (int)((double)bounds.Y * factor);
					g.CopyFromScreen(new Point(wndBounds.Left + p.X + x, wndBounds.Top + p.Y + y), Point.Empty, size);
				}
				bitmap.Save(GetImagePath(images), ImageFormat.Png);
			}
			OnImageCaptured?.Invoke();
			ScreenNotification.ShowNotification(string.Format("[Characters]: " + strings.CapturedXPotraits, _characterPotraitFrames.Count), (NotificationType)0, (Texture2D)null, 4);
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
			((Control)this).OnMoved(e);
			if (_characterPotraitFrames.Count > 0)
			{
				RepositionPotraitFrames();
			}
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).OnShown(e);
			((Control)_characterPotraitsBackground).Show();
			foreach (FramedMaskedRegion characterPotraitFrame in _characterPotraitFrames)
			{
				((Control)characterPotraitFrame).Show();
			}
			ForceOnScreen();
		}

		protected override void OnHidden(EventArgs e)
		{
			((Control)this).OnHidden(e);
			((Control)_characterPotraitsBackground).Hide();
			foreach (FramedMaskedRegion characterPotraitFrame in _characterPotraitFrames)
			{
				((Control)characterPotraitFrame).Hide();
			}
		}

		private void ForceOnScreen()
		{
			Screen screen = Control.get_Graphics().get_SpriteScreen();
			if (((Control)this).get_Bottom() > ((Control)screen).get_Bottom())
			{
				((Control)this).set_Bottom(((Control)screen).get_Bottom());
			}
			if (((Control)this).get_Top() < ((Control)screen).get_Top() + ((Control)this).get_Height())
			{
				((Control)this).set_Top(((Control)screen).get_Top() + ((Control)this).get_Height());
			}
			if (((Control)this).get_Left() < ((Control)screen).get_Left())
			{
				((Control)this).set_Left(((Control)screen).get_Left());
			}
			if (((Control)this).get_Right() > ((Control)screen).get_Right())
			{
				((Control)this).set_Left(((Control)screen).get_Right() - ((Control)this).get_Width());
			}
		}
	}
}
