using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeaStove.Gw2Tldr.UI
{
	internal sealed class ToolbarIcon : Control
	{
		private readonly string _kind;

		private Texture2D _smoothIcon;

		public bool Active { get; set; }

		public bool Expanded { get; set; } = true;


		public ToolbarIcon(string kind)
			: this()
		{
			_kind = kind;
		}

		protected override void Paint(SpriteBatch batch, Rectangle bounds)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			Color color = (Color)((!Active) ? (OverlayTheme.Light ? Color.get_DarkSlateGray() : Color.get_LightGray()) : (OverlayTheme.Light ? new Color(145, 70, 0) : new Color(255, 180, 90)));
			if (_kind == "search" || _kind == "book" || _kind == "auto" || _kind == "up" || _kind == "down")
			{
				if (_smoothIcon == null)
				{
					_smoothIcon = CreateSmoothIcon(_kind);
				}
				SpriteBatchExtensions.DrawOnCtrl(batch, (Control)(object)this, _smoothIcon, new Rectangle(0, 0, 20, 20), color);
				return;
			}
			string kind = _kind;
			if (kind != null)
			{
				switch (kind.Length)
				{
				case 6:
					switch (kind[0])
					{
					case 's':
					{
						if (!(kind == "search"))
						{
							break;
						}
						Rect(3, 2, 9, 2);
						Rect(3, 11, 9, 2);
						Rect(1, 4, 2, 7);
						Rect(12, 4, 2, 7);
						for (int l = 0; l < 6; l++)
						{
							Rect(12 + l, 12 + l, 2, 2);
						}
						return;
					}
					case 'd':
						if (!(kind == "delete"))
						{
							break;
						}
						Rect(4, 4, 12, 2);
						Rect(7, 2, 6, 2);
						Rect(5, 6, 2, 11);
						Rect(13, 6, 2, 11);
						Rect(5, 16, 10, 2);
						Rect(8, 8, 2, 6);
						Rect(11, 8, 2, 6);
						return;
					case 'r':
						if (!(kind == "resize"))
						{
							break;
						}
						Rect(12, 16, 6, 2);
						Rect(16, 12, 2, 6);
						Rect(8, 16, 2, 2);
						Rect(16, 8, 2, 2);
						return;
					}
					break;
				case 4:
					switch (kind[0])
					{
					case 'b':
						if (!(kind == "book"))
						{
							break;
						}
						Rect(2, 3, 7, 2);
						Rect(11, 3, 7, 2);
						Rect(2, 5, 2, 11);
						Rect(17, 5, 2, 11);
						Rect(3, 15, 6, 2);
						Rect(11, 15, 6, 2);
						Rect(9, 5, 2, 13);
						return;
					case 'a':
						if (!(kind == "auto"))
						{
							break;
						}
						Rect(3, 4, 12, 2);
						Rect(14, 4, 2, 6);
						Rect(11, 7, 5, 2);
						Rect(5, 14, 12, 2);
						Rect(4, 10, 2, 6);
						Rect(4, 10, 5, 2);
						return;
					case 'd':
						if (!(kind == "down"))
						{
							break;
						}
						Rect(9, 3, 2, 12);
						Rect(7, 12, 2, 2);
						Rect(5, 10, 2, 2);
						Rect(11, 12, 2, 2);
						Rect(13, 10, 2, 2);
						return;
					case 'c':
						if (!(kind == "copy"))
						{
							break;
						}
						Rect(3, 2, 10, 2);
						Rect(3, 2, 2, 10);
						Rect(7, 6, 10, 2);
						Rect(7, 6, 2, 11);
						Rect(15, 6, 2, 11);
						Rect(7, 15, 10, 2);
						return;
					case 'l':
						if (!(kind == "lock"))
						{
							break;
						}
						Rect(5, 8, 12, 2);
						Rect(5, 8, 2, 9);
						Rect(15, 8, 2, 9);
						Rect(5, 15, 12, 2);
						Rect(7, 3, 2, 6);
						Rect(13, 3, 2, 6);
						Rect(8, 2, 6, 2);
						return;
					case 'e':
					{
						if (!(kind == "edit"))
						{
							break;
						}
						for (int i = 0; i < 11; i++)
						{
							Rect(4 + i, 14 - i, 3, 3);
						}
						Rect(3, 15, 4, 2);
						Rect(14, 3, 3, 3);
						return;
					}
					case 'n':
					{
						if (!(kind == "next"))
						{
							break;
						}
						for (int j = 0; j < 6; j++)
						{
							Rect(5 + j, 5 + j, 2, 2);
							Rect(10 - j, 11 + j, 2, 2);
						}
						return;
					}
					}
					break;
				case 8:
					switch (kind[0])
					{
					case 'p':
					{
						if (!(kind == "previous"))
						{
							break;
						}
						for (int m = 0; m < 6; m++)
						{
							Rect(10 - m, 5 + m, 2, 2);
							Rect(5 + m, 11 + m, 2, 2);
						}
						return;
					}
					case 'c':
						if (!(kind == "collapse"))
						{
							break;
						}
						Rect(4, 10, 13, 2);
						if (!Expanded)
						{
							Rect(10, 4, 2, 14);
						}
						return;
					}
					break;
				case 2:
					if (!(kind == "up"))
					{
						break;
					}
					Rect(9, 5, 2, 12);
					Rect(7, 6, 2, 2);
					Rect(5, 8, 2, 2);
					Rect(11, 6, 2, 2);
					Rect(13, 8, 2, 2);
					return;
				case 5:
				{
					if (!(kind == "close"))
					{
						break;
					}
					for (int k = 0; k < 11; k++)
					{
						Rect(4 + k, 4 + k, 2, 2);
						Rect(14 - k, 4 + k, 2, 2);
					}
					return;
				}
				}
			}
			Rect(3, 5, 3, 3);
			Rect(9, 5, 3, 3);
			Rect(15, 5, 3, 3);
			void Rect(int x, int y, int width, int height)
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				SpriteBatchExtensions.DrawOnCtrl(batch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(x, y, width, height), color);
			}
		}

		private static Texture2D CreateSmoothIcon(string kind)
		{
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c3: Expected O, but got Unknown
			int num;
			switch (kind)
			{
			default:
				num = 64;
				break;
			case "auto":
			case "up":
			case "down":
				num = 20;
				break;
			}
			int size = num;
			Color[] pixels = (Color[])(object)new Color[size * size];
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					float px = ((float)x + 0.5f) / (float)size * 20f;
					float py = ((float)y + 0.5f) / (float)size * 20f;
					float distance;
					switch (kind)
					{
					case "up":
					case "down":
					{
						float tip = ((kind == "up") ? 5f : 15f);
						float baseY = ((kind == "up") ? 11f : 9f);
						float tail = ((kind == "up") ? 16f : 4f);
						distance = Math.Min(Segment(px, py, 4f, baseY, 10f, tip), Segment(px, py, 10f, tip, 16f, baseY));
						distance = Math.Min(distance, Segment(px, py, 10f, tip, 10f, tail));
						break;
					}
					case "auto":
					{
						float dx = px - 10f;
						float dy = py - 10f;
						distance = Math.Abs((float)Math.Sqrt(dx * dx + dy * dy) - 5.4f);
						distance = Math.Min(distance, Segment(px, py, 10f, 1.5f, 10f, 5f));
						distance = Math.Min(distance, Segment(px, py, 10f, 15f, 10f, 18.5f));
						distance = Math.Min(distance, Segment(px, py, 1.5f, 10f, 5f, 10f));
						distance = Math.Min(distance, Segment(px, py, 15f, 10f, 18.5f, 10f));
						distance = Math.Min(distance, (float)Math.Sqrt(dx * dx + dy * dy));
						break;
					}
					case "search":
					{
						float num2 = px - 8f;
						float dy2 = py - 8f;
						distance = Math.Min(Math.Abs((float)Math.Sqrt(num2 * num2 + dy2 * dy2) - 5f), Segment(px, py, 12f, 12f, 17f, 17f));
						break;
					}
					default:
						distance = Math.Min(Segment(px, py, 2f, 4f, 2f, 15f), Segment(px, py, 18f, 4f, 18f, 15f));
						distance = Math.Min(distance, Segment(px, py, 10f, 5f, 10f, 17f));
						distance = Math.Min(distance, Segment(px, py, 2f, 4f, 7f, 4f));
						distance = Math.Min(distance, Segment(px, py, 7f, 4f, 10f, 6f));
						distance = Math.Min(distance, Segment(px, py, 10f, 6f, 13f, 4f));
						distance = Math.Min(distance, Segment(px, py, 13f, 4f, 18f, 4f));
						distance = Math.Min(distance, Segment(px, py, 2f, 15f, 7f, 15f));
						distance = Math.Min(distance, Segment(px, py, 7f, 15f, 10f, 17f));
						distance = Math.Min(distance, Segment(px, py, 10f, 17f, 13f, 15f));
						distance = Math.Min(distance, Segment(px, py, 13f, 15f, 18f, 15f));
						break;
					}
					float val;
					switch (kind)
					{
					default:
						val = (1.05f - distance) * 3.2f;
						break;
					case "auto":
					case "up":
					case "down":
						val = 1.2f - distance;
						break;
					}
					float coverage = Math.Max(0f, Math.Min(1f, val));
					pixels[y * size + x] = new Color(coverage, coverage, coverage, coverage);
				}
			}
			Texture2D val2 = new Texture2D(((GraphicsResource)Textures.get_Pixel()).get_GraphicsDevice(), size, size);
			val2.SetData<Color>(pixels);
			return val2;
		}

		private static float Segment(float x, float y, float x1, float y1, float x2, float y2)
		{
			float dx = x2 - x1;
			float dy = y2 - y1;
			float t = Math.Max(0f, Math.Min(1f, ((x - x1) * dx + (y - y1) * dy) / (dx * dx + dy * dy)));
			float num = x - (x1 + t * dx);
			float ey = y - (y1 + t * dy);
			return (float)Math.Sqrt(num * num + ey * ey);
		}

		protected override void DisposeControl()
		{
			Texture2D smoothIcon = _smoothIcon;
			if (smoothIcon != null)
			{
				((GraphicsResource)smoothIcon).Dispose();
			}
			_smoothIcon = null;
			((Control)this).DisposeControl();
		}
	}
}
