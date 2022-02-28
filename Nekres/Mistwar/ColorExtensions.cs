using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace Nekres.Mistwar
{
	public static class ColorExtensions
	{
		private static readonly Dictionary<ColorType, float[][]> ColorTypes = new Dictionary<ColorType, float[][]>
		{
			{
				ColorType.Normal,
				new float[5][]
				{
					new float[5] { 1f, 0f, 0f, 0f, 0f },
					new float[5] { 0f, 1f, 0f, 0f, 0f },
					new float[5] { 0f, 0f, 1f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0f, 0f, 0f, 0f, 1f }
				}
			},
			{
				ColorType.Protanopia,
				new float[5][]
				{
					new float[5] { 0.567f, 0.433f, 0f, 0f, 0f },
					new float[5] { 0.558f, 0.442f, 0f, 0f, 0f },
					new float[5] { 0f, 0.242f, 0.758f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0f, 0f, 0f, 0f, 1f }
				}
			},
			{
				ColorType.Protanomaly,
				new float[5][]
				{
					new float[5] { 0.817f, 0.183f, 0f, 0f, 0f },
					new float[5] { 0.333f, 0.667f, 0f, 0f, 0f },
					new float[5] { 0f, 0.125f, 0.875f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0f, 0f, 0f, 0f, 1f }
				}
			},
			{
				ColorType.Deuteranopia,
				new float[5][]
				{
					new float[5] { 0.625f, 0.375f, 0f, 0f, 0f },
					new float[5] { 0.7f, 0.3f, 0f, 0f, 0f },
					new float[5] { 0f, 0.3f, 0.7f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0f, 0f, 0f, 0f, 1f }
				}
			},
			{
				ColorType.Deuteranomaly,
				new float[5][]
				{
					new float[5] { 0.8f, 0.2f, 0f, 0f, 0f },
					new float[5] { 0.258f, 0.742f, 0f, 0f, 0f },
					new float[5] { 0f, 0.142f, 0.858f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0f, 0f, 0f, 0f, 1f }
				}
			},
			{
				ColorType.Tritanopia,
				new float[5][]
				{
					new float[5] { 0.95f, 0.05f, 0f, 0f, 0f },
					new float[5] { 0f, 0.433f, 0.567f, 0f, 0f },
					new float[5] { 0f, 0.475f, 0.525f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0f, 0f, 0f, 0f, 1f }
				}
			},
			{
				ColorType.Tritanomaly,
				new float[5][]
				{
					new float[5] { 0.967f, 0.033f, 0f, 0f, 0f },
					new float[5] { 0f, 0.733f, 0.267f, 0f, 0f },
					new float[5] { 0f, 0.183f, 0.817f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0f, 0f, 0f, 0f, 1f }
				}
			},
			{
				ColorType.Achromatopsia,
				new float[5][]
				{
					new float[5] { 0.299f, 0.587f, 0.114f, 0f, 0f },
					new float[5] { 0.299f, 0.587f, 0.114f, 0f, 0f },
					new float[5] { 0.299f, 0.587f, 0.114f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0f, 0f, 0f, 0f, 1f }
				}
			},
			{
				ColorType.Achromatomaly,
				new float[5][]
				{
					new float[5] { 0.618f, 0.32f, 0.062f, 0f, 0f },
					new float[5] { 0.163f, 0.775f, 0.062f, 0f, 0f },
					new float[5] { 0.163f, 0.32f, 0.516f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0f, 0f, 0f, 0f, 1f }
				}
			}
		};

		private static int Clamp(float n)
		{
			return (int)((n < 0f) ? 0f : ((n < 255f) ? n : 255f));
		}

		public static Color GetColorBlindType(this Color color, ColorType type, int overwriteAlpha = -1)
		{
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			float[][] i = ColorTypes[type];
			float n = (float)(int)((Color)(ref color)).get_R() * i[0][0] + (float)(int)((Color)(ref color)).get_G() * i[0][1] + (float)(int)((Color)(ref color)).get_B() * i[0][2] + (float)(int)((Color)(ref color)).get_A() * i[0][3] + i[0][4];
			float g = (float)(int)((Color)(ref color)).get_R() * i[1][0] + (float)(int)((Color)(ref color)).get_G() * i[1][1] + (float)(int)((Color)(ref color)).get_B() * i[1][2] + (float)(int)((Color)(ref color)).get_A() * i[1][3] + i[1][4];
			float b = (float)(int)((Color)(ref color)).get_R() * i[2][0] + (float)(int)((Color)(ref color)).get_G() * i[2][1] + (float)(int)((Color)(ref color)).get_B() * i[2][2] + (float)(int)((Color)(ref color)).get_A() * i[2][3] + i[2][4];
			float a = (float)(int)((Color)(ref color)).get_R() * i[3][0] + (float)(int)((Color)(ref color)).get_G() * i[3][1] + (float)(int)((Color)(ref color)).get_B() * i[3][2] + (float)(int)((Color)(ref color)).get_A() * i[3][3] + i[3][4];
			return new Color(Clamp(n), Clamp(g), Clamp(b), Clamp((overwriteAlpha > -1) ? ((float)overwriteAlpha) : a));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color ToDrawingColor(this Color mediaColor)
		{
			return Color.FromArgb(((Color)(ref mediaColor)).get_A(), ((Color)(ref mediaColor)).get_R(), ((Color)(ref mediaColor)).get_G(), ((Color)(ref mediaColor)).get_B());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Color ToMediaColor(this Color drawingColor)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			return new Color(drawingColor.R, drawingColor.G, drawingColor.B, drawingColor.A);
		}
	}
}
