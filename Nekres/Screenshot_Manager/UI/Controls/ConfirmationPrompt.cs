using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Screenshot_Manager.UI.Controls
{
	internal sealed class ConfirmationPrompt : Container
	{
		private static Texture2D _bgTexture = GameService.Content.GetTexture("tooltip");

		private static BitmapFont _font = GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0);

		private Rectangle _confirmButtonBounds;

		private Rectangle _cancelButtonBounds;

		private Rectangle _challengeTextBoxBounds;

		private StandardButton _confirmButton;

		private StandardButton _cancelButton;

		private TextBox _challengeTextBox;

		private readonly Action<bool> _callback;

		private readonly string _text;

		private readonly string _confirmButtonText;

		private readonly string _cancelButtonButtonText;

		private readonly string _challengeText;

		private ConfirmationPrompt(Action<bool> callback, string text, string confirmButtonText, string cancelButtonText, string challengeText)
			: this()
		{
			_callback = callback;
			_text = text;
			_confirmButtonText = confirmButtonText;
			_cancelButtonButtonText = cancelButtonText;
			_challengeText = challengeText;
			((Control)this).set_ZIndex(999);
		}

		public static void ShowPrompt(Action<bool> callback, string text, string confirmButtonText = "Confirm", string cancelButtonText = "Cancel", string challengeText = "")
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			ConfirmationPrompt confirmationPrompt = new ConfirmationPrompt(callback, text, confirmButtonText, cancelButtonText, challengeText);
			((Control)confirmationPrompt).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((Control)confirmationPrompt).set_Location(Point.get_Zero());
			((Control)confirmationPrompt).set_Size(((Control)Control.get_Graphics().get_SpriteScreen()).get_Size());
			((Control)confirmationPrompt).Show();
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
			//IL_0059: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Expected O, but got Unknown
			if (_confirmButton == null)
			{
				StandardButton val = new StandardButton();
				((Control)val).set_Parent((Container)(object)this);
				val.set_Text(_confirmButtonText);
				((Control)val).set_Size(((Rectangle)(ref _confirmButtonBounds)).get_Size());
				((Control)val).set_Location(((Rectangle)(ref _confirmButtonBounds)).get_Location());
				((Control)val).set_Enabled(string.IsNullOrEmpty(_challengeText));
				_confirmButton = val;
				((Control)_confirmButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					_callback(obj: true);
					((Control)this).Dispose();
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
					GameService.Content.PlaySoundEffectByName("button-click");
					_callback(obj: false);
					((Control)this).Dispose();
				});
			}
		}

		private void CreateTextInput()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			if (_challengeTextBox == null)
			{
				TextBox val = new TextBox();
				((Control)val).set_Parent((Container)(object)this);
				((Control)val).set_Size(((Rectangle)(ref _challengeTextBoxBounds)).get_Size());
				((Control)val).set_Location(((Rectangle)(ref _challengeTextBoxBounds)).get_Location());
				((TextInputBase)val).set_Font(_font);
				_challengeTextBox = val;
				((TextInputBase)_challengeTextBox).add_TextChanged((EventHandler<EventArgs>)delegate(object o, EventArgs _)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					((Control)_confirmButton).set_Enabled(((TextInputBase)(TextBox)o).get_Text().Equals(_challengeText));
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
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			Size2 textSize = _font.MeasureString(_text);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.8f);
			Point bgTextureSize = default(Point);
			((Point)(ref bgTextureSize))._002Ector((int)textSize.Width + 12, (int)textSize.Height + ((!string.IsNullOrEmpty(_challengeText)) ? 125 : 60));
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
			if (!string.IsNullOrEmpty(_challengeText))
			{
				_challengeTextBoxBounds = new Rectangle(_confirmButtonBounds.X, _confirmButtonBounds.Y - 55, bgBounds.Width - 10, 45);
				CreateTextInput();
			}
			CreateButtons();
		}
	}
}
