using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage
{
	public class ItemTexture : DetailedTexture
	{
		private BaseItem? _item;

		private Color _frameColor;

		public BaseItem? Item
		{
			get
			{
				return _item;
			}
			set
			{
				Common.SetProperty(ref _item, value, new Action(ApplyItem));
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
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			_frameColor = ((Item != null) ? (Item?.Rarity.GetColor()).Value : (Color.get_White() * 0.5f));
			base.Texture = Item?.Icon;
		}

		public void Draw(Control ctrl, SpriteBatch spriteBatch, Point? mousePos = null, Color? color = null)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			if (base.FallBackTexture != null || base.Texture != null)
			{
				int hovered;
				if (mousePos.HasValue)
				{
					Rectangle bounds = base.Bounds;
					hovered = (((Rectangle)(ref bounds)).Contains(mousePos.Value) ? 1 : 0);
				}
				else
				{
					hovered = 0;
				}
				base.Hovered = (byte)hovered != 0;
				Color valueOrDefault = color.GetValueOrDefault();
				if (!color.HasValue)
				{
					valueOrDefault = (Color)(((_003F?)((base.Hovered && base.HoverDrawColor.HasValue) ? base.HoverDrawColor : base.DrawColor)) ?? Color.get_White());
					color = valueOrDefault;
				}
				if (base.Texture != null)
				{
					spriteBatch.DrawOnCtrl(ctrl, base.Texture, base.Bounds.Add(2, 2, -4, -4), base.TextureRegion, color.Value, 0f, Vector2.get_Zero(), (SpriteEffects)0);
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
