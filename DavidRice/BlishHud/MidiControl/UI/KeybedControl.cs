using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using DavidRice.BlishHud.MidiControl.Keymaps.Visualization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace DavidRice.BlishHud.MidiControl.UI
{
	public class KeybedControl : Control
	{
		private static readonly Logger Logger = Logger.GetLogger<KeybedControl>();

		private const int WhiteKeyWidth = 24;

		private const int KeyPadding = 4;

		private const int DefaultWidth = 420;

		private const int NoteLabelHeight = 14;

		private const int DefaultHeight = 110;

		private KeybedLayout _layout = KeybedLayout.Empty;

		private KeybedKey? _hoveredKey;

		private readonly Dictionary<int, Rectangle> _keyRects = new Dictionary<int, Rectangle>();

		private static Texture2D? _pixelTexture;

		public KeybedLayout Layout
		{
			get
			{
				return _layout;
			}
			set
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				if (_layout == value)
				{
					return;
				}
				_layout = value;
				if (_layout.IsEmpty)
				{
					((Control)this).set_Size(new Point(420, 110));
				}
				else
				{
					int desiredWidth = _layout.Keys.Count((KeybedKey k) => !k.IsBlackKey) * 24 + 8;
					((Control)this).set_Size(new Point(desiredWidth, 110));
				}
				RebuildKeyRects();
				((Control)this).Invalidate();
			}
		}

		public KeybedControl()
			: this()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(420, 110));
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			((Control)this).OnMouseMoved(e);
			UpdateHoveredKey();
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
			UpdateHoveredKey();
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			if (_hoveredKey != null)
			{
				_hoveredKey = null;
				((Control)this).set_BasicTooltipText("");
				((Control)this).Invalidate();
			}
		}

		private void UpdateHoveredKey()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			KeybedKey newHover = null;
			Point mousePos = ((Control)this).get_RelativeMousePosition();
			foreach (KeybedKey key in _layout.Keys)
			{
				if (_keyRects.TryGetValue(key.NoteNumber, out var rect) && ((Rectangle)(ref rect)).Contains(mousePos))
				{
					newHover = key;
					break;
				}
			}
			if (_hoveredKey?.NoteNumber != newHover?.NoteNumber)
			{
				_hoveredKey = newHover;
				((Control)this).set_BasicTooltipText(BuildTooltipText(newHover));
				((Control)this).Invalidate();
			}
		}

		private static string BuildTooltipText(KeybedKey? key)
		{
			if (key == null)
			{
				return "";
			}
			if (!key!.IsMapped)
			{
				return key!.NoteName + " (unmapped)";
			}
			if (key!.IsKeySwitch)
			{
				return key!.NoteName + "\r\nOctave shift (Key: " + key!.Gw2Key + ")";
			}
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(key!.NoteName);
			sb.AppendLine("GW2 Key: " + key!.Gw2Key);
			if (key!.Octave.HasValue)
			{
				sb.AppendLine($"Octave: {key!.Octave.Value}");
			}
			if (key!.HasAltOctave && key!.AltOctave.HasValue && !string.IsNullOrEmpty(key!.AltOctaveKey))
			{
				sb.AppendLine($"Also plays as {key!.AltOctaveKey} on octave {key!.AltOctave.Value}");
			}
			return sb.ToString().TrimEnd();
		}

		private void RebuildKeyRects()
		{
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			_keyRects.Clear();
			if (_layout.IsEmpty)
			{
				return;
			}
			_layout.Keys.Count((KeybedKey k) => !k.IsBlackKey);
			float whiteKeyWidth = 24f;
			float blackKeyWidth = whiteKeyWidth * 0.65f;
			int whiteKeyHeight;
			int blackKeyHeight = (int)((float)(whiteKeyHeight = 88) * 0.6f);
			float x = 4f;
			int y = 4;
			Dictionary<int, Rectangle> whiteRects = new Dictionary<int, Rectangle>();
			Rectangle rect = default(Rectangle);
			foreach (KeybedKey key2 in _layout.Keys)
			{
				if (!key2.IsBlackKey)
				{
					((Rectangle)(ref rect))._002Ector((int)x, y, (int)whiteKeyWidth, whiteKeyHeight);
					_keyRects[key2.NoteNumber] = rect;
					whiteRects[key2.NoteNumber] = rect;
					x += whiteKeyWidth;
				}
			}
			Rectangle rect2 = default(Rectangle);
			foreach (KeybedKey key in _layout.Keys)
			{
				if (key.IsBlackKey && GetPrecedingWhiteKeyIndex(key.NoteNumber % 12) >= 0)
				{
					int prevNote = key.NoteNumber - 1;
					if (whiteRects.TryGetValue(prevNote, out var prevRect))
					{
						float blackX = (float)((Rectangle)(ref prevRect)).get_Right() - blackKeyWidth / 2f;
						((Rectangle)(ref rect2))._002Ector((int)blackX, y, (int)blackKeyWidth, blackKeyHeight);
						_keyRects[key.NoteNumber] = rect2;
					}
				}
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			((Rectangle)(ref bounds)).Offset(((Control)this).get_AbsoluteBounds().X, ((Control)this).get_AbsoluteBounds().Y);
			if (_layout.IsEmpty)
			{
				DrawEmptyMessage(spriteBatch, bounds);
			}
			else
			{
				DrawKeys(spriteBatch, bounds);
			}
		}

		private static void DrawEmptyMessage(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = GameService.Content.get_DefaultFont16();
			Size2 size = font.MeasureString("No keymap to preview.");
			float x = (float)bounds.X + ((float)bounds.Width - size.Width) / 2f;
			float y = (float)bounds.Y + ((float)bounds.Height - size.Height) / 2f;
			BitmapFontExtensions.DrawString(spriteBatch, font, "No keymap to preview.", new Vector2(x, y), Color.get_Gray(), (Rectangle?)null);
		}

		private static void DrawKeys(SpriteBatch spriteBatch, Rectangle bounds, KeybedLayout layout, KeybedKey? hoveredKey, Dictionary<int, Rectangle> keyRects)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = GameService.Content.get_DefaultFont12();
			int whiteKeyCount = layout.Keys.Count((KeybedKey k) => !k.IsBlackKey);
			if (whiteKeyCount == 0)
			{
				return;
			}
			int num = bounds.Width - 8;
			int availableHeight = bounds.Height - 8 - 14;
			float whiteKeyWidth = (float)num / (float)whiteKeyCount;
			float blackKeyWidth = whiteKeyWidth * 0.65f;
			int whiteKeyHeight = availableHeight;
			int blackKeyHeight = (int)((float)availableHeight * 0.6f);
			Dictionary<int, Rectangle> whiteKeyRects = new Dictionary<int, Rectangle>();
			List<(string, Rectangle)> cNoteRects = new List<(string, Rectangle)>();
			float x = bounds.X + 4;
			int y = bounds.Y + 4;
			Rectangle rect2 = default(Rectangle);
			Rectangle gw2Rect = default(Rectangle);
			foreach (KeybedKey key2 in layout.Keys)
			{
				if (!key2.IsBlackKey)
				{
					((Rectangle)(ref rect2))._002Ector((int)x, y, (int)whiteKeyWidth, whiteKeyHeight);
					whiteKeyRects[key2.NoteNumber] = rect2;
					if (key2.NoteNumber % 12 == 0)
					{
						cNoteRects.Add((key2.NoteName, rect2));
					}
					Color fillColor2 = (key2.IsMapped ? Color.FromNonPremultiplied(245, 245, 245, 255) : Color.FromNonPremultiplied(190, 190, 190, 255));
					Color borderColor2 = (key2.IsKeySwitch ? Color.get_Orange() : Color.FromNonPremultiplied(120, 120, 120, 255));
					DrawFilledRect(spriteBatch, rect2, fillColor2);
					DrawRectBorder(spriteBatch, rect2, borderColor2, 1);
					if (key2.IsMapped && key2.Gw2Key != null)
					{
						((Rectangle)(ref gw2Rect))._002Ector(rect2.X, rect2.Y + (int)((float)rect2.Height * 0.55f), rect2.Width, (int)((float)rect2.Height * 0.45f));
						DrawCenteredString(spriteBatch, font, key2.Gw2Key, gw2Rect, Color.get_Black());
					}
					x += whiteKeyWidth;
				}
			}
			Rectangle rect3 = default(Rectangle);
			foreach (KeybedKey key in layout.Keys)
			{
				if (key.IsBlackKey && GetPrecedingWhiteKeyIndex(key.NoteNumber % 12) >= 0 && whiteKeyRects.TryGetValue(key.NoteNumber - 1, out var prevWhiteRect))
				{
					float blackX = (float)((Rectangle)(ref prevWhiteRect)).get_Right() - blackKeyWidth / 2f;
					((Rectangle)(ref rect3))._002Ector((int)blackX, y, (int)blackKeyWidth, blackKeyHeight);
					Color fillColor = (key.IsMapped ? Color.FromNonPremultiplied(30, 30, 30, 255) : Color.FromNonPremultiplied(70, 70, 70, 255));
					Color borderColor = (key.IsKeySwitch ? Color.get_Orange() : Color.FromNonPremultiplied(50, 50, 50, 255));
					DrawFilledRect(spriteBatch, rect3, fillColor);
					DrawRectBorder(spriteBatch, rect3, borderColor, 1);
					if (key.IsMapped && key.Gw2Key != null)
					{
						DrawCenteredString(spriteBatch, font, key.Gw2Key, rect3, Color.get_White());
					}
				}
			}
			if (hoveredKey != null && keyRects.TryGetValue(hoveredKey!.NoteNumber, out var hoverLocalRect))
			{
				Rectangle hoverScreenRect = default(Rectangle);
				((Rectangle)(ref hoverScreenRect))._002Ector(bounds.X + 4 + hoverLocalRect.X - 4, bounds.Y + 4 + hoverLocalRect.Y - 4, hoverLocalRect.Width, hoverLocalRect.Height);
				Color tint = (hoveredKey!.IsMapped ? new Color(255, 165, 0, 77) : new Color(176, 196, 222, 77));
				DrawFilledRect(spriteBatch, hoverScreenRect, tint);
			}
			int labelY = bounds.Y + 4 + availableHeight + 2;
			Rectangle labelRect = default(Rectangle);
			foreach (var (noteName, rect) in cNoteRects)
			{
				((Rectangle)(ref labelRect))._002Ector(rect.X, labelY, rect.Width, 12);
				DrawCenteredString(spriteBatch, font, noteName, labelRect, Color.get_LightGray());
			}
		}

		private void DrawKeys(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			DrawKeys(spriteBatch, bounds, _layout, _hoveredKey, _keyRects);
		}

		private static int GetPrecedingWhiteKeyIndex(int semitone)
		{
			return semitone switch
			{
				1 => 0, 
				3 => 1, 
				6 => 3, 
				8 => 4, 
				10 => 5, 
				_ => -1, 
			};
		}

		private static void DrawFilledRect(SpriteBatch spriteBatch, Rectangle rect, Color color)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.Draw(GetPixelTexture(), rect, color);
		}

		private static void DrawRectBorder(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			Texture2D px = GetPixelTexture();
			spriteBatch.Draw(px, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
			spriteBatch.Draw(px, new Rectangle(rect.X, ((Rectangle)(ref rect)).get_Bottom() - thickness, rect.Width, thickness), color);
			spriteBatch.Draw(px, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
			spriteBatch.Draw(px, new Rectangle(((Rectangle)(ref rect)).get_Right() - thickness, rect.Y, thickness, rect.Height), color);
		}

		private static void DrawCenteredString(SpriteBatch spriteBatch, BitmapFont font, string text, Rectangle rect, Color color)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			Size2 size = font.MeasureString(text);
			float x = (float)rect.X + ((float)rect.Width - size.Width) / 2f;
			float y = (float)rect.Y + ((float)rect.Height - size.Height) / 2f;
			BitmapFontExtensions.DrawString(spriteBatch, font, text, new Vector2(x, y), color, (Rectangle?)null);
		}

		private static Texture2D GetPixelTexture()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (_pixelTexture != null)
			{
				return _pixelTexture;
			}
			_pixelTexture = new Texture2D(GameService.Graphics.get_GraphicsDeviceManager().get_GraphicsDevice(), 1, 1);
			_pixelTexture!.SetData<Color>((Color[])(object)new Color[1] { Color.get_White() });
			return _pixelTexture;
		}
	}
}
