using System;
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

		private AsyncTexture2D _fallbackTexture;

		public bool Hovered { get; protected set; }

		public AsyncTexture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				Common.SetProperty(ref _texture, value, (Action)delegate
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
				Common.SetProperty(ref _hoveredTexture, value, (Action)delegate
				{
					ApplyBounds();
				}, value != null);
			}
		}

		public AsyncTexture2D FallBackTexture
		{
			get
			{
				return _fallbackTexture;
			}
			set
			{
				Common.SetProperty(ref _fallbackTexture, value, (Action)delegate
				{
					ApplyBounds();
				}, value != null);
			}
		}

		public Rectangle TextureRegion { get; set; }

		public Rectangle FallbackRegion { get; set; }

		public Rectangle Bounds { get; set; }

		public Rectangle FallbackBounds { get; set; }

		public Color? DrawColor { get; set; }

		public Color? HoverDrawColor { get; set; }

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

		public virtual void Draw(Control ctrl, SpriteBatch spriteBatch, Point? mousePos = null, Color? color = null, Color? bgColor = null, bool? forceHover = null, float? rotation = null, Vector2? origin = null)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			if (FallBackTexture != null || Texture != null)
			{
				Vector2 valueOrDefault = origin.GetValueOrDefault();
				if (!origin.HasValue)
				{
					valueOrDefault = Vector2.get_Zero();
					origin = valueOrDefault;
				}
				float valueOrDefault2 = rotation.GetValueOrDefault();
				if (!rotation.HasValue)
				{
					valueOrDefault2 = 0f;
					rotation = valueOrDefault2;
				}
				int hovered;
				if (mousePos.HasValue)
				{
					Rectangle bounds = Bounds;
					hovered = (((Rectangle)(ref bounds)).Contains(mousePos.Value) ? 1 : 0);
				}
				else
				{
					hovered = 0;
				}
				Hovered = (byte)hovered != 0;
				Color valueOrDefault3 = color.GetValueOrDefault();
				if (!color.HasValue)
				{
					valueOrDefault3 = (Color)(((_003F?)((Hovered && HoverDrawColor.HasValue) ? HoverDrawColor : DrawColor)) ?? Color.get_White());
					color = valueOrDefault3;
				}
				if (Texture != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, AsyncTexture2D.op_Implicit(((forceHover == true || Hovered) && HoveredTexture != null) ? HoveredTexture : (Texture ?? FallBackTexture)), Bounds, (Rectangle?)TextureRegion, color.Value, rotation.Value, origin.Value, (SpriteEffects)0);
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, AsyncTexture2D.op_Implicit(FallBackTexture), (FallbackBounds == Rectangle.get_Empty()) ? Bounds : FallbackBounds, (Rectangle?)FallbackRegion, color.Value, rotation.Value, origin.Value, (SpriteEffects)0);
				}
				if (bgColor.HasValue)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), Bounds, (Rectangle?)Rectangle.get_Empty(), bgColor.Value, rotation.Value, origin.Value, (SpriteEffects)0);
				}
			}
		}

		public virtual void Draw(Control ctrl, SpriteBatch spriteBatch, SpriteEffects? effect, Point? mousePos = null, Color? color = null, Color? bgColor = null, bool? forceHover = null, float? rotation = null, Vector2? origin = null)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			if (FallBackTexture != null || Texture != null)
			{
				SpriteEffects valueOrDefault = effect.GetValueOrDefault();
				if (!effect.HasValue)
				{
					valueOrDefault = (SpriteEffects)1;
					effect = valueOrDefault;
				}
				Vector2 valueOrDefault2 = origin.GetValueOrDefault();
				if (!origin.HasValue)
				{
					valueOrDefault2 = Vector2.get_Zero();
					origin = valueOrDefault2;
				}
				float valueOrDefault3 = rotation.GetValueOrDefault();
				if (!rotation.HasValue)
				{
					valueOrDefault3 = 0f;
					rotation = valueOrDefault3;
				}
				int hovered;
				if (mousePos.HasValue)
				{
					Rectangle bounds = Bounds;
					hovered = (((Rectangle)(ref bounds)).Contains(mousePos.Value) ? 1 : 0);
				}
				else
				{
					hovered = 0;
				}
				Hovered = (byte)hovered != 0;
				Color valueOrDefault4 = color.GetValueOrDefault();
				if (!color.HasValue)
				{
					valueOrDefault4 = (Color)(((_003F?)((Hovered && HoverDrawColor.HasValue) ? HoverDrawColor : DrawColor)) ?? Color.get_White());
					color = valueOrDefault4;
				}
				if (Texture != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, AsyncTexture2D.op_Implicit(((forceHover == true || Hovered) && HoveredTexture != null) ? HoveredTexture : (Texture ?? FallBackTexture)), Bounds, (Rectangle?)TextureRegion, color.Value, rotation.Value, origin.Value, effect.Value);
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, AsyncTexture2D.op_Implicit(FallBackTexture), (FallbackBounds == Rectangle.get_Empty()) ? Bounds : FallbackBounds, (Rectangle?)FallbackRegion, color.Value, rotation.Value, origin.Value, effect.Value);
				}
				if (bgColor.HasValue)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ctrl, Textures.get_Pixel(), Bounds, (Rectangle?)Rectangle.get_Empty(), bgColor.Value, rotation.Value, origin.Value, effect.Value);
				}
			}
		}

		private void ApplyBounds(bool force = false)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			if (TextureRegion == Rectangle.get_Empty() || force)
			{
				TextureRegion = (Texture ?? FallBackTexture).get_Bounds();
			}
			if (FallbackRegion == Rectangle.get_Empty() || force)
			{
				FallbackRegion = (Texture ?? FallBackTexture).get_Bounds();
			}
			if (Bounds == Rectangle.get_Empty() || force)
			{
				Bounds = (Texture ?? FallBackTexture).get_Bounds();
			}
		}
	}
}
