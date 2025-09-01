using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended.Properties;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Blish_HUD.Extended
{
	internal sealed class ErrorPrompt : Container
	{
		[Flags]
		public enum DialogButtons : ushort
		{
			None = 0x0,
			OK = 0x1,
			Confirm = 0x2,
			Cancel = 0x4,
			Yes = 0x8,
			No = 0x10,
			Ignore = 0x20,
			Close = 0x40,
			Apply = 0x80
		}

		public enum DialogIcon
		{
			None,
			Exclamation,
			Question
		}

		private static ErrorPrompt _singleton;

		private AsyncTexture2D _bgTexture;

		private AsyncTexture2D _icon;

		private static BitmapFont _font = GameService.Content.GetFont((FontFace)0, (FontSize)20, (FontStyle)0);

		private Rectangle _bgBounds;

		private Point _iconMargin = new Point(9, 8);

		private const int BUTTON_HEIGHT = 25;

		private const int BUTTON_WIDTH = 112;

		private readonly Dictionary<DialogButtons, StandardButton> _buttons;

		private readonly Action<DialogButtons> _callback;

		private readonly string _text;

		private readonly DialogButtons _enterButton;

		private readonly DialogButtons _escapeButton;

		private ErrorPrompt(string text, DialogButtons buttons, Action<DialogButtons> callback = null, DialogIcon icon = DialogIcon.None, AsyncTexture2D customIcon = null, DialogButtons enterButton = DialogButtons.None, DialogButtons escapeButton = DialogButtons.None)
			: this()
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			_text = text;
			_buttons = new Dictionary<DialogButtons, StandardButton>();
			foreach (DialogButtons button in Enum.GetValues(typeof(DialogButtons)))
			{
				if (button != 0 && buttons.HasFlag(button))
				{
					_buttons[button] = null;
				}
			}
			if (!IsValidDialog(out var errorMessage))
			{
				throw new ArgumentException(errorMessage);
			}
			_enterButton = enterButton;
			_escapeButton = escapeButton;
			_callback = callback;
			((Control)this).set_ZIndex(999);
			LoadIcon(icon, customIcon);
			LoadTextures();
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
		}

		private void LoadTextures()
		{
			_bgTexture = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156003);
		}

		private void LoadIcon(DialogIcon icon, AsyncTexture2D customIcon)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			if (customIcon != null)
			{
				_icon = customIcon;
			}
			else
			{
				if (icon <= DialogIcon.None)
				{
					return;
				}
				_icon = new AsyncTexture2D();
				GameService.Content.get_DatAssetCache().GetTextureFromAssetId(154985).add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate(object _, ValueChangedEventArgs<Texture2D> e)
				{
					if (e.get_NewValue() != null)
					{
						GetIconRegion(icon, e.get_NewValue());
					}
				});
			}
		}

		private void GetIconRegion(DialogIcon icon, Texture2D atlas)
		{
			switch (icon)
			{
			case DialogIcon.Exclamation:
				_icon.SwapTexture(Texture2DExtension.GetRegion(atlas, 0, 0, 64, 64));
				break;
			case DialogIcon.Question:
				_icon.SwapTexture(Texture2DExtension.GetRegion(atlas, 64, 0, 64, 64));
				break;
			}
		}

		private bool IsValidDialog(out string errorMessage)
		{
			errorMessage = string.Empty;
			if (_buttons.Count < 1)
			{
				errorMessage += "Prompt dialog must have at least one button. ";
			}
			if (string.IsNullOrWhiteSpace(_text))
			{
				errorMessage += "Prompt dialog must have text content.";
			}
			return string.IsNullOrEmpty(errorMessage);
		}

		protected override void DisposeControl()
		{
			_singleton = null;
			AsyncTexture2D icon = _icon;
			if (icon != null)
			{
				icon.Dispose();
			}
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
			((Container)this).DisposeControl();
		}

		private void ButtonPress(DialogButtons button)
		{
			if (button != 0)
			{
				GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
				GameService.Content.PlaySoundEffectByName("button-click");
				_callback?.Invoke(button);
				_singleton = null;
				((Control)this).Dispose();
			}
		}

		private void OnKeyPressed(object o, KeyboardEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Invalid comparison between Unknown and I4
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			Keys key = e.get_Key();
			if ((int)key != 13)
			{
				if ((int)key == 27)
				{
					ButtonPress(_escapeButton);
				}
			}
			else
			{
				ButtonPress(_enterButton);
			}
		}

		public static void Show(string text, DialogButtons buttons, Action<DialogButtons> callback = null, DialogButtons enterButton = DialogButtons.None, DialogButtons escapeButton = DialogButtons.None)
		{
			Show(text, DialogIcon.None, null, buttons, callback, enterButton, escapeButton);
		}

		public static void Show(string text, DialogIcon icon, DialogButtons buttons, Action<DialogButtons> callback = null, DialogButtons enterButton = DialogButtons.None, DialogButtons escapeButton = DialogButtons.None)
		{
			Show(text, icon, null, buttons, callback, enterButton, escapeButton);
		}

		public static void Show(string text, AsyncTexture2D icon, DialogButtons buttons, Action<DialogButtons> callback = null, DialogButtons enterButton = DialogButtons.None, DialogButtons escapeButton = DialogButtons.None)
		{
			Show(text, DialogIcon.None, icon, buttons, callback, enterButton, escapeButton);
		}

		private static void Show(string text, DialogIcon icon, AsyncTexture2D customIcon, DialogButtons buttons, Action<DialogButtons> callback, DialogButtons enterButton, DialogButtons escapeButton)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (_singleton == null)
			{
				ErrorPrompt errorPrompt = new ErrorPrompt(text, buttons, callback, icon, customIcon, enterButton, escapeButton);
				((Control)errorPrompt).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
				((Control)errorPrompt).set_Location(Point.get_Zero());
				((Control)errorPrompt).set_Size(((Control)Control.get_Graphics().get_SpriteScreen()).get_Size());
				_singleton = errorPrompt;
				((Control)_singleton).Show();
			}
		}

		private void CreateButtons()
		{
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			int minLeftOffset = 50;
			int buttonCount = _buttons.Count;
			int availableWidth = _bgBounds.Width - minLeftOffset;
			int buttonWidth = 112;
			if (buttonCount * buttonWidth > availableWidth)
			{
				buttonWidth = availableWidth / buttonCount;
			}
			int xOffset = _bgBounds.Width - minLeftOffset - buttonWidth - 4;
			int yOffset = ((Rectangle)(ref _bgBounds)).get_Bottom() - 25 - 7;
			foreach (DialogButtons buttonKey in _buttons.Keys.ToList())
			{
				StandardButton button = _buttons[buttonKey];
				if (button == null)
				{
					StandardButton val = new StandardButton();
					((Control)val).set_Parent((Container)(object)this);
					val.set_Text(GetButtonText(buttonKey));
					((Control)val).set_Width(buttonWidth);
					((Control)val).set_Height(25);
					((Control)val).set_Location(new Point(((Rectangle)(ref _bgBounds)).get_Left() + minLeftOffset + xOffset, yOffset));
					((Control)val).set_Enabled(true);
					button = val;
					((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						ButtonPress(buttonKey);
					});
				}
				xOffset -= buttonWidth + _iconMargin.X;
				_buttons[buttonKey] = button;
			}
		}

		private string GetButtonText(DialogButtons button)
		{
			return button switch
			{
				DialogButtons.OK => Resources.OK, 
				DialogButtons.Confirm => Resources.Confirm, 
				DialogButtons.Cancel => Resources.Cancel, 
				DialogButtons.Yes => Resources.Yes, 
				DialogButtons.No => Resources.No, 
				DialogButtons.Ignore => Resources.Ignore, 
				DialogButtons.Close => Resources.Close, 
				DialogButtons.Apply => Resources.Apply, 
				_ => string.Empty, 
			};
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			int textMarginRight = 25;
			string text = DrawUtil.WrapText(_font, _text, 412f);
			Size2 val = _font.MeasureString(text);
			int textWidth = (int)val.Width;
			int textHeight = (int)val.Height;
			int iconSize = ((_icon != null) ? 64 : 0);
			Point textMargin = default(Point);
			((Point)(ref textMargin))._002Ector(iconSize + _iconMargin.X * 2, 17);
			int contentWidth = textWidth + iconSize + _iconMargin.X + textMargin.X + textMarginRight;
			int contentHeight = ((textHeight > 64) ? textHeight : (textHeight + iconSize));
			contentHeight = ((contentHeight < 150) ? 150 : contentHeight);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.5f);
			Point bgTextureSize = default(Point);
			((Point)(ref bgTextureSize))._002Ector(contentWidth, contentHeight + 64);
			Point bgTexturePos = default(Point);
			((Point)(ref bgTexturePos))._002Ector((bounds.Width - bgTextureSize.X) / 2, (bounds.Height - bgTextureSize.Y) / 2);
			Rectangle bgBounds = default(Rectangle);
			((Rectangle)(ref bgBounds))._002Ector(bgTexturePos, bgTextureSize);
			_bgBounds = bgBounds;
			Rectangle textBounds = default(Rectangle);
			((Rectangle)(ref textBounds))._002Ector(((Rectangle)(ref bgBounds)).get_Left() + textMargin.X, bgBounds.Y + textMargin.Y, bgBounds.Width - textMarginRight, contentHeight);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_bgTexture), bgBounds, (Rectangle?)new Rectangle(29, 23, 942, 942), Color.get_White());
			spriteBatch.DrawRectangleOnCtrl((Control)(object)this, _bgBounds, 2, Color.get_Black() * 0.8f);
			if (_icon != null && _icon.get_HasTexture())
			{
				Rectangle iconBounds = default(Rectangle);
				((Rectangle)(ref iconBounds))._002Ector(((Rectangle)(ref bgBounds)).get_Left() + _iconMargin.X, ((Rectangle)(ref bgBounds)).get_Top() + _iconMargin.Y, 64, 64);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), iconBounds);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, _font, textBounds, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)0);
			CreateButtons();
		}
	}
}
