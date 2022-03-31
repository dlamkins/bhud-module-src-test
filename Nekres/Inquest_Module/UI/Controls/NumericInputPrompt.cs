using System;
using System.Globalization;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Inquest_Module.UI.Controls
{
	internal sealed class NumericInputPrompt : Container
	{
		private static Texture2D _bgTexture = GameService.Content.GetTexture("tooltip");

		private static BitmapFont _font = GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0);

		private static NumericInputPrompt _singleton;

		private Rectangle _confirmButtonBounds;

		private Rectangle _cancelButtonBounds;

		private Rectangle _inputTextBoxBounds;

		private StandardButton _confirmButton;

		private StandardButton _cancelButton;

		private TextBox _inputTextBox;

		private readonly Action<bool, double> _callback;

		private readonly string _text;

		private readonly string _confirmButtonText;

		private readonly string _cancelButtonButtonText;

		private readonly double _defaultValue;

		private NumericInputPrompt(Action<bool, double> callback, string text, double defaultValue, string confirmButtonText, string cancelButtonText)
			: this()
		{
			_callback = callback;
			_text = text;
			_defaultValue = defaultValue;
			_confirmButtonText = confirmButtonText;
			_cancelButtonButtonText = cancelButtonText;
			((Control)this).set_ZIndex(999);
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
		}

		public static void ShowPrompt(Action<bool, double> callback, string text, double defaultValue = 0.0, string confirmButtonText = "Confirm", string cancelButtonText = "Cancel")
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (_singleton == null)
			{
				NumericInputPrompt numericInputPrompt = new NumericInputPrompt(callback, text, defaultValue, confirmButtonText, cancelButtonText);
				((Control)numericInputPrompt).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
				((Control)numericInputPrompt).set_Location(Point.get_Zero());
				((Control)numericInputPrompt).set_Size(((Control)Control.get_Graphics().get_SpriteScreen()).get_Size());
				_singleton = numericInputPrompt;
				((Control)_singleton).Show();
			}
		}

		private void CreateButtons()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Expected O, but got Unknown
			if (_confirmButton == null)
			{
				StandardButton val = new StandardButton();
				((Control)val).set_Parent((Container)(object)this);
				val.set_Text(_confirmButtonText);
				((Control)val).set_Size(((Rectangle)(ref _confirmButtonBounds)).get_Size());
				((Control)val).set_Location(((Rectangle)(ref _confirmButtonBounds)).get_Location());
				((Control)val).set_Enabled(_defaultValue > 0.0);
				_confirmButton = val;
				((Control)_confirmButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Confirm();
				});
			}
			if (_cancelButton == null)
			{
				StandardButton val2 = new StandardButton();
				((Control)val2).set_Parent((Container)(object)this);
				val2.set_Text(_cancelButtonButtonText);
				((Control)val2).set_Size(((Rectangle)(ref _cancelButtonBounds)).get_Size());
				((Control)val2).set_Location(((Rectangle)(ref _cancelButtonBounds)).get_Location());
				_cancelButton = val2;
				((Control)_cancelButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Cancel();
				});
			}
		}

		private void Confirm()
		{
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
			GameService.Content.PlaySoundEffectByName("button-click");
			_callback(arg1: true, double.Parse(((TextInputBase)_inputTextBox).get_Text(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
			_singleton = null;
			((Control)this).Dispose();
		}

		private void Cancel()
		{
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
			GameService.Content.PlaySoundEffectByName("button-click");
			_callback(arg1: false, 0.0);
			_singleton = null;
			((Control)this).Dispose();
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
					Cancel();
				}
			}
			else if (((Control)_confirmButton).get_Enabled())
			{
				Confirm();
			}
		}

		private void CreateTextInput()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected O, but got Unknown
			if (_inputTextBox == null)
			{
				string defaultText = ((_defaultValue > 0.0) ? _defaultValue.ToString(CultureInfo.InvariantCulture) : string.Empty);
				TextBox val = new TextBox();
				((Control)val).set_Parent((Container)(object)this);
				((Control)val).set_Size(((Rectangle)(ref _inputTextBoxBounds)).get_Size());
				((Control)val).set_Location(((Rectangle)(ref _inputTextBoxBounds)).get_Location());
				((TextInputBase)val).set_Font(_font);
				((TextInputBase)val).set_Focused(true);
				val.set_HorizontalAlignment((HorizontalAlignment)1);
				((TextInputBase)val).set_Text(defaultText);
				((TextInputBase)val).set_CursorIndex(defaultText.Length);
				_inputTextBox = val;
				((TextInputBase)_inputTextBox).add_TextChanged((EventHandler<EventArgs>)delegate(object o, EventArgs _)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					((Control)_confirmButton).set_Enabled(double.TryParse(((TextInputBase)(TextBox)o).get_Text(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var _));
				});
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			Size2 textSize = _font.MeasureString(_text);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.8f);
			Point bgTextureSize = default(Point);
			((Point)(ref bgTextureSize))._002Ector((int)textSize.Width + 12, (int)textSize.Height + 125);
			Point bgTexturePos = default(Point);
			((Point)(ref bgTexturePos))._002Ector((bounds.Width - bgTextureSize.X) / 2, (bounds.Height - bgTextureSize.Y) / 2);
			Rectangle bgBounds = default(Rectangle);
			((Rectangle)(ref bgBounds))._002Ector(bgTexturePos, bgTextureSize);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bgBounds.X - 1, bgBounds.Y - 1, bgBounds.Width + 1, 1), Color.get_Black());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bgBounds.X - 1, bgBounds.Y - 1, 1, bgBounds.Height + 1), Color.get_Black());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bgBounds.X + bgBounds.Width, bgBounds.Y, 1, bgBounds.Height + 1), Color.get_Black());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bgBounds.X, bgBounds.Y + bgBounds.Height, bgBounds.Width, 1), Color.get_Black());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _bgTexture, bgBounds, Color.get_White());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, _font, new Rectangle(bgBounds.X + 6, bgBounds.Y + 5, bgBounds.Width - 11, bgBounds.Height), Color.get_White(), true, (HorizontalAlignment)0, (VerticalAlignment)0);
			_confirmButtonBounds = new Rectangle(((Rectangle)(ref bgBounds)).get_Left() + 5, ((Rectangle)(ref bgBounds)).get_Bottom() - 50, 100, 45);
			_cancelButtonBounds = new Rectangle(((Rectangle)(ref _confirmButtonBounds)).get_Right() + 10, _confirmButtonBounds.Y, 100, 45);
			_inputTextBoxBounds = new Rectangle(_confirmButtonBounds.X, _confirmButtonBounds.Y - 55, bgBounds.Width - 10, 45);
			CreateTextInput();
			CreateButtons();
		}
	}
}
