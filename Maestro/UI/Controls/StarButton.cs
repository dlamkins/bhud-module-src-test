using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Controls
{
	public class StarButton : Control
	{
		private const int TEXTURE_SIZE = 16;

		private const int CONTROL_SIZE = 20;

		private static Texture2D _starTexture;

		private bool _isFavorite;

		public bool IsFavorite
		{
			get
			{
				return _isFavorite;
			}
			set
			{
				_isFavorite = value;
				((Control)this).set_BasicTooltipText(value ? "Remove from Favorites" : "Add to Favorites");
			}
		}

		public StarButton()
			: this()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			base._size = new Point(20, 20);
			((Control)this).set_BasicTooltipText("Add to Favorites");
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			Color color = (_isFavorite ? MaestroTheme.AmberGold : (((Control)this).get_MouseOver() ? MaestroTheme.MutedCream : MaestroTheme.MediumGray));
			Texture2D texture = GetStarTexture();
			int ox = 2;
			int oy = 2;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, new Rectangle(ox, oy, 16, 16), color);
		}

		private static Texture2D GetStarTexture()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (_starTexture != null)
			{
				return _starTexture;
			}
			GraphicsDeviceContext context = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				_starTexture = CreateStarTexture(((GraphicsDeviceContext)(ref context)).get_GraphicsDevice(), 16);
			}
			finally
			{
				((GraphicsDeviceContext)(ref context)).Dispose();
			}
			return _starTexture;
		}

		private static Texture2D CreateStarTexture(GraphicsDevice device, int size)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Expected O, but got Unknown
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			Texture2D texture = new Texture2D(device, size, size);
			Color[] data = (Color[])(object)new Color[size * size];
			float outerRadius = (float)size / 2f - 0.5f;
			float innerRadius = outerRadius * 0.38f;
			float cx = (float)size / 2f;
			float cy = (float)size / 2f;
			Vector2[] vertices = (Vector2[])(object)new Vector2[10];
			for (int i = 0; i < 10; i++)
			{
				double angle = -Math.PI / 2.0 + (double)i * Math.PI / 5.0;
				float radius = ((i % 2 == 0) ? outerRadius : innerRadius);
				vertices[i] = new Vector2(cx + (float)((double)radius * Math.Cos(angle)), cy + (float)((double)radius * Math.Sin(angle)));
			}
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					float coverage = GetPixelCoverage(x, y, vertices);
					if (coverage > 0f)
					{
						byte alpha = (byte)(coverage * 255f);
						data[y * size + x] = new Color(alpha, alpha, alpha, alpha);
					}
				}
			}
			texture.SetData<Color>(data);
			return texture;
		}

		private static float GetPixelCoverage(int px, int py, Vector2[] vertices)
		{
			int inside = 0;
			for (int sy = 0; sy < 4; sy++)
			{
				for (int sx = 0; sx < 4; sx++)
				{
					float px2 = (float)px + 0.125f + (float)sx * 0.25f;
					float y = (float)py + 0.125f + (float)sy * 0.25f;
					if (IsPointInPolygon(px2, y, vertices))
					{
						inside++;
					}
				}
			}
			return (float)inside / 16f;
		}

		private static bool IsPointInPolygon(float px, float py, Vector2[] vertices)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			bool inside = false;
			int k = vertices.Length;
			int i = 0;
			int j = k - 1;
			while (i < k)
			{
				Vector2 vi = vertices[i];
				Vector2 vj = vertices[j];
				if (vi.Y > py != vj.Y > py && px < (vj.X - vi.X) * (py - vi.Y) / (vj.Y - vi.Y) + vi.X)
				{
					inside = !inside;
				}
				j = i++;
			}
			return inside;
		}
	}
}
