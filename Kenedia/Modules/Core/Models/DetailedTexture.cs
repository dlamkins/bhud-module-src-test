using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Models
{
	public class DetailedTexture : IDisposable
	{
		private bool _isDisposed;

		public bool Hovered { get; protected set; }

		public AsyncTexture2D Texture
		{
			[CompilerGenerated]
			get
			{
				return _003CTexture_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CTexture_003Ek__BackingField, value, (Action<AsyncTexture2D>)delegate(AsyncTexture2D v)
				{
					_003CTexture_003Ek__BackingField = v;
				}, (Action)delegate
				{
					ApplyBounds();
				}, value != null);
			}
		}

		public AsyncTexture2D HoveredTexture
		{
			[CompilerGenerated]
			get
			{
				return _003CHoveredTexture_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CHoveredTexture_003Ek__BackingField, value, (Action<AsyncTexture2D>)delegate(AsyncTexture2D v)
				{
					_003CHoveredTexture_003Ek__BackingField = v;
				}, (Action)delegate
				{
					ApplyBounds();
				}, value != null);
			}
		}

		public AsyncTexture2D FallBackTexture
		{
			[CompilerGenerated]
			get
			{
				return _003CFallBackTexture_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CFallBackTexture_003Ek__BackingField, value, (Action<AsyncTexture2D>)delegate(AsyncTexture2D v)
				{
					_003CFallBackTexture_003Ek__BackingField = v;
				}, (Action)delegate
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
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
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
					valueOrDefault3 = (Color)(((_003F?)(((forceHover.GetValueOrDefault() || Hovered) && HoverDrawColor.HasValue) ? HoverDrawColor : DrawColor)) ?? Color.get_White());
					color = valueOrDefault3;
				}
				if (Texture != null)
				{
					spriteBatch.DrawOnCtrl(ctrl, ((forceHover.GetValueOrDefault() || Hovered) && HoveredTexture != null) ? HoveredTexture : (Texture ?? FallBackTexture), Bounds, TextureRegion, color.Value, rotation.Value, origin.Value, (SpriteEffects)0);
				}
				else
				{
					spriteBatch.DrawOnCtrl(ctrl, FallBackTexture, (FallbackBounds == Rectangle.get_Empty()) ? Bounds : FallbackBounds, FallbackRegion, color.Value, rotation.Value, origin.Value, (SpriteEffects)0);
				}
				if (bgColor.HasValue)
				{
					spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, Bounds, Rectangle.get_Empty(), bgColor.Value, rotation.Value, origin.Value, (SpriteEffects)0);
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
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
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
					spriteBatch.DrawOnCtrl(ctrl, ((forceHover.GetValueOrDefault() || Hovered) && HoveredTexture != null) ? HoveredTexture : (Texture ?? FallBackTexture), Bounds, TextureRegion, color.Value, rotation.Value, origin.Value, effect.Value);
				}
				else
				{
					spriteBatch.DrawOnCtrl(ctrl, FallBackTexture, (FallbackBounds == Rectangle.get_Empty()) ? Bounds : FallbackBounds, FallbackRegion, color.Value, rotation.Value, origin.Value, effect.Value);
				}
				if (bgColor.HasValue)
				{
					spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, Bounds, Rectangle.get_Empty(), bgColor.Value, rotation.Value, origin.Value, effect.Value);
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
				TextureRegion = (Texture ?? FallBackTexture).Bounds;
			}
			if (FallbackRegion == Rectangle.get_Empty() || force)
			{
				FallbackRegion = (Texture ?? FallBackTexture).Bounds;
			}
			if (Bounds == Rectangle.get_Empty() || force)
			{
				Bounds = (Texture ?? FallBackTexture).Bounds;
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
