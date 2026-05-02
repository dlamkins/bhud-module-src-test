using System;
using System.Globalization;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Stopwatch.Core.Controls
{
	internal sealed class TimeSpanInputPrompt : Container
	{
		private static BitmapFont _font = GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0);

		private static TimeSpanInputPrompt _singleton;

		private Rectangle _confirmButtonBounds;

		private Rectangle _cancelButtonBounds;

		private Rectangle _inputTextBoxBounds;

		private StandardButton _confirmButton;

		private StandardButton _cancelButton;

		private TextBox _inputTextBox;

		private readonly Action<bool, TimeSpan> _callback;

		private readonly string _text;

		private readonly string _confirmButtonText;

		private readonly string _cancelButtonButtonText;

		private readonly string _defaultValue;

		private AsyncTexture2D _bgTexture;

		private TimeSpanInputPrompt(Action<bool, TimeSpan> callback, string text, string defaultValue, string confirmButtonText, string cancelButtonText)
			: this()
		{
			_callback = callback;
			_text = text;
			_defaultValue = defaultValue;
			_confirmButtonText = confirmButtonText;
			_cancelButtonButtonText = cancelButtonText;
			((Control)this).set_ZIndex(999);
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
			LoadTextures();
		}

		protected override void DisposeControl()
		{
			_singleton = null;
			AsyncTexture2D bgTexture = _bgTexture;
			if (bgTexture != null)
			{
				bgTexture.Dispose();
			}
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
			((Container)this).DisposeControl();
		}

		private void LoadTextures()
		{
			_bgTexture = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156003);
		}

		private static bool SafeParseTime(string input, out TimeSpan timeSpan)
		{
			timeSpan = TimeSpan.Zero;
			if (string.IsNullOrWhiteSpace(input))
			{
				return true;
			}
			input = input.Replace(',', '.');
			string[] parts = input.Split(':');
			try
			{
				double h;
				double i;
				double s;
				if (parts.Length == 1)
				{
					if (double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var s2))
					{
						timeSpan = TimeSpan.FromSeconds(s2);
						return true;
					}
				}
				else if (parts.Length == 2)
				{
					if (double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var j) && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var s3))
					{
						timeSpan = TimeSpan.FromMinutes(j).Add(TimeSpan.FromSeconds(s3));
						return true;
					}
				}
				else if (parts.Length == 3 && double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out h) && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out i) && double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out s))
				{
					timeSpan = TimeSpan.FromHours(h).Add(TimeSpan.FromMinutes(i)).Add(TimeSpan.FromSeconds(s));
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		public static void ShowPrompt(Action<bool, TimeSpan> callback, string text, string defaultValue = "", string confirmButtonText = "Confirm", string cancelButtonText = "Cancel")
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (_singleton == null)
			{
				TimeSpanInputPrompt timeSpanInputPrompt = new TimeSpanInputPrompt(callback, text, defaultValue, confirmButtonText, cancelButtonText);
				((Control)timeSpanInputPrompt).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
				((Control)timeSpanInputPrompt).set_Location(Point.get_Zero());
				((Control)timeSpanInputPrompt).set_Size(((Control)Control.get_Graphics().get_SpriteScreen()).get_Size());
				_singleton = timeSpanInputPrompt;
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
			//IL_005b: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected O, but got Unknown
			if (_confirmButton == null)
			{
				StandardButton val = new StandardButton();
				((Control)val).set_Parent((Container)(object)this);
				val.set_Text(_confirmButtonText);
				((Control)val).set_Size(((Rectangle)(ref _confirmButtonBounds)).get_Size());
				((Control)val).set_Location(((Rectangle)(ref _confirmButtonBounds)).get_Location());
				((Control)val).set_Enabled(SafeParseTime(_defaultValue, out var _));
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
			TimeSpan timeSpan = TimeSpan.Zero;
			SafeParseTime(((TextInputBase)_inputTextBox).get_Text(), out timeSpan);
			_callback(arg1: true, timeSpan);
			_singleton = null;
			((Control)this).Dispose();
		}

		private void Cancel()
		{
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
			GameService.Content.PlaySoundEffectByName("button-click");
			_callback(arg1: false, TimeSpan.Zero);
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
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			if (_inputTextBox == null)
			{
				TextBox val = new TextBox();
				((Control)val).set_Parent((Container)(object)this);
				((Control)val).set_Size(((Rectangle)(ref _inputTextBoxBounds)).get_Size());
				((Control)val).set_Location(((Rectangle)(ref _inputTextBoxBounds)).get_Location());
				((TextInputBase)val).set_Font(_font);
				((TextInputBase)val).set_Focused(false);
				val.set_HorizontalAlignment((HorizontalAlignment)1);
				((TextInputBase)val).set_Text(_defaultValue);
				((TextInputBase)val).set_PlaceholderText("MM:SS, SS, .fff");
				((TextInputBase)val).set_CursorIndex(_defaultValue.Length);
				_inputTextBox = val;
				((TextInputBase)_inputTextBox).add_TextChanged((EventHandler<EventArgs>)delegate(object o, EventArgs e)
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					string text = ((TextInputBase)(TextBox)o).get_Text();
					((Control)_confirmButton).set_Enabled(SafeParseTime(text, out var _));
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
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			Size2 textSize = _font.MeasureString(_text);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.8f);
			Point bgTextureSize = default(Point);
			((Point)(ref bgTextureSize))._002Ector((int)textSize.Width + 12, (int)textSize.Height + 145);
			Point bgTexturePos = default(Point);
			((Point)(ref bgTexturePos))._002Ector((bounds.Width - bgTextureSize.X) / 2, (bounds.Height - bgTextureSize.Y) / 2);
			Rectangle bgBounds = default(Rectangle);
			((Rectangle)(ref bgBounds))._002Ector(bgTexturePos, bgTextureSize);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bgBounds.X - 1, bgBounds.Y - 1, bgBounds.Width + 1, 1), Color.get_Black());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bgBounds.X - 1, bgBounds.Y - 1, 1, bgBounds.Height + 1), Color.get_Black());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bgBounds.X + bgBounds.Width, bgBounds.Y, 1, bgBounds.Height + 1), Color.get_Black());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(bgBounds.X, bgBounds.Y + bgBounds.Height, bgBounds.Width, 1), Color.get_Black());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_bgTexture), bgBounds, (Rectangle?)new Rectangle(29, 23, 942, 942), Color.get_White());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, _font, new Rectangle(bgBounds.X + 6, bgBounds.Y + 5, bgBounds.Width - 11, bgBounds.Height), Color.get_White(), true, (HorizontalAlignment)0, (VerticalAlignment)0);
			int btnMaxWith = Math.Min(100, bgBounds.Width / 2 - 10);
			_confirmButtonBounds = new Rectangle(((Rectangle)(ref bgBounds)).get_Left() + 5, ((Rectangle)(ref bgBounds)).get_Bottom() - 50, btnMaxWith, 45);
			_cancelButtonBounds = new Rectangle(((Rectangle)(ref _confirmButtonBounds)).get_Right() + 10, _confirmButtonBounds.Y, btnMaxWith, 45);
			_inputTextBoxBounds = new Rectangle(_confirmButtonBounds.X, _confirmButtonBounds.Y - 70, bgBounds.Width - 10, 45);
			CreateTextInput();
			CreateButtons();
			if (_inputTextBox != null && !string.IsNullOrWhiteSpace(((TextInputBase)_inputTextBox).get_Text()) && SafeParseTime(((TextInputBase)_inputTextBox).get_Text(), out var timeSpan))
			{
				string ms = ((timeSpan.Milliseconds > 0) ? $" {timeSpan.Milliseconds}ms" : "");
				string parsedText = ((timeSpan.TotalHours >= 1.0) ? $"{(int)timeSpan.TotalHours}h {timeSpan.Minutes}m {timeSpan.Seconds}s{ms}" : $"{(int)timeSpan.TotalMinutes}m {timeSpan.Seconds}s{ms}");
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, parsedText, GameService.Content.get_DefaultFont14(), new Rectangle(_inputTextBoxBounds.X, ((Rectangle)(ref _inputTextBoxBounds)).get_Bottom(), _inputTextBoxBounds.Width, 20), Color.get_LightGray(), false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}
	}
}
