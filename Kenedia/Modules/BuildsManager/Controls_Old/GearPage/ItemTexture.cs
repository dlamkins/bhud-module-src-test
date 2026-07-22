using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage
{
	public class ItemTexture : DetailedTexture
	{
		private Color _frameColor;

		public BaseItem? Item
		{
			[CompilerGenerated]
			get
			{
				return _003CItem_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CItem_003Ek__BackingField, value, delegate(BaseItem v)
				{
					_003CItem_003Ek__BackingField = v;
				}, new Action(ApplyItem));
			}
		}

		public Container Parent { get; set; }

		public ItemTexture()
		{
		}

		public ItemTexture(Container parent)
		{
			Parent = parent;
		}

		private void ApplyItem()
		{
			_frameColor = ((Item != null) ? (Item?.Rarity.GetColor()).Value : (Color.White * 0.5f));
			base.Texture = TexturesService.GetAsyncTexture(Item?.AssetId);
		}

		public void Draw(Control ctrl, SpriteBatch spriteBatch, Point? mousePos = null, Color? color = null)
		{
			if (base.FallBackTexture != null || base.Texture != null)
			{
				base.Hovered = mousePos.HasValue && base.Bounds.Contains(mousePos.Value);
				Color valueOrDefault = color.GetValueOrDefault();
				if (!color.HasValue)
				{
					valueOrDefault = ((base.Hovered && base.HoverDrawColor.HasValue) ? base.HoverDrawColor : base.DrawColor) ?? Color.White;
					color = valueOrDefault;
				}
				if (base.Texture != null)
				{
					spriteBatch.DrawOnCtrl(ctrl, base.Texture, base.Bounds.Add(2, 2, -4, -4), base.TextureRegion, color.Value, 0f, Vector2.Zero);
				}
				spriteBatch.DrawFrame(ctrl, base.Bounds, _frameColor, 2);
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			Item = null;
		}
	}
}
