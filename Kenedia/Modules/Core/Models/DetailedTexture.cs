using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Models
{
	public class DetailedTexture
	{
		private AsyncTexture2D _texture;

		private AsyncTexture2D _hoveredTexture;

		public bool Hovered { get; private set; }

		public AsyncTexture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				Common.SetProperty(ref _texture, value, delegate
				{
					ApplyBounds();
				}, value != null);
			}
		}

		public AsyncTexture2D HoveredTexture
		{
			get
			{
				return _hoveredTexture;
			}
			set
			{
				Common.SetProperty(ref _hoveredTexture, value, delegate
				{
					ApplyBounds();
				}, value != null);
			}
		}

		public Rectangle TextureRegion { get; set; }

		public Rectangle Bounds { get; set; }

		public DetailedTexture()
		{
		}

		public DetailedTexture(int assetId)
		{
			Texture = AsyncTexture2D.FromAssetId(assetId);
		}

		public DetailedTexture(int assetId, int hoveredAssetId)
		{
			Texture = AsyncTexture2D.FromAssetId(assetId);
			HoveredTexture = AsyncTexture2D.FromAssetId(hoveredAssetId);
		}

		public DetailedTexture(AsyncTexture2D texture)
		{
			Texture = texture;
		}

		public void Draw(Control ctrl, SpriteBatch spriteBatch, Point? mousePos = null, Color? color = null, Color? bgColor = null, bool? forceHover = null, float rotation = 0f, Vector2? origin = null)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			if (Texture == null)
			{
				return;
			}
			Vector2 valueOrDefault = origin.GetValueOrDefault();
			if (!origin.HasValue)
			{
				valueOrDefault = Vector2.get_Zero();
				origin = valueOrDefault;
			}
			Color valueOrDefault2 = color.GetValueOrDefault();
			if (!color.HasValue)
			{
				valueOrDefault2 = Color.get_White();
				color = valueOrDefault2;
			}
			Point valueOrDefault3 = mousePos.GetValueOrDefault();
			if (!mousePos.HasValue)
			{
				valueOrDefault3 = Point.get_Zero();
				mousePos = valueOrDefault3;
			}
			int hovered;
			if (forceHover != true)
			{
				if (!forceHover.HasValue)
				{
					Rectangle bounds = Bounds;
					hovered = (((Rectangle)(ref bounds)).Contains(mousePos.Value) ? 1 : 0);
				}
				else
				{
					hovered = 0;
				}
			}
			else
			{
				hovered = 1;
			}
			Hovered = (byte)hovered != 0;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, AsyncTexture2D.op_Implicit((Hovered && HoveredTexture != null) ? HoveredTexture : Texture), Bounds, (Rectangle?)TextureRegion, color.Value, rotation, origin.Value, (SpriteEffects)0);
			if (bgColor.HasValue)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), Bounds, (Rectangle?)Rectangle.get_Empty(), bgColor.Value, rotation, origin.Value, (SpriteEffects)0);
			}
		}

		private void ApplyBounds(bool force = false)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			if (TextureRegion == Rectangle.get_Empty() || force)
			{
				TextureRegion = Texture.get_Bounds();
			}
			if (Bounds == Rectangle.get_Empty() || force)
			{
				Bounds = Texture.get_Bounds();
			}
		}
	}
}
