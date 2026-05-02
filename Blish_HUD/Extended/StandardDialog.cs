using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended.Properties;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blish_HUD.Extended
{
	public class StandardDialog : Container
	{
		public enum DialogIcon
		{
			None,
			Exclamation,
			Question,
			Present
		}

		public sealed class DialogButton
		{
			internal string Text;

			internal bool Selected;

			private Action _callback;

			private StandardButton _button;

			public static DialogButton OK => new DialogButton(Resources.Action_OK);

			public static DialogButton Confirm => new DialogButton(Resources.Action_Confirm);

			public static DialogButton Accept => new DialogButton(Resources.Action_Accept);

			public static DialogButton Cancel => new DialogButton(Resources.Action_Cancel);

			public static DialogButton Yes => new DialogButton(Resources.Action_Yes);

			public static DialogButton No => new DialogButton(Resources.Action_No);

			public static DialogButton Ignore => new DialogButton(Resources.Action_Ignore);

			public static DialogButton Close => new DialogButton(Resources.Action_Close);

			public static DialogButton Apply => new DialogButton(Resources.Action_Apply);

			public static DialogButton Decline => new DialogButton(Resources.Action_Decline);

			public event EventHandler<MouseEventArgs> Click;

			private DialogButton(string text)
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Expected O, but got Unknown
				if (string.IsNullOrEmpty(text))
				{
					throw new ArgumentNullException("text", "[DialogButton] Parameter 'text' cannot be null or empty.");
				}
				Text = text;
				StandardButton val = new StandardButton();
				val.set_Text(text);
				((Control)val).set_Enabled(false);
				_button = val;
				((Control)_button).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					DoClick();
				});
			}

			internal void DoClick()
			{
				GameService.Content.PlaySoundEffectByName("button-click");
				_callback?.Invoke();
				this.Click?.Invoke(this, null);
			}

			internal void Transform(Container parent, Rectangle bounds)
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				((Control)_button).set_Parent(parent);
				((Control)_button).set_Location(((Rectangle)(ref bounds)).get_Location());
				((Control)_button).set_Size(((Rectangle)(ref bounds)).get_Size());
				((Control)_button).set_Enabled(true);
			}

			public DialogButton Action(Action callback)
			{
				_callback = callback;
				return this;
			}

			public DialogButton Select(bool selected = true)
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				Selected = selected;
				((Control)_button).set_BackgroundColor((Color)(selected ? new Color(192, 216, 255, 217) : Color.get_Transparent()));
				return this;
			}

			public static DialogButton Create(string text)
			{
				return new DialogButton(text);
			}
		}

		private static Texture2D _questionIcon;

		private static Texture2D _exclamationIcon;

		private static Texture2D _presentIcon;

		private AsyncTexture2D _bgTexture;

		private Rectangle _bgTextureBounds;

		private AsyncTexture2D _icon;

		private Rectangle _bgBounds;

		private const int DIALOG_WIDTH = 454;

		private const int DIALOG_HEIGHT = 100;

		private const int BUTTON_HEIGHT = 24;

		private const int BUTTON_WIDTH = 117;

		private const int ICON_SIZE = 64;

		private const int ICON_MARGIN = 5;

		private const int BUTTON_MARGIN = 3;

		private int _maxDialogWidth;

		private int _maxDialogHeight;

		private int _maxIconSize;

		private int _maxButtonWidth;

		private readonly List<DialogButton> _buttons;

		private readonly FormattedLabel _label;

		private StandardDialog(Container parent, FormattedLabelBuilder label, AsyncTexture2D icon, List<DialogButton> buttons)
			: this()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			if (parent == null)
			{
				throw new ArgumentNullException("parent", "[StandardDialog] Parameter 'parent' cannot be null.");
			}
			if (label == null)
			{
				throw new ArgumentNullException("label", "[StandardDialog] Parameter 'label' cannot be null.");
			}
			((Control)this).set_Parent(parent);
			((Control)this).set_Location(Point.get_Zero());
			((Control)this).set_Size(((Control)parent).get_Size());
			_maxIconSize = ((icon != null) ? 64 : 0);
			_maxDialogWidth = CalculateWidth();
			int labelPadding = _maxIconSize + 40;
			int labelWidth = _maxDialogWidth - labelPadding;
			if (labelWidth <= 0)
			{
				throw new Exception("[StandardDialog] Parent container width is too small.");
			}
			_label = label.SetWidth(labelWidth).AutoSizeHeight().Wrap()
				.Build();
			if (((Control)_label).get_Height() <= 0)
			{
				throw new ArgumentException("[StandardDialog] Parameter 'label' must have non-empty text or its height failed to calculate.", "label");
			}
			_maxDialogHeight = CalculateHeight();
			if (_maxDialogHeight > ((Control)this).get_Parent().get_ContentRegion().Height - 14)
			{
				throw new Exception("[StandardDialog] Parameter 'label' exceeded parent container height.");
			}
			if (buttons == null || buttons.Count == 0)
			{
				buttons = new List<DialogButton> { DialogButton.OK };
			}
			if (buttons.Count((DialogButton b) => b.Selected) > 1)
			{
				throw new ArgumentException("[StandardDialog] Only one DialogButton can be selected by default.", "buttons");
			}
			((Control)_label).set_Parent((Container)(object)this);
			_icon = icon;
			_buttons = buttons;
			_maxButtonWidth = 117;
			foreach (DialogButton button in _buttons)
			{
				float bttnWidth = GameService.Content.get_DefaultFont14().MeasureString(button.Text).Width + 8f;
				if (bttnWidth > (float)_maxButtonWidth)
				{
					_maxButtonWidth = (int)Math.Round(bttnWidth);
				}
				button.Click += delegate
				{
					((Control)this).Dispose();
				};
			}
			((Control)this).set_ZIndex(2147483599);
			LoadTextures();
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
		}

		private void LoadTextures()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			_bgTexture = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156003);
			_bgTextureBounds = new Rectangle(33, 27, 936, 936);
		}

		protected override void DisposeControl()
		{
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
			((Container)this).DisposeControl();
		}

		private void OnKeyPressed(object o, KeyboardEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Invalid comparison between Unknown and I4
			if ((int)e.get_Key() == 13)
			{
				_buttons.FirstOrDefault((DialogButton b) => b.Selected)?.DoClick();
			}
			else if ((int)e.get_Key() == 9)
			{
				CycleButtonFocus();
			}
		}

		private void CycleButtonFocus()
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Invalid comparison between Unknown and I4
			GameService.Content.PlaySoundEffectByName("menu-item-click");
			int currentIndex = _buttons.FindIndex((DialogButton b) => b.Selected);
			if (currentIndex < 0)
			{
				currentIndex = 0;
			}
			int newIndex = (((GameService.Input.get_Keyboard().get_ActiveModifiers() & 4) > 0) ? ((currentIndex - 1 + _buttons.Count) % _buttons.Count) : ((currentIndex + 1) % _buttons.Count));
			for (int i = 0; i < _buttons.Count; i++)
			{
				_buttons[i].Select(i == newIndex);
			}
		}

		public static void Show(FormattedLabelBuilder label, params DialogButton[] buttons)
		{
			Show(null, label, null, buttons);
		}

		public static void Show(string text, DialogIcon sysIcon, params DialogButton[] buttons)
		{
			Show(null, GetDefaultLabel(text), GetDefaultIcon(sysIcon), buttons);
		}

		public static void Show(string text, AsyncTexture2D customIcon, params DialogButton[] buttons)
		{
			Show(null, GetDefaultLabel(text), customIcon, buttons);
		}

		public static void Show(string text, params DialogButton[] buttons)
		{
			Show(null, GetDefaultLabel(text), null, buttons);
		}

		public static void Show(FormattedLabelBuilder label, DialogIcon sysIcon, params DialogButton[] buttons)
		{
			Show(null, label, GetDefaultIcon(sysIcon), buttons);
		}

		public static void Show(FormattedLabelBuilder label, AsyncTexture2D customIcon, params DialogButton[] buttons)
		{
			Show(null, label, customIcon, buttons);
		}

		public static void Show(Container parent, FormattedLabelBuilder label, params DialogButton[] buttons)
		{
			Show(parent, label, null, buttons);
		}

		public static void Show(Container parent, string text, AsyncTexture2D customIcon, params DialogButton[] buttons)
		{
			Show(parent, GetDefaultLabel(text), customIcon, buttons);
		}

		public static void Show(Container parent, string text, params DialogButton[] buttons)
		{
			Show(parent, GetDefaultLabel(text), null, buttons);
		}

		public static void Show(Container parent, FormattedLabelBuilder label, DialogIcon sysIcon, params DialogButton[] buttons)
		{
			Show(parent, label, GetDefaultIcon(sysIcon), buttons);
		}

		public static void Show(Container parent, string text, DialogIcon sysIcon, params DialogButton[] buttons)
		{
			Show(parent, GetDefaultLabel(text), GetDefaultIcon(sysIcon), buttons);
		}

		public static void Show(Container parent, FormattedLabelBuilder label, AsyncTexture2D customIcon, params DialogButton[] buttons)
		{
			if (parent == null)
			{
				parent = (Container)(object)GameService.Graphics.get_SpriteScreen();
			}
			((Control)new StandardDialog(parent, label, customIcon, buttons?.ToList())).Show();
		}

		private static FormattedLabelBuilder GetDefaultLabel(string text)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new FormattedLabelBuilder().CreatePart(text, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
			{
				o.SetFontSize((FontSize)16);
			});
		}

		private static AsyncTexture2D GetDefaultIcon(DialogIcon sysIcon)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			AsyncTexture2D iconTex = new AsyncTexture2D();
			GameService.Content.get_DatAssetCache().GetTextureFromAssetId(154985).add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate(object o, ValueChangedEventArgs<Texture2D> e)
			{
				if (_exclamationIcon == null)
				{
					_exclamationIcon = Texture2DExtension.GetRegion(e.get_NewValue(), 0, 0, 64, 64);
				}
				if (_questionIcon == null)
				{
					_questionIcon = Texture2DExtension.GetRegion(e.get_NewValue(), 64, 0, 64, 64);
				}
				if (_presentIcon == null)
				{
					_presentIcon = Texture2DExtension.GetRegion(e.get_NewValue(), 128, 0, 64, 64);
				}
				SwapDefaultIcon(sysIcon, iconTex);
			});
			SwapDefaultIcon(sysIcon, iconTex);
			return iconTex;
		}

		private static void SwapDefaultIcon(DialogIcon sysIcon, AsyncTexture2D iconTex)
		{
			switch (sysIcon)
			{
			case DialogIcon.Exclamation:
				iconTex.SwapTexture(_exclamationIcon);
				break;
			case DialogIcon.Question:
				iconTex.SwapTexture(_questionIcon);
				break;
			case DialogIcon.Present:
				iconTex.SwapTexture(_presentIcon);
				break;
			}
		}

		private void CalculateButtonLayout()
		{
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			int buttonCount = _buttons.Count;
			int availableWidth = _bgBounds.Width - 8;
			int buttonWidth = _maxButtonWidth + 8;
			int totalMargins = buttonCount * 3;
			int totalWidth = buttonCount * buttonWidth + totalMargins;
			if (totalWidth > availableWidth)
			{
				buttonWidth = Math.Max(1, (availableWidth - totalMargins) / buttonCount);
				totalWidth = buttonCount * buttonWidth + totalMargins;
			}
			int xOffset = ((Rectangle)(ref _bgBounds)).get_Right() - totalWidth - 4;
			int yOffset = ((Rectangle)(ref _bgBounds)).get_Bottom() - 24 - 7;
			Rectangle bounds = default(Rectangle);
			foreach (DialogButton button in _buttons)
			{
				if (button != null)
				{
					((Rectangle)(ref bounds))._002Ector(new Point(xOffset, yOffset), new Point(buttonWidth, 24));
					button.Transform((Container)(object)this, bounds);
					xOffset += buttonWidth + 3;
				}
			}
		}

		private int CalculateWidth()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			int dialogWidth = 454;
			int dialogMargin = 16 + _maxIconSize + 5 + 8;
			if (((Control)this).get_Parent().get_ContentRegion().Width < 454 + dialogMargin)
			{
				dialogWidth = ((Control)this).get_Parent().get_ContentRegion().Width - dialogMargin;
			}
			return dialogWidth;
		}

		private int CalculateHeight()
		{
			int textHeight = ((((Control)_label).get_Height() < 100) ? 100 : ((Control)_label).get_Height());
			return ((textHeight > _maxIconSize) ? textHeight : (textHeight + _maxIconSize)) + 24 + 28;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			AsyncTexture2D icon = _icon;
			Point textPos = default(Point);
			((Point)(ref textPos))._002Ector(_maxIconSize + 5 + 8, 17);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.15f);
			Point bgSize = default(Point);
			((Point)(ref bgSize))._002Ector(_maxDialogWidth, _maxDialogHeight);
			Point bgTextureSize = default(Point);
			((Point)(ref bgTextureSize))._002Ector((bgSize.X < _bgTextureBounds.Width) ? bgSize.X : _bgTextureBounds.Width, (bgSize.Y < _bgTextureBounds.Height) ? bgSize.Y : _bgTextureBounds.Height);
			Point bgTexturePos = default(Point);
			((Point)(ref bgTexturePos))._002Ector((bounds.Width - bgTextureSize.X) / 2, (bounds.Height - bgTextureSize.Y) / 2);
			Rectangle bgBounds = default(Rectangle);
			((Rectangle)(ref bgBounds))._002Ector(bgTexturePos, bgSize);
			_bgBounds = bgBounds;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_bgTexture), bgBounds, (Rectangle?)new Rectangle(((Rectangle)(ref _bgTextureBounds)).get_Location(), bgTextureSize), Color.get_White());
			spriteBatch.DrawBorderOnCtrl((Control)(object)this, _bgBounds, Color.get_Black(), 2);
			if (icon != null && icon.get_HasTexture())
			{
				Rectangle slotBounds = default(Rectangle);
				((Rectangle)(ref slotBounds))._002Ector(((Rectangle)(ref bgBounds)).get_Left() + 5, ((Rectangle)(ref bgBounds)).get_Top() + 5 + 2, _maxIconSize, _maxIconSize);
				int texWidth = icon.get_Width();
				int texHeight = icon.get_Height();
				float scale = Math.Min(1f, Math.Min((float)_maxIconSize / (float)texWidth, (float)_maxIconSize / (float)texHeight));
				int drawWidth = (int)Math.Round((float)texWidth * scale);
				int drawHeight = (int)Math.Round((float)texHeight * scale);
				int x = ((Rectangle)(ref slotBounds)).get_Left() + (slotBounds.Width - drawWidth) / 2;
				int y = ((Rectangle)(ref slotBounds)).get_Top();
				Rectangle iconBounds = default(Rectangle);
				((Rectangle)(ref iconBounds))._002Ector(x, y, drawWidth, drawHeight);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), slotBounds, Color.get_Black() * 0.15f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(icon), iconBounds);
			}
			((Control)_label).set_Location(new Point(((Rectangle)(ref bgBounds)).get_Left() + textPos.X, bgBounds.Y + textPos.Y));
			CalculateButtonLayout();
		}
	}
}
