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
				return Bounds.Size;
			}
			set
			{
				Bounds = new Rectangle(Bounds.Location, value);
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
			if (!_isDisposed && (FallBackTexture != null || Texture != null))
			{
				Vector2 valueOrDefault = origin.GetValueOrDefault();
				if (!origin.HasValue)
				{
					valueOrDefault = Vector2.Zero;
					origin = valueOrDefault;
				}
				float valueOrDefault2 = rotation.GetValueOrDefault();
				if (!rotation.HasValue)
				{
					valueOrDefault2 = 0f;
					rotation = valueOrDefault2;
				}
				Hovered = mousePos.HasValue && Bounds.Contains(mousePos.Value);
				Color valueOrDefault3 = color.GetValueOrDefault();
				if (!color.HasValue)
				{
					valueOrDefault3 = (((forceHover.GetValueOrDefault() || Hovered) && HoverDrawColor.HasValue) ? HoverDrawColor : DrawColor) ?? Color.White;
					color = valueOrDefault3;
				}
				if (Texture != null)
				{
					spriteBatch.DrawOnCtrl(ctrl, ((forceHover.GetValueOrDefault() || Hovered) && HoveredTexture != null) ? HoveredTexture : (Texture ?? FallBackTexture), Bounds, TextureRegion, color.Value, rotation.Value, origin.Value);
				}
				else
				{
					spriteBatch.DrawOnCtrl(ctrl, FallBackTexture, (FallbackBounds == Rectangle.Empty) ? Bounds : FallbackBounds, FallbackRegion, color.Value, rotation.Value, origin.Value);
				}
				if (bgColor.HasValue)
				{
					spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, Bounds, Rectangle.Empty, bgColor.Value, rotation.Value, origin.Value);
				}
			}
		}

		public virtual void Draw(Control ctrl, SpriteBatch spriteBatch, SpriteEffects? effect, Point? mousePos = null, Color? color = null, Color? bgColor = null, bool? forceHover = null, float? rotation = null, Vector2? origin = null)
		{
			if (!_isDisposed && (FallBackTexture != null || Texture != null))
			{
				SpriteEffects valueOrDefault = effect.GetValueOrDefault();
				if (!effect.HasValue)
				{
					valueOrDefault = SpriteEffects.FlipHorizontally;
					effect = valueOrDefault;
				}
				Vector2 valueOrDefault2 = origin.GetValueOrDefault();
				if (!origin.HasValue)
				{
					valueOrDefault2 = Vector2.Zero;
					origin = valueOrDefault2;
				}
				float valueOrDefault3 = rotation.GetValueOrDefault();
				if (!rotation.HasValue)
				{
					valueOrDefault3 = 0f;
					rotation = valueOrDefault3;
				}
				Hovered = mousePos.HasValue && Bounds.Contains(mousePos.Value);
				Color valueOrDefault4 = color.GetValueOrDefault();
				if (!color.HasValue)
				{
					valueOrDefault4 = ((Hovered && HoverDrawColor.HasValue) ? HoverDrawColor : DrawColor) ?? Color.White;
					color = valueOrDefault4;
				}
				if (Texture != null)
				{
					spriteBatch.DrawOnCtrl(ctrl, ((forceHover.GetValueOrDefault() || Hovered) && HoveredTexture != null) ? HoveredTexture : (Texture ?? FallBackTexture), Bounds, TextureRegion, color.Value, rotation.Value, origin.Value, effect.Value);
				}
				else
				{
					spriteBatch.DrawOnCtrl(ctrl, FallBackTexture, (FallbackBounds == Rectangle.Empty) ? Bounds : FallbackBounds, FallbackRegion, color.Value, rotation.Value, origin.Value, effect.Value);
				}
				if (bgColor.HasValue)
				{
					spriteBatch.DrawOnCtrl(ctrl, ContentService.Textures.Pixel, Bounds, Rectangle.Empty, bgColor.Value, rotation.Value, origin.Value, effect.Value);
				}
			}
		}

		private void ApplyBounds(bool force = false)
		{
			if (TextureRegion == Rectangle.Empty || force)
			{
				TextureRegion = (Texture ?? FallBackTexture).Bounds;
			}
			if (FallbackRegion == Rectangle.Empty || force)
			{
				FallbackRegion = (Texture ?? FallBackTexture).Bounds;
			}
			if (Bounds == Rectangle.Empty || force)
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
