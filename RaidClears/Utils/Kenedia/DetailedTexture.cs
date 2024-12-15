using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Utils.Kenedia
{
	public class DetailedTexture : IDisposable
	{
		private bool _isDisposed;

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

		public Point Size
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				Rectangle bounds = Bounds;
				return ((Rectangle)(ref bounds)).get_Size();
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Rectangle bounds = Bounds;
				Bounds = new Rectangle(((Rectangle)(ref bounds)).get_Location(), value);
			}
		}

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
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			if (!_isDisposed && (FallBackTexture != null || Texture != null))
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
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			if (!_isDisposed && (FallBackTexture != null || Texture != null))
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

		public virtual void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Texture = null;
				HoveredTexture = null;
				FallBackTexture = null;
			}
		}
	}
}
